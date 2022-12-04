//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

List<Rucksack> rucksacks = new();

foreach (var line in File.ReadLines(inputFile))
{
    int totalItems = line.Length;
    int itemsPerCompartment = totalItems / 2;

    var rucksack = new Rucksack();

    var compartment = new Compartment();

    for (int i = 0; i < itemsPerCompartment; i++)
    {
        compartment.Add(line[i]);
    }

    rucksack.Add(compartment);

    compartment = new Compartment();

    for (int i = itemsPerCompartment; i < totalItems; i++)
    {
        compartment.Add(line[i]);
    }

    rucksack.Add(compartment);

    rucksacks.Add(rucksack);
}

long total = rucksacks.Sum(SumOfDuplicatesInRucksack);

Console.WriteLine(total);

long SumOfDuplicatesInRucksack(Rucksack rucksack)
{
    long sum = 0;

    for (char item = 'a'; item <= 'z'; item++)
    {
        if (rucksack.AllContain(item))
        {
            sum += new Item(item).Priority;
        }
    }

    for (char item = 'A'; item <= 'Z'; item++)
    {
        if (rucksack.AllContain(item))
        {
            sum += new Item(item).Priority;
        }
    }

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

class Compartment
{
    private readonly HashSet<char> items = new();

    public void Add(char item) => items.Add(item);

    public bool Contains(char item) => items.Contains(item);
}

class Rucksack
{
    private readonly List<Compartment> compartments = new();

    public void Add(Compartment compartment) => compartments.Add(compartment);

    public bool AllContain(char item) => compartments.All(c => c.Contains(item));
}