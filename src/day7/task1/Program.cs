using System.Collections.Immutable;
using System.Xml.Linq;

//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

DirectoryNode rootDirectory = new();
DirectoryNode currentDirectory = rootDirectory;

bool lsInProgress = false;

foreach (var line in File.ReadLines(inputFile))
{
    if (line[0] == '$')
    {
        lsInProgress = false;

        switch (line.Substring(2, 2))
        {
            case "cd":
                string directoryName = line.Substring(5);

                switch (directoryName)
                {
                    case "/":
                        currentDirectory = rootDirectory;
                        break;

                    case "..":
                        if (currentDirectory.Parent == null)
                        {
                            throw new InvalidOperationException("Cannot change directory out of root.");
                        }

                        currentDirectory = currentDirectory.Parent;
                        break;

                    default:
                        currentDirectory = currentDirectory.GetOrCreateDirectory(directoryName);
                        break;
                }

                break;

            case "ls":
                lsInProgress = true;
                break;

            default:
                throw new InvalidOperationException($"Unexpected command: '{line}'");
        }
    }
    else
    {
        if (!lsInProgress)
        {
            throw new InvalidOperationException($"Unexpected output: '{line}'");
        }

        var parts = line.Split(' ');

        if (parts[0] != "dir")
        {
            var size = long.Parse(parts[0]);
            var name = parts[1];

            _ = currentDirectory.CreateFile(name, size);
        }
    }
}

var sum = FileSystem.Instance.Directories.Where(d => d.Size <= 100_000).Sum(d => d.Size);

Console.WriteLine(sum);

class FileSystem
{
    private List<FileNode> _files = new();

    private List<DirectoryNode> _directories = new();

    public static FileSystem Instance { get; } = new();

    public IEnumerable<FileNode> Files => _files;

    public IEnumerable<DirectoryNode> Directories => _directories;

    public void AddNode(Node node)
    {
        if (node is FileNode fileNode)
        {
            _files.Add(fileNode);
        }
        else if (node is DirectoryNode directoryNode)
        {
            _directories.Add(directoryNode);
        }
        else
        {
            throw new ArgumentException($"Unexpected node type '{node.GetType()}'.", nameof(node));
        }
    }
}

abstract class Node
{
    public string Name { get; }

    public long Size { get; protected set; }

    public DirectoryNode? Parent { get; }

    protected Node(string name, long size, DirectoryNode? parent = null)
    {
        Name = name;
        Size = size;
        Parent = parent;

        FileSystem.Instance.AddNode(this);
    }
}

class FileNode : Node
{
    public FileNode(string name, long size, DirectoryNode parent) : base(name, size, parent)
    {
    }
}

class DirectoryNode : Node
{
    private readonly Dictionary<string, Node> _nodes = new();

    public IEnumerable<Node> Nodes => _nodes.Values;

    public DirectoryNode() : base("", 0)
    {
    }

    private DirectoryNode(string name, DirectoryNode parent) : base(name, 0, parent)
    {
    }

    private void Add(Node node)
    {
        _nodes.Add(node.Name, node);

        if (node.Size == 0)
        {
            return;
        }

        DirectoryNode? currentDirectory = this;

        do
        {
            currentDirectory.Size += node.Size;
            currentDirectory = currentDirectory.Parent;
        }
        while (currentDirectory != null);
    }

    public DirectoryNode GetOrCreateDirectory(string directoryName)
    {
        if (_nodes.TryGetValue(directoryName, out var node))
        {
            if (node is DirectoryNode directory)
            {
                return directory;
            }
            else
            {
                throw new InvalidOperationException($"'{directoryName}' is not a directory.");
            }
        }
        else
        {
            var directory = new DirectoryNode(directoryName, this);
            this.Add(directory);
            return directory;
        }
    }

    public FileNode CreateFile(string name, long size)
    {
        FileNode file = new(name, size, this);
        this.Add(file);
        return file;
    }
}