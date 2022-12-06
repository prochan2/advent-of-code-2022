//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

using var file = File.OpenRead(inputFile);

SlidingWindow window = new(14);

int i = 0;

while (file.Position < file.Length)
{
    var c = (char)file.ReadByte();

    window.Push(c);

    if (i++ >= 3 && window.IsContentDistinct())
    {
        Console.WriteLine(i);
        return;
    }
}

class SlidingWindow
{
    private readonly int size;

    private readonly Queue<char> queue;

    public SlidingWindow(int size)
    {
        this.size = size;
        this.queue = new(size);
    }

    public void Push(char c)
    {
        if (queue.Count == size)
        {
            queue.Dequeue();
        }

        queue.Enqueue(c);
    }

    public bool IsContentDistinct() => queue.Distinct().Count() == size;
}