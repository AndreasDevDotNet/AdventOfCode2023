using AoCToolbox;
using System.Text.RegularExpressions;

var inputData = File.ReadAllLines("input.txt");
var rightLeftInput = File.ReadAllText("rightleftinput.txt");

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 8th: *");
Console.WriteLine("   Part #1");
Console.WriteLine($"      Number of steps to reach ZZZ : {RunPart1(inputData, rightLeftInput)}");
Console.WriteLine("   Part #1");
Console.WriteLine($"      Number of steps until you are only on Z nodes : {RunPart2(inputData, rightLeftInput)}");
Console.WriteLine("-------------------------");

long RunPart1(string[] inputdata, string rightLeftInstructions)
{
    var nodes = inputData.Select(ParseNode).ToDictionary(n => n.Id);

    return NavigateSingle(rightLeftInstructions, nodes);
}

long RunPart2(string[] inputdata, string rightLeftInstructions)
{
    var nodes = inputData.Select(ParseNode).ToDictionary(n => n.Id);

    return NavigateMany(rightLeftInstructions, nodes);
}


long NavigateSingle(string dirs, IDictionary<string, Node> nodes)
{
    return Navigate(dirs, nodes, start: "AAA", stop: id => id == "ZZZ");
}

long NavigateMany(string dirs, IDictionary<string, Node> nodes)
{
    var stop = (string id) => id.EndsWith('Z');
    var cycles = nodes.Keys
        .Where(id => id.EndsWith('A'))
        .Select(id => Navigate(dirs, nodes, start: id, stop))
        .ToList();

    return Numerics.Lcm(cycles);
}

long Navigate(string dirs, IDictionary<string, Node> nodes, string start, Func<string, bool> stop)
{
    var i = 0L;
    var pos = nodes[start];

    while (!stop.Invoke(pos.Id))
    {
        pos = dirs[(int)(i++ % dirs.Length)] switch
        {
            'L' => nodes[pos.Left],
            'R' => nodes[pos.Right],
            _ => throw new Exception("No solution")
        };
    }

    return i;
}

Node ParseNode(string line)
{
    var match = Regex.Match(line, @"(?<I>[1-9A-Z]+) = \((?<L>[1-9A-Z]+), (?<R>[1-9A-Z]+)\)");
    return new Node(
        Id: match.Groups["I"].Value,
        Left: match.Groups["L"].Value,
        Right: match.Groups["R"].Value);
}


readonly record struct Node(string Id, string Left, string Right);