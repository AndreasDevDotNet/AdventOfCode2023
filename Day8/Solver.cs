using AoCToolbox;
using System.Text.RegularExpressions;

namespace Day8
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 8th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Haunted Wasteland";
        }

        public override void ShowVisualization()
        {
            throw new NotImplementedException();
        }

        public override object Run(int part)
        {
            var inputData = GetInputLines();
            var rightLeftInput = File.ReadAllText("rightleftinput.txt");

            var value = part switch
            {
                1 => RunPart1(inputData, rightLeftInput),
                2 => RunPart2(inputData, rightLeftInput)
            };

            return part switch
            {
                1 => $" Part #1\n  Number of steps to reach ZZZ: {value}",
                2 => $" Part #2\n  Number of steps until you are only on Z nodes: {value}",
            };
        }

        private object RunPart1(string[] inputData, string rightLeftInstructions)
        {
            var nodes = inputData.Select(ParseNode).ToDictionary(n => n.Id);

            return NavigateSingle(rightLeftInstructions, nodes);
        }

        private object RunPart2(string[] inputData, string rightLeftInstructions)
        {
            var nodes = inputData.Select(ParseNode).ToDictionary(n => n.Id);

            return NavigateMany(rightLeftInstructions, nodes);
        }

        private long NavigateSingle(string dirs, IDictionary<string, Node> nodes)
        {
            return Navigate(dirs, nodes, start: "AAA", stop: id => id == "ZZZ");
        }

        private long NavigateMany(string dirs, IDictionary<string, Node> nodes)
        {
            var stop = (string id) => id.EndsWith('Z');
            var cycles = nodes.Keys
                .Where(id => id.EndsWith('A'))
                .Select(id => Navigate(dirs, nodes, start: id, stop))
                .ToList();

            return Numerics.Lcm(cycles);
        }

        private long Navigate(string dirs, IDictionary<string, Node> nodes, string start, Func<string, bool> stop)
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

        private Node ParseNode(string line)
        {
            var match = Regex.Match(line, @"(?<I>[1-9A-Z]+) = \((?<L>[1-9A-Z]+), (?<R>[1-9A-Z]+)\)");
            return new Node(
                Id: match.Groups["I"].Value,
                Left: match.Groups["L"].Value,
                Right: match.Groups["R"].Value);
        }


        readonly record struct Node(string Id, string Left, string Right);
    }
}
