using AoCToolbox;

namespace Day16
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 16th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: The Floor Will Be Lava";
        }

        public override object Run(int part)
        {
            var inputData = GetInputLines();
            var grid = Grid2D<char>.MapChars(inputData, c => c);

            var value = part switch
            {
                1 => RunPart1(grid),
                2 => RunPart2(grid)
            };

            return part switch
            {
                1 => $" Part #1\n  Number of tiles end up being energized: {value}",
                2 => $" Part #2\n  Number of tiles end up being energized max directions: {value}",
            };
        }

        private object RunPart1(Grid2D<char> grid)
        {
            return countEnergized(grid, -1,grid.Height - 1, Vector2D.Right);
        }

        private object RunPart2(Grid2D<char> grid)
        {
            var max = int.MinValue;
            for (var x = 0; x < grid.Width; x++)
            {
                max = Math.Max(max, countEnergized(grid, x, y: grid.Height, face: Vector2D.Down));
                max = Math.Max(max, countEnergized(grid, x, y: -1, face: Vector2D.Up));
            }
            for (var y = 0; y < grid.Height; y++)
            {
                max = Math.Max(max, countEnergized(grid, x: -1, y: y, face: Vector2D.Right));
                max = Math.Max(max, countEnergized(grid, x: grid.Width, y: y, face: Vector2D.Left));
            }
            return max;
        }

        private int countEnergized(Grid2D<char> grid, int x, int y, Vector2D face)
        {
            var start = new Pose2D(pos: new Vector2D(x, y), face: face);
            var queue = new Queue<Pose2D>(collection: [start]);
            var visited = new HashSet<Pose2D>();

            while (queue.Count > 0)
            {
                var pose = queue.Dequeue();
                var ahead = pose.Ahead;

                if (!grid.IsInDomain(ahead))
                {
                    continue;
                }

                var entity = grid[ahead];
                var yields = new List<Pose2D>(capacity: 2);

                switch (entity, pose.Face.X, pose.Face.Y)
                {
                    case (entity: '\\', X: 0, Y: 1):
                    case (entity: '/', X: 0, Y: -1):
                    case (entity: '-', X: -1, Y: 0):
                        yields.Add(item: new Pose2D(pos: ahead, face: Vector2D.Left));
                        break;
                    case (entity: '\\', X: 0, Y: -1):
                    case (entity: '/', X: 0, Y: 1):
                    case (entity: '-', X: 1, Y: 0):
                        yields.Add(item: new Pose2D(pos: ahead, face: Vector2D.Right));
                        break;
                    case (entity: '\\', X: -1, Y: 0):
                    case (entity: '/', X: 1, Y: 0):
                    case (entity: '|', X: 0, Y: 1):
                        yields.Add(item: new Pose2D(pos: ahead, face: Vector2D.Up));
                        break;
                    case (entity: '\\', X: 1, Y: 0):
                    case (entity: '/', X: -1, Y: 0):
                    case (entity: '|', X: 0, Y: -1):
                        yields.Add(item: new Pose2D(pos: ahead, face: Vector2D.Down));
                        break;
                    case (entity: '-', X: _, Y: _):
                        yields.Add(item: new Pose2D(pos: ahead, face: Vector2D.Left));
                        yields.Add(item: new Pose2D(pos: ahead, face: Vector2D.Right));
                        break;
                    case (entity: '|', X: _, Y: _):
                        yields.Add(item: new Pose2D(pos: ahead, face: Vector2D.Up));
                        yields.Add(item: new Pose2D(pos: ahead, face: Vector2D.Down));
                        break;
                    case (entity: '.', X: _, Y: _):
                        yields.Add(item: pose.Step());
                        break;
                }

                foreach (var yield in yields)
                {
                    if (visited.Add(yield))
                    {
                        queue.Enqueue(yield);
                    }
                }
            }

            return visited.DistinctBy(pose => pose.Pos).Count();
        }
    }
}
