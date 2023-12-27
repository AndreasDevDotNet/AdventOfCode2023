using AoCToolbox;

namespace Day21
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 21th *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Step Counter";
        }

        public override void ShowVisualization()
        {
        }

        public override object Run(int part)
        {
            var inputData = GetInputLines().ToList();

            var value = part switch
            {
                1 => RunPart1(inputData),
                2 => RunPart2(inputData)
            };

            return part switch
            {
                1 => $" Part #1\n  Number of garden plots the Elf could reach in exactly 64 steps: {value}",
                2 => $" Part #2\n  Number of garden plots the Elf could reach in exactly 605492675373144 steps (Infinite map): {value}",
            };
        }

        private object RunPart1(List<string> inputData)
        {
            var gridSize = inputData.Count == inputData[0].Length ? inputData.Count : throw new ArgumentOutOfRangeException();

            var gardenerStartPos = Enumerable.Range(0, gridSize)
                .SelectMany(i => Enumerable.Range(0, gridSize)
                    .Where(j => inputData[i][j] == 'S')
                    .Select(j => new GardenerPosition(i, j)))
                .Single();

            int stepsLeft = 64;

            var answerSet = new HashSet<GardenerPosition> { gardenerStartPos };
            for (int i = 0; i < stepsLeft; i++)
            {
                answerSet = new HashSet<GardenerPosition>(answerSet
                    .SelectMany(gs => new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right }.Select(dir => gs.Move(dir)))
                    .Where(destPos =>
                                    destPos.X >= 0 &&
                                    destPos.Y >= 0 &&
                                    destPos.X < gridSize &&
                                    destPos.Y < gridSize &&
                                    inputData[destPos.Y][destPos.X] != '#'));
            }

            return answerSet.Count;
        }

        private object RunPart2(List<string> inputData)
        {
            var gridSize = inputData.Count == inputData[0].Length ? inputData.Count : throw new ArgumentOutOfRangeException();

            var gardenerStartPos = Enumerable.Range(0, gridSize)
                .SelectMany(i => Enumerable.Range(0, gridSize)
                    .Where(j => inputData[i][j] == 'S')
                    .Select(j => new GardenerPosition(i, j)))
                .Single();

            // AFter consulting Reddit I made the assumtion that the result to be quadratic in (rem + n * gridSize) steps, i.e. (rem), (rem + gridSize), (rem + 2 * gridSize)
            var grids = 26501365 / inputData.Count;
            var rem = 26501365 % inputData.Count;

            var sequence = new List<int>();
            var answerSet = new HashSet<GardenerPosition> { gardenerStartPos };
            var steps = 0;
            for (int i = 0;i < 3;i++)
            {
                for (; steps < i * gridSize + rem; steps++)
                {
                    answerSet = new HashSet<GardenerPosition>(answerSet
                        .SelectMany(gs => new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right }.Select(dir => gs.Move(dir)))
                        .Where(destPos => inputData[((destPos.Y % 131) + 131) % 131][((destPos.X % 131) + 131) % 131] != '#'));
                }

                sequence.Add(answerSet.Count);
            }

            // Solve for the quadratic coefficients (taken from Reddit)
            var c = sequence[0];
            var aPlusB = sequence[1] - c;
            var fourAPlusTwoB = sequence[2] - c;
            var twoA = fourAPlusTwoB - (2 * aPlusB);
            var a = twoA / 2;
            var b = aPlusB - a;

            long F(long n)
            {
                return a * (n * n) + b * n + c;
            }

            return F(grids);
        }

        record GardenerPosition(int X, int Y)
        {
            public GardenerPosition Move(Direction direction)
            {
                return direction switch
                {
                    Direction.Up => new GardenerPosition(this.X, this.Y - 1),
                    Direction.Down => new GardenerPosition(this.X, this.Y + 1),
                    Direction.Left => new GardenerPosition(this.X - 1, this.Y),
                    Direction.Right => new GardenerPosition(this.X + 1, this.Y)
                };
            }
        }

        enum Direction
        {
            Up,
            Down, 
            Left, 
            Right
        }
    }
}
