using AoCToolbox;
using System.Text.RegularExpressions;

namespace Day19
{
    using Ratings = Dictionary<string, Range<long>>;
    using Workflows = DefaultDict<string, List<Rule>>;
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 19th *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Aplenty";
        }

        public override void ShowVisualization()
        {
        }

        public override object Run(int part)
        {
            var inputData = GetInputLines()
                            .ChunkBy(line => !string.IsNullOrWhiteSpace(line))
                            .ToArray();

            var workflows = ParseWorkflows(lines: inputData[0]);
            var parts = ParseParts(lines: inputData[1]);

            var value = part switch
            {
                1 => RunPart1(parts, workflows),
                2 => RunPart2(workflows)
            };

            return part switch
            {
                1 => $" Part #1\n  Sum of all part ratings: {value}",
                2 => $" Part #2\n  Number of distinct combinations of ratings: {value}",
            };
        }

        private object RunPart1(IEnumerable<Part> parts, Workflows workflows)
        {
            return parts
                .Where(part => CheckAccepted(id: "in", workflows, part))
                .Sum(part => part.Ratings.Values.Sum());
        }

        private object RunPart2(Workflows workflows)
        {
            var range = new Range<long>(min: 1L, max: 4000L);
            var ratings = new Ratings
            {
                { "x", range },
                { "m", range },
                { "a", range },
                { "s", range }
            };

            return CountAccepted(id: "in", workflows, ratings);
        }

        private bool CheckAccepted(string id, Workflows workflows, Part part)
        {
            switch (id)
            {
                case "A":
                    return true;
                case "R":
                    return false;
            }

            foreach (var (lhs, op, rhs, next) in workflows[id])
            {
                switch (op)
                {
                    case ">" when part.Ratings[lhs] > rhs:
                    case "<" when part.Ratings[lhs] < rhs:
                        return CheckAccepted(id: next, workflows, part);
                    default:
                        continue;
                }
            }

            throw new Exception();
        }

        private long CountCombinations(Workflows workflows)
        {
            var range = new Range<long>(min: 1L, max: 4000L);
            var ratings = new Ratings
        {
            { "x", range },
            { "m", range },
            { "a", range },
            { "s", range }
        };

            return CountAccepted(id: "in", workflows, ratings);
        }

        private long CountAccepted(string id, Workflows workflows, Ratings ratings)
        {
            switch (id)
            {
                case "A":
                    return ratings.Values.Aggregate(seed: 1L, func: (product, range) => product * range.Length);
                case "R":
                    return 0;
            }

            foreach (var (lhs, op, rhs, next) in workflows[id])
            {
                var range = ratings[lhs];
                var pass = default(Range<long>);
                var fail = default(Range<long>);

                switch (op)
                {
                    case "<" when range.Min >= rhs:
                    case ">" when range.Max <= rhs:
                        continue;
                    case "<" when range.Max < rhs:
                    case ">" when range.Min > rhs:
                        return CountAccepted(id: next, workflows, ratings);
                    case ">":
                        {
                            fail = new Range<long>(min: range.Min, max: rhs);
                            pass = new Range<long>(min: rhs + 1, max: range.Max);
                            break;
                        }
                    case "<":
                        {
                            pass = new Range<long>(min: range.Min, max: rhs - 1);
                            fail = new Range<long>(min: rhs, max: range.Max);
                            break;
                        }
                }

                return CountAccepted(id: next, workflows, ratings: BranchRatings(ratings, category: lhs, range: pass)) +
                       CountAccepted(id: id, workflows, ratings: BranchRatings(ratings, category: lhs, range: fail));
            }

            throw new Exception();
        }

        private Ratings BranchRatings(Ratings nominal, string category, Range<long> range)
        {
            return new Ratings(collection: nominal)
            {
                [category] = range
            };
        }

        private Workflows ParseWorkflows(IEnumerable<string> lines)
        {
            var workflows = new Workflows(defaultSelector: _ => []);
            foreach (var line in lines)
            {
                var elements = line.Split(separator: ['{', '}']);
                var id = elements[0];
                var rules = elements[1].Split(separator: ',');

                for (var i = 0; i < rules.Length - 1; i++)
                {
                    var m = Regex.Match(input: rules[i], pattern: @"(?<T>[xmas])(?<O>[\<\>])(?<V>\d+)\:(?<G>[a-zA-Z]+)$");
                    workflows[id].Add(item: new Rule(
                        LeftSide: m.Groups["T"].Value,
                        Op: m.Groups["O"].Value,
                        RightSide: m.Groups["V"].ParseLong(),
                        Next: m.Groups["G"].Value));
                }

                workflows[id].Add(item: new Rule(LeftSide: "x", Op: ">", RightSide: 0, Next: rules[^1]));
            }
            return workflows;
        }

        private IEnumerable<Part> ParseParts(IEnumerable<string> lines)
        {
            var parts = new List<Part>();
            foreach (var line in lines)
            {
                var longs = line.ParseLongs();
                var ratings = new Dictionary<string, long>
                {
                    {"x", longs[0]},
                    {"m", longs[1]},
                    {"a", longs[2]},
                    {"s", longs[3]}
                };
                parts.Add(item: new Part(ratings));
            }
            return parts;
        }


    }

    public record Part(Dictionary<string, long> Ratings);
    public record Rule(string LeftSide, string Op, long RightSide, string Next);
}
