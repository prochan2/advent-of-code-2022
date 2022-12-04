﻿using System.Collections.Immutable;

//const string inputFile = @"..\..\..\input\sinput.txt";
const string inputFile = @"..\..\..\input\input.txt";

List<Elf> elfs = new();

Elf currentElf = new Elf();

foreach (var line in File.ReadLines(inputFile))
{
    if (line == "")
    {
        elfs.Add(currentElf);
        currentElf = new Elf();
    }
    else
    {
        currentElf = currentElf.WithMeal(new Meal(long.Parse(line)));
    }
}

Elf elfWithMostCalories = elfs.MaxBy(e => e.TotalCalories);

Console.WriteLine(elfWithMostCalories.TotalCalories);

readonly struct Elf
{
    public Elf()
    {
    }

    public IImmutableList<Meal> Meals { get; init; } = ImmutableList<Meal>.Empty;

    public long TotalCalories => this.Meals.Sum(m => m.Calories);

    public Elf WithMeal(Meal meal) => new() { Meals = this.Meals.Add(meal) };
}

record Meal(long Calories);