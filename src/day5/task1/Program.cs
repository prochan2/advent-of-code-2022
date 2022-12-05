//const string inputFile = @"..\..\..\input\sinput.txt";
//List<Stack<char>> stacks = new(3);

const string inputFile = @"..\..\..\input\input.txt";
List<Stack<char>> stacks = new(9);

for (int i = 0; i < stacks.Capacity; i++)
{
    stacks.Add(new());
}

CargoShip? ship = null;

foreach (var line in File.ReadLines(inputFile))
{
    if (ship == null)
    {
        if (line.Length >= 1 && char.IsDigit(line[1]))
        {
            continue;
        }
        else if (line == "")
        {
            ship = new(stacks.Count);

            for (int i = 0; i < stacks.Count; i++)
            {
                stacks[i].ToList().ForEach(c => ship.Push(i + 1, c));
            }

            ship.Print();
            Console.WriteLine();
            continue;
        }
        else
        {
            int stackIndex = 0;
            int lineIndex = 1;

            while (lineIndex < line.Length)
            {
                var item = line[lineIndex];

                if (item != ' ')
                {
                    stacks[stackIndex].Push(item);
                }

                stackIndex++;
                lineIndex += 4;
            }
        }
    }
    else
    {
        var tokens = line.Split(' ');
        
        int cratesCount = int.Parse(tokens[1]);
        int from = int.Parse(tokens[3]);
        int to = int.Parse(tokens[5]);

        for (int i = 0; i < cratesCount; i++)
        {
            ship.Move(from, to);
        }

        ship.Print();
        Console.WriteLine();
    }
}

Console.WriteLine(string.Join("", ship!.Peek()));

class CargoStack
{
    private readonly Stack<char> content = new();

    public char Top => content.TryPeek(out char top) ? top : '\0';

    public int Count => content.Count;

    public char Pop() => content.Pop();

    public void Push(char item) => content.Push(item);

    internal char[] ToArray() => content.ToArray();
}

class CargoShip
{
    private readonly Dictionary<int, CargoStack> stacks;

    public CargoShip(int size)
    {
        stacks = new(size);

        for (int i = 1; i <= size; i++)
        {
            stacks.Add(i, new());
        }
    }

    public CargoStack this[int position] => stacks[position];

    public void Push(int position, char item) => stacks[position].Push(item);

    public char Pop(int position) => stacks[position].Pop();

    public void Move(int from, int to) => this.Push(to, this.Pop(from));

    public IEnumerable<char> Peek() => stacks.OrderBy(s => s.Key).Select(s => s.Value.Top);

    public void Print()
    {
        var stackArrays = stacks.Values.Select(s => s.ToArray().Reverse().ToArray()).ToArray();

        int maxCount = stackArrays.Max(a => a.Length);

        for (int row = maxCount - 1; row >= 0; row--)
        {
            for (int column = 0; column < stackArrays.Length; column++)
            {
                if (stackArrays[column].Length > row)
                {
                    Console.Write($"[{stackArrays[column][row]}] ");
                }
                else
                {
                    Console.Write("    ");
                }
            }

            Console.WriteLine();
        }

        for (int column = 1; column <= stackArrays.Length; column++)
        {
            Console.Write($" {column}  ");
        }

        Console.WriteLine();
    }
}