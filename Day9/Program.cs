using AoCToolbox;

var inputData = File.ReadAllLines("input.txt");

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 9th: *");
Console.WriteLine("   Problem: Mirage Maintenance");
Console.WriteLine("   Part #1");
Console.WriteLine($"      Sum of these extrapolated values(Forward) : {RunPart1(inputData)}");
Console.WriteLine("   Part #2");
Console.WriteLine($"      Sum of these extrapolated values(Backward) : {RunPart2(inputData)}");
Console.WriteLine("-------------------------");

long RunPart1(string[] inputData)
{
    var histories = inputData.Select(FormHistory);

    return histories.Sum(ExtrapolateForwards);
}

long RunPart2(string[] inputData)
{
    var histories = inputData.Select(FormHistory);
    return histories.Sum(ExtrapolateBackwards);
}

List<int[]> FormHistory(string report)
{
    var initial = report.ParseInts();
    var sequences = new List<int[]> { initial.ToArray() };

    while (sequences[^1].Any(v => v != 0))
    {
        sequences.Add(item: sequences[^1]
            .Skip(1)
            .Select((val, i) => val - sequences[^1][i])
            .ToArray());
    }

    return sequences;
}

int ExtrapolateForwards(IList<int[]> sequences)
{
    return sequences
        .Reverse()
        .Skip(1)
        .Aggregate(seed: 0, func: (n, seq) => n + seq[^1]);
}

int ExtrapolateBackwards(IList<int[]> sequences)
{
    return sequences
        .Reverse()
        .Skip(1)
        .Aggregate(seed: 0, func: (n, seq) => seq[0] - n);
}
