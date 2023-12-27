using AoCToolbox;

namespace Day9
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 9th *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Mirage Maintenance";
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
                1 => $" Part #1\n  Sum of these extrapolated values(Forward): {value}",
                2 => $" Part #2\n  Sum of these extrapolated values(Backward): {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            int total = 0;

            foreach (var line in inputData)
            {
                var nums = line.ExtractInts().ToArray();
                total += Extrapolate(nums);
            }

            return total;
        }

        private object RunPart2(string[] inputData)
        {
            int total = 0;

            foreach (var line in inputData)
            {
                var nums = line.ExtractInts().ToArray();
                total += Extrapolate(nums, true);
            }

            return total;
        }

        int Extrapolate(int[] array, bool runBackwards = false)
        {
            if (array.All(x => x == 0))
            {
                return 0;
            }

            var deltas = array.Zip(array.Skip(1), (x, y) => y - x).ToArray();
            var diff = Extrapolate(deltas, runBackwards);
            if (!runBackwards)
            {
                return array[^1] + diff; 
            }
            else
            {
                return array[0] - diff;
            }
                
        }
    }
}
