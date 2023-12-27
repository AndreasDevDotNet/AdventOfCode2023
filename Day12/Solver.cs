using AoCToolbox;

namespace Day12
{
    public class Solver : SolverBase
    {
        private Dictionary<string, long> cache = new Dictionary<string, long>();

        public override string GetDayString()
        {
            return "* December 12th *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Hot Springs";
        }

        public override void ShowVisualization()
        {
            throw new NotImplementedException();
        }

        public override object Run(int part)
        {
            var inputData = GetInputLines();

            var value = part switch
            {
                1 => RunPart1(inputData),
                2 => RunPart2(inputData)
            };

            return part switch
            {
                1 => $" Part #1\n  Number of possible arrangements: {value}",
                2 => $" Part #2\n  Number of possible arrangements unfolded: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            long arrangements = 0;

            foreach (var line in inputData.Select(l => l.Split(' ')))
            {
                var springs = line[0];
                var groups = line[1].Split(',').Select(int.Parse).ToList();

                arrangements += calculate(springs, groups);
            }

            return arrangements;
        }

        private object RunPart2(string[] inputData)
        {
            long arrangements = 0;

            foreach (var line in inputData.Select(l => l.Split(' ')))
            {
                var springs = line[0];
                var groups = line[1].Split(',').Select(int.Parse).ToList();
                springs = string.Join('?', Enumerable.Repeat(springs, 5));
                groups = Enumerable.Repeat(groups, 5).SelectMany(g => g).ToList();

                arrangements += calculate(springs, groups);
            }

            return arrangements;
        }

        private long calculate(string springs, List<int> groups)
        {
            var key = $"{springs},{string.Join(',', groups)}";  // Cache key: spring pattern + group lengths

            if (cache.TryGetValue(key, out var value))
            {
                return value;
            }

            value = getCount(springs, groups);
            cache[key] = value;

            return value;
        }

        private long getCount(string springs, List<int> groups)
        {
            while (true)
            {
                if (groups.Count == 0)
                {
                    return springs.Contains('#') ? 0 : 1; // No more groups to match: if there are no springs left, we have a match
                }

                if (string.IsNullOrEmpty(springs))
                {
                    return 0; // No more springs to match, although we still have groups to match
                }

                if (springs.StartsWith('.'))
                {
                    springs = springs.Trim('.'); // Remove all dots from the beginning
                    continue;
                }

                if (springs.StartsWith('?'))
                {
                    return calculate("." + springs[1..], groups) + calculate("#" + springs[1..], groups); // Try both options recursively
                }

                if (springs.StartsWith('#')) // Start of a group
                {
                    if (groups.Count == 0)
                    {
                        return 0; // No more groups to match, although we still have a spring in the input
                    }

                    if (springs.Length < groups[0])
                    {
                        return 0; // Not enough characters to match the group
                    }

                    if (springs[..groups[0]].Contains('.'))
                    {
                        return 0; // Group cannot contain dots for the given length
                    }

                    if (groups.Count > 1)
                    {
                        if (springs.Length < groups[0] + 1 || springs[groups[0]] == '#')
                        {
                            return 0; // Group cannot be followed by a spring, and there must be enough characters left
                        }

                        springs = springs[(groups[0] + 1)..]; // Skip the character after the group - it's either a dot or a question mark
                        groups = groups[1..];
                        continue;
                    }

                    springs = springs[groups[0]..]; // Last group, no need to check the character after the group
                    groups = groups[1..];
                    continue;
                }

                throw new Exception("Invalid input");
            }
        }
    }
}
