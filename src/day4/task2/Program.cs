//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

List<(Section Section1, Section Section2)> sectionPairs = new();

foreach (var line in File.ReadLines(inputFile))
{
    var sectionIntervals = line.Split(',');
    sectionPairs.Add((Section.Parse(sectionIntervals[0]), Section.Parse(sectionIntervals[1])));
}

sectionPairs.ForEach(p => Console.WriteLine($"{p.Section1.Start}-{p.Section1.End},{p.Section2.Start}-{p.Section2.End}:{p.Section1.Overlaps(p.Section2)}"));

var count = sectionPairs.Count(p => p.Section1.Overlaps(p.Section2));

Console.WriteLine(count);

record Section
{
    public Section()
    {
    }

    public Section(long start, long end)
    {
        Start = start;
        End = end;
    }

    public long Start { get; }

    public long End { get; }

    public static Section Parse(string sectionInterval)
    {
        var minMax = sectionInterval.Split('-');
        return new(long.Parse(minMax[0]), long.Parse(minMax[1]));
    }

    public bool FullyContains(Section other) => this.Start <= other.Start && this.End >= other.End;

    public bool Overlaps(Section other) => (this.Start <= other.Start && this.End >= other.Start) || (this.Start <= other.End && this.End >= other.End) || this.FullyContains(other) || other.FullyContains(this);
}