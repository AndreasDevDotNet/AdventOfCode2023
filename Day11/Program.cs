
using AoCToolbox;

var inputData = File.ReadAllLines("input.txt");

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 11th: *");
Console.WriteLine("   Problem: Cosmic Expansion");
Console.WriteLine("   Part #1");
Console.WriteLine($"      Sum of lengths between galaxies : {RunPart1(inputData)}");
Console.WriteLine("   Part #2");
Console.WriteLine($"      Sum of lengths between galaxies one million times larger : {RunPart2(inputData)}");
Console.WriteLine("-------------------------");

const char Galaxy = '#';

long RunPart1(string[] inputData)
{
    return expandSpace(inputData, 2L);
}

long RunPart2(string[] inputData)
{
    return expandSpace(inputData, 1000000L);
}

long expandSpace(string[] unexpandedSpace, long expandFactor)
{
    var map = new Dictionary<int, Vector2D>();
    var rows = Enumerable.Range(start: 0, count: unexpandedSpace.Length).ToHashSet();
    var cols = Enumerable.Range(start: 0, count: unexpandedSpace[0].Length).ToHashSet();

    for (var y = 0; y < unexpandedSpace.Length; y++)
        for (var x = 0; x < unexpandedSpace[0].Length; x++)
        {
            if (unexpandedSpace[y][x] == Galaxy)
            {
                map[map.Count] = new Vector2D(x, y);
                cols.Remove(x);
                rows.Remove(y);
            }
        }

    var sum = 0L;
    var done = new HashSet<(int, int)>();

    foreach (var (id1, pos1) in map)
    foreach (var (id2, pos2) in map)
    {
        if (done.Add((id1, id2)) && done.Add((id2, id1)))
        {
            var xMin = Math.Min(pos1.X, pos2.X);
            var xMax = Math.Max(pos1.X, pos2.X);
            var yMin = Math.Min(pos1.Y, pos2.Y);
            var yMax = Math.Max(pos1.Y, pos2.Y);

            var dx = (expandFactor - 1) * cols.Count(x => x > xMin && x < xMax);
            var dy = (expandFactor - 1) * rows.Count(y => y > yMin && y < yMax);

            sum += Vector2D.Distance(a: pos1, b: pos2, Metric.Taxicab) + dx + dy;
        }
    }

    return sum;
}