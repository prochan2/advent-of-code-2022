//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

List<Group> groups = new();

var group = new Group();

int lineNumber = 0;

foreach (var line in File.ReadLines(inputFile))
{
    Console.WriteLine(line);

    var rucksack = new Rucksack();

    foreach (var item in line)
    {
        rucksack.Add(item);
    }

    group.Add(rucksack);

    if (++lineNumber % 3 == 0)
    {
        groups.Add(group);
        group = new Group();
        Console.WriteLine();
    }
}

long total = groups.Sum(SumOfDuplicatesInGroup);

Console.WriteLine(total);

long SumOfDuplicatesInGroup(Group rucksack)
{
    long sum = 0;

    for (char item = 'a'; item <= 'z'; item++)
    {
        if (rucksack.AllContain(item))
        {
            Console.Write(item);
            sum += new Item(item).Priority;
        }
    }

    for (char item = 'A'; item <= 'Z'; item++)
    {
        if (rucksack.AllContain(item))
        {
            Console.Write(item);
            sum += new Item(item).Priority;
        }
    }

    Console.WriteLine(sum);

    return sum;
}

struct Item
{
    public Item()
    {
    }

    public Item(char name) : this()
    {
        Name = name;
        Priority = char.IsLower(Name) ? name - 'a' + 1 : name - 'A' + 27;
    }

    public char Name { get; }

    public long Priority { get; }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

class Rucksack
{
    private readonly HashSet<char> items = new();

    public void Add(char item) => items.Add(item);

    public bool Contains(char item) => items.Contains(item);
}

class Group
{
    private readonly List<Rucksack> compartments = new();

    public void Add(Rucksack compartment) => compartments.Add(compartment);

    public bool AllContain(char item) => compartments.All(c => c.Contains(item));
}