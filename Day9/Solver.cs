using AoCToolbox;

namespace Day9
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 9th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Mirage Maintenance";
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
                1 => $" Part #1\n  Sum of these extrapolated values(Forward): {value}",
                2 => $" Part #2\n  Sum of these extrapolated values(Backward): {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            var histories = inputData.Select(formHistory);

            return histories.Sum(extrapolateForwards);
        }

        private object RunPart2(string[] inputData)
        {
            var histories = inputData.Select(formHistory);
            return histories.Sum(extrapolateBackwards);
        }

        private List<int[]> formHistory(string report)
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

        private int extrapolateForwards(IList<int[]> sequences)
        {
            return sequences
                .Reverse()
                .Skip(1)
                .Aggregate(seed: 0, func: (n, seq) => n + seq[^1]);
        }

        private int extrapolateBackwards(IList<int[]> sequences)
        {
            return sequences
                .Reverse()
                .Skip(1)
                .Aggregate(seed: 0, func: (n, seq) => seq[0] - n);
        }
    }
}
