using AoCToolbox;

namespace Day5
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 5th *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: If You Give A Seed A Fertilizer";
        }

        public override void ShowVisualization()
        {
            throw new NotImplementedException();
        }

        public override object Run(int part)
        {
         
            var value = part switch
            {
                1 => RunPart1(),
                2 => RunPart2(),
            };

            return part switch
            {
                1 => $" Part #1\n  Lowest location number that corresponds to any of the initial seed numbers: {value}",
                2 => $" Part #2\n  Lowest location number that corresponds to any of the initial seed ranges: {value}",
            };
        }

        private object RunPart1()
        {
            var inputLines = GetInputText().SplitByDoubleNewline();

            var seeds = inputLines[0].Split(':')[1].ExtractLongs().ToList();

            foreach (var block in inputLines.Skip(1))
            {
                var ranges = block.Split('\n').Skip(1).Select(line => line.ExtractLongs().ToList()).ToList();
                var newSeeds = seeds.Select(seed =>
                {
                    foreach (var range in ranges)
                    {
                        long destinationRangeStart = range[0];
                        long sourceRangeStart = range[1];
                        long rangeLength = range[2];

                        if (sourceRangeStart <= seed && seed < sourceRangeStart + rangeLength)
                        {
                            return seed - sourceRangeStart + destinationRangeStart;
                        }
                    }
                    return seed;
                }).ToList();

                seeds = newSeeds;
            }

            return seeds.Min();
        }

        private object RunPart2()
        {
            var inputLines = GetInputText().SplitByDoubleNewline();

            var inputs = inputLines[0].Split(':')[1].ExtractLongs().ToList();
            var seeds = new List<(long, long)>();

            for (int i = 0; i < inputs.Count; i += 2)
            {
                seeds.Add((inputs[i], inputs[i] + inputs[i + 1]));
            }

            foreach (var block in inputLines.Skip(1))
            {
                var ranges = block.Split('\n').Skip(1).Select(line => line.ExtractLongs().ToList()).ToList();
                var newSeeds = new List<(long, long)>();

                while (seeds.Count > 0)
                {
                    var (seedStart, seedEnd) = seeds.Pop();

                    bool added = false;
                    foreach (var range in ranges)
                    {
                        long destinationRangeStart = range[0];
                        long sourceRangeStart = range[1];
                        long rangeLength = range[2];

                        var overlapStart = Math.Max(seedStart, sourceRangeStart);
                        var overlapEnd = Math.Min(seedEnd, sourceRangeStart + rangeLength);

                        if (overlapStart < overlapEnd)
                        {
                            newSeeds.Add((overlapStart - sourceRangeStart + destinationRangeStart, overlapEnd - sourceRangeStart + destinationRangeStart));
                            if (overlapStart > seedStart)
                            {
                                seeds.Add((seedStart, overlapStart));
                            }
                            if (seedEnd > overlapEnd)
                            {
                                seeds.Add((overlapEnd, seedEnd));
                            }
                            added = true;
                            break;
                        }
                    }

                    if(!added)
                    {
                        newSeeds.Add((seedStart, seedEnd));
                    }
                }
                seeds = newSeeds;
            }

            return seeds.Min().Item1;
        }

    }
}
