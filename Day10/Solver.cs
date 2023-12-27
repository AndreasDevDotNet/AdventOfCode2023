using AoCToolbox;
using System.Diagnostics;

namespace Day10
{
    public class Solver : SolverBase
    {
        public Solver()
        {
            HasVisualization = true;
        }

        public override string GetDayString()
        {
            return "* December 10th *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: PipeMaze";
        }

        public override void ShowVisualization()
        {

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
                1 => $" Part #1\n  Number of steps until farthest in the loop: {value}",
                2 => $" Part #2\n  Number of tiles inside the loop: {value}",
            };
        }

        private object RunPart1(string[] grid)
        {
            int startRow = -1, startCol = -1;

            HashSet<(int, int)> loop = new HashSet<(int, int)>();
            Queue<(int, int)> q = new Queue<(int, int)>();

            // Find start row and column
            for (int r = 0; r < grid.Length; r++)
            {
                for (int c = 0; c < grid[r].Length; c++)
                {
                    char ch = grid[r][c];
                    if (ch == 'S')
                    {
                        startRow = r;
                        startCol = c;
                        break;
                    }
                }
                if (startRow != -1) break;
            }

            loop.Add((startRow, startCol));
            q.Enqueue((startRow, startCol));
            // Run trough the loop
            while (q.Count > 0)
            {
                (int r, int c) = q.Dequeue();
                char ch = grid[r][c];

                if (r > 0 && "S|JL".Contains(ch) && "|7F".Contains(grid[r - 1][c]) && !loop.Contains((r - 1, c)))
                {
                    loop.Add((r - 1, c));
                    q.Enqueue((r - 1, c));
                }

                if (r < grid.Length - 1 && "S|7F".Contains(ch) && "|JL".Contains(grid[r + 1][c]) && !loop.Contains((r + 1, c)))
                {
                    loop.Add((r + 1, c));
                    q.Enqueue((r + 1, c));
                }

                if (c > 0 && "S-J7".Contains(ch) && "-LF".Contains(grid[r][c - 1]) && !loop.Contains((r, c - 1)))
                {
                    loop.Add((r, c - 1));
                    q.Enqueue((r, c - 1));
                }

                if (c < grid[r].Length - 1 && "S-LF".Contains(ch) && "-J7".Contains(grid[r][c + 1]) && !loop.Contains((r, c + 1)))
                {
                    loop.Add((r, c + 1));
                    q.Enqueue((r, c + 1));
                }
            }

            return loop.Count / 2;
        }

        private object RunPart2(string[] grid)
        {
            int startRow = -1, startCol = -1;

            HashSet<(int, int)> loop = new HashSet<(int, int)>();
            Queue<(int, int)> q = new Queue<(int, int)>();
            HashSet<char> maybeS = new HashSet<char> { '|', '-', 'J', 'L', '7', 'F' };

            // Find start row and column
            for (int r = 0; r < grid.Length; r++)
            {
                for (int c = 0; c < grid[r].Length; c++)
                {
                    char ch = grid[r][c];
                    if (ch == 'S')
                    {
                        startRow = r;
                        startCol = c;
                        break;
                    }
                }
                if (startRow != -1) break;
            }

            loop.Add((startRow, startCol));
            q.Enqueue((startRow, startCol));

            // Run trough the loop, also try to find out what kind of pipe 'S' is
            while (q.Count > 0)
            {
                (int r, int c) = q.Dequeue();
                char ch = grid[r][c];

                if (r > 0 && "S|JL".Contains(ch) && "|7F".Contains(grid[r - 1][c]) && !loop.Contains((r - 1, c)))
                {
                    loop.Add((r - 1, c));
                    q.Enqueue((r - 1, c));
                    if (ch == 'S')
                    {
                        maybeS.IntersectWith(new HashSet<char> { '|', 'J', 'L' });
                    }
                }

                if (r < grid.Length - 1 && "S|7F".Contains(ch) && "|JL".Contains(grid[r + 1][c]) && !loop.Contains((r + 1, c)))
                {
                    loop.Add((r + 1, c));
                    q.Enqueue((r + 1, c));
                    if (ch == 'S')
                    {
                        maybeS.IntersectWith(new HashSet<char> { '|', '7', 'F' });
                    }
                }

                if (c > 0 && "S-J7".Contains(ch) && "-LF".Contains(grid[r][c - 1]) && !loop.Contains((r, c - 1)))
                {
                    loop.Add((r, c - 1));
                    q.Enqueue((r, c - 1));
                    if (ch == 'S')
                    {
                        maybeS.IntersectWith(new HashSet<char> { '-', 'J', '7' });
                    }
                }

                if (c < grid[r].Length - 1 && "S-LF".Contains(ch) && "-J7".Contains(grid[r][c + 1]) && !loop.Contains((r, c + 1)))
                {
                    loop.Add((r, c + 1));
                    q.Enqueue((r, c + 1));
                    if (ch == 'S')
                    {
                        maybeS.IntersectWith(new HashSet<char> { '-', 'L', 'F' });
                    }
                }
            }

            char S = maybeS.Single();

            // Fix grid and replace 'S' with what is underneath
            grid = grid.Select(row => row.Replace('S', S)).ToArray();
            // Remove all the garbage pipes
            grid = grid.Select((row, r) => new string(row.Select((ch, c) => loop.Contains((r, c)) ? ch : '.').ToArray())).ToArray();

            // Find all positions that are outside the loop
            HashSet<(int, int)> outside = new HashSet<(int, int)>();
            for (int r = 0; r < grid.Length; r++)
            {
                bool within = false;
                bool? up = null;

                for (int c = 0; c < grid[r].Length; c++)
                {
                    char ch = grid[r][c];

                    if (ch == '|')
                    {
                        Debug.Assert(up is null);
                        within = !within;
                    }
                    else if (ch == '-')
                    {
                        Debug.Assert(up is not null);
                    }
                    else if (ch == 'L' || ch == 'F')
                    {
                        Debug.Assert(up is null);
                        up = ch == 'L';
                    }
                    else if (ch == 'J' || ch == '7')
                    {
                        Debug.Assert(up is not null);

                        if (ch != (up.Value ? 'J' : '7'))
                        {
                            within = !within;
                        }
                        up = null;
                    }
                    else if (ch == '.')
                    {
                        // Do nothing
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unexpected character (horizontal): {ch}");
                    }

                    if (!within)
                    {
                        outside.Add((r, c));
                    }
                }
            }

            // number of tiles inside the loop: gridLength * gridWidth - (outsideTiles - loopTiles)
            return grid.Length * grid[0].Length - (outside.Union(loop)).Count();
        }

    }
}
