//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

List<(Section Section1, Section Section2)> sectionPairs = new();

foreach (var line in File.ReadLines(inputFile))
{
    var sectionIntervals = line.Split(',');
    sectionPairs.Add((Section.Parse(sectionIntervals[0]), Section.Parse(sectionIntervals[1])));
}

var count = sectionPairs.Count(p => p.Section1.FullyContains(p.Section2) || p.Section2.FullyContains(p.Section1));

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
}