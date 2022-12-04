using System.Collections.Immutable;
using System.Dynamic;

//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

List<Round> rounds = new();

foreach (var line in File.ReadLines(inputFile))
{
    var opponent = ShapeHelpers.CharToShape(line[0]);
    var me = ShapeHelpers.CharToShape(line[2]);
    rounds.Add(new Round(opponent, me));
}

Console.WriteLine(rounds.Sum(r => r.Total));

static class ShapeHelpers
{
    public static Shape CharToShape(char c) => c switch
    {
        'A' or 'X' => Shape.Rock,
        'B' or 'Y' => Shape.Paper,
        'C' or 'Z' => Shape.Scissors,
        _ => throw new ArgumentOutOfRangeException(nameof(c))
    };

    public static long ValueOfShape(Shape shape) => shape switch
    {
        Shape.Rock => 1,
        Shape.Paper => 2,
        Shape.Scissors => 3,
        _ => throw new ArgumentOutOfRangeException(nameof(shape))
    };

    public static long ValueOfOutcome(Shape opponent, Shape me)
    {
        if (opponent == me)
        {
            return 3;
        }
        else
        {
            return (opponent, me) switch
            {
                (Shape.Rock, Shape.Paper) or (Shape.Paper, Shape.Scissors) or (Shape.Scissors, Shape.Rock) => 6,
                (Shape.Rock, Shape.Scissors) or (Shape.Paper, Shape.Rock) or (Shape.Scissors, Shape.Paper) => 0,
                _ => throw new InvalidOperationException()
            };
        }
    }
}

enum Shape
{
    Rock,
    Paper,
    Scissors
}

struct Round
{
    public Round()
    {
    }

    public Round(Shape opponent, Shape me) : this()
    {
        Opponent = opponent;
        Me = me;
        Outcome = ShapeHelpers.ValueOfOutcome(opponent, me);
        MyValue = ShapeHelpers.ValueOfShape(me);
        Total = Outcome + MyValue;
    }

    public Shape Opponent { get; }

    public Shape Me { get; }

    public long Outcome { get; }

    public long MyValue { get; }

    public long Total { get; }
}