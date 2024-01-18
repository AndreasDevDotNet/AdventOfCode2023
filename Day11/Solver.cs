using AoCToolbox;

namespace Day11
{
    public class Solver : SolverBase
    {
        const char Galaxy = '#';

        public override string GetDayString()
        {
            return "* December 11th *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Cosmic Expansion";
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
                1 => $" Part #1\n  Sum of lengths between galaxies: {value}",
                2 => $" Part #2\n  Sum of lengths between galaxies one million times larger: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            return calculateDistanceBetweenGalaxies(inputData, 2L);
        }

        private object RunPart2(string[] inputData)
        {
            return calculateDistanceBetweenGalaxies(inputData, 1000000L);
        }

        private long calculateDistanceBetweenGalaxies(string[] unexpandedSpace, long expandFactor)
        {
            List<int> emptyRows = Enumerable.Range(0, unexpandedSpace.Length)
                .Where(r => unexpandedSpace[r].All(ch => ch == '.'))
                .ToList();

            List<int> emptyCols = Enumerable.Range(0, unexpandedSpace[0].Length)
                .Where(c => unexpandedSpace.All(row => row[c] == '.'))
                .ToList();

            List<(int, int)> galaxies = new List<(int, int)>();

            for (int r = 0; r < unexpandedSpace.Length; r++)
            {
                for (int c = 0; c < unexpandedSpace[r].Length; c++)
                {
                    if (unexpandedSpace[r][c] == '#')
                    {
                        galaxies.Add((r, c));
                    }
                }
            }

            long total = 0;

            for (int i = 0; i < galaxies.Count; i++)
            {
                int r1 = galaxies[i].Item1;
                int c1 = galaxies[i].Item2;

                for (int j = 0; j < i; j++)
                {
                    int r2 = galaxies[j].Item1;
                    int c2 = galaxies[j].Item2;

                    for (int r = Math.Min(r1, r2); r < Math.Max(r1, r2); r++)
                    {
                        total += emptyRows.Contains(r) ? expandFactor : 1;
                    }

                    for (int c = Math.Min(c1, c2); c < Math.Max(c1, c2); c++)
                    {
                        total += emptyCols.Contains(c) ? expandFactor : 1;
                    }
                }
            }

            return total;
        }
    }
}
