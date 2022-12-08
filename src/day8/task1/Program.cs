//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

var lines = File.ReadAllLines(inputFile);

var orchard = new Tree[lines[0].Length, lines.Length];

for (int row = 0; row < orchard.GetLength(0); row++)
{
    for (int column = 0; column < orchard.GetLength(1); column++)
    {
        orchard[column, row] = new((int)(lines[row][column] - '0'));
    }
}

for (int row = 0; row < orchard.GetLength(0); row++)
{
    int tallest = -1;

    for (int column = 0; column < orchard.GetLength(1); column++)
    {
        if (orchard[column, row].Height > tallest)
        {
            tallest = orchard[column, row].Height;
            orchard[column, row].IsVisibleFromLeft = true;
        }
    }

    tallest = -1;

    for (int column = orchard.GetLength(1) - 1; column >= 0; column--)
    {
        if (orchard[column, row].Height > tallest)
        {
            tallest = orchard[column, row].Height;
            orchard[column, row].IsVisibleFromRight = true;
        }
    }
}

for (int column = 0; column < orchard.GetLength(1); column++)
{
    int tallest = -1;

    for (int row = 0; row < orchard.GetLength(0); row++)
    {
        if (orchard[column, row].Height > tallest)
        {
            tallest = orchard[column, row].Height;
            orchard[column, row].IsVisibleFromTop = true;
        }
    }

    tallest = -1;

    for (int row = orchard.GetLength(0) - 1; row >= 0; row--)
    {
        if (orchard[column, row].Height > tallest)
        {
            tallest = orchard[column, row].Height;
            orchard[column, row].IsVisibleFromBottom = true;
        }
    }
}

var totalVisibleTreesCount = 0L;

var defaultColor = Console.ForegroundColor;

for (int row = 0; row < orchard.GetLength(0); row++)
{
    string outputLine1 = "";
    string outputLine2 = "";
    string outputLine3 = "";

    for (int column = 0; column < orchard.GetLength(1); column++)
    {
        var tree = orchard[column, row];
        var isVisible = tree.IsVisibleFromTop || tree.IsVisibleFromRight || tree.IsVisibleFromBottom || tree.IsVisibleFromLeft;

        if (isVisible)
        {
            totalVisibleTreesCount++;
        }

        outputLine1 += " " + (tree.IsVisibleFromTop ? "T" : "t") + " ";
        outputLine2 += (tree.IsVisibleFromLeft ? "L" : "l") + (isVisible ? "X" : "x") + tree.Height + (tree.IsVisibleFromRight ? "R" : "r");
        outputLine3 += " " + (tree.IsVisibleFromBottom ? "B" : "b") + " ";
    }

    void WriteLine(string line)
    {
        foreach (var c in line)
        {
            if (char.IsLetter(c))
            {
                if (char.IsUpper(c))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
            }

            if (c != 'x' && c != 'X')
            {
                Console.Write(c switch
                {
                    'T' or 't' => '^',
                    'R' or 'r' => '>',
                    'L' or 'l' => '<',
                    'B' or 'b' => 'ˇ',
                    _ => c
                });
            }
        }

        Console.WriteLine();
    }

    WriteLine(outputLine1);
    WriteLine(outputLine2);
    WriteLine(outputLine3);
}

Console.ForegroundColor = defaultColor;

Console.WriteLine(totalVisibleTreesCount);

class Tree {
    public int Height { get; }

    public bool IsVisibleFromTop { get; set; }

    public bool IsVisibleFromRight { get; set; }

    public bool IsVisibleFromBottom { get; set; }

    public bool IsVisibleFromLeft { get; set; }

    public Tree(int height)
    {
        Height = height;
    }
}