using AoCToolbox;

namespace Day11
{
    public class Solver : SolverBase
    {
        const char Galaxy = '#';

        public override string GetDayString()
        {
            return "* December 11th: *";
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
            return expandSpace(inputData, 2L);
        }

        private object RunPart2(string[] inputData)
        {
            return expandSpace(inputData, 1000000L);
        }

        private long expandSpace(string[] unexpandedSpace, long expandFactor)
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

        
    }
}
