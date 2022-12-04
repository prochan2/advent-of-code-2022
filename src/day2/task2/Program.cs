using System.Collections.Immutable;
using System.Dynamic;

//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

List<Round> rounds = new();

foreach (var line in File.ReadLines(inputFile))
{
    var opponent = ShapeHelpers.CharToShape(line[0]);
    var me = ShapeHelpers.ShapeForOutcome(opponent, line[2]);
    rounds.Add(new Round(opponent, me));
}

Console.WriteLine(rounds.Sum(r => r.Total));

static class ShapeHelpers
{
    public static Shape CharToShape(char c) => c switch
    {
        'A' => Shape.Rock,
        'B' => Shape.Paper,
        'C' => Shape.Scissors,
        _ => throw new ArgumentOutOfRangeException(nameof(c))
    };

    public static Shape ShapeForOutcome(Shape opponent, char outcome) => outcome switch
    {
        // Loose
        'X' => opponent switch
        {
            Shape.Rock => Shape.Scissors,
            Shape.Paper => Shape.Rock,
            Shape.Scissors => Shape.Paper,
            _ => throw new ArgumentOutOfRangeException(nameof(opponent))
        },
        // Draw
        'Y' => opponent,
        // Win
        'Z' => opponent switch
        {
            Shape.Rock => Shape.Paper,
            Shape.Paper => Shape.Scissors,
            Shape.Scissors => Shape.Rock,
            _ => throw new ArgumentOutOfRangeException(nameof(opponent))
        },
        _ => throw new ArgumentOutOfRangeException(nameof(outcome))
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