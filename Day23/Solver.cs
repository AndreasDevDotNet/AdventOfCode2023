using AoCToolbox;
using System.Diagnostics;

namespace Day23
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 23rd *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: A Long Walk";
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
            } ;

            return part switch
            {
                1 => $" Part #1\n  Number of steps for the longest hike: {value}",
                2 => $" Part #2\n  Number of steps for the longest hike without caring about slope rules: {value}",
            };
        }

        private object RunPart1(string[] map)
        {
            return findLongestPath(map);
        }

        private object RunPart2(string[] map)
        {
            return findLongestPath(map, false);
        }

        private long findLongestPath(string[] grid, bool useSlopeRules = true)
        {
            var start = (0, Array.IndexOf(grid[0].ToCharArray(), '.'));
            var end = (grid.Length - 1, Array.IndexOf(grid[^1].ToCharArray(), '.'));

            var points = new List<(int, int)> { start, end };

            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col++)
                {
                    if (grid[row][col] == '#')
                        continue;

                    int neighbors = 0;
                    foreach (var (neighborRow, neighborCol) in new[] { (row - 1, col), (row + 1, col), (row, col - 1), (row, col + 1) })
                    {
                        if (0 <= neighborRow && neighborRow < grid.Length && 0 <= neighborCol && neighborCol < grid[0].Length && grid[neighborRow][neighborCol] != '#')
                            neighbors++;
                    }

                    if (neighbors >= 3)
                        points.Add((row, col));
                }
            }

            var graph = points.ToDictionary(pt => pt, _ => new Dictionary<(int, int), int>());

            var dirs = new Dictionary<char, List<(int, int)>>
            {
                { '^', new List<(int, int)> { (-1, 0) } },
                { 'v', new List<(int, int)> { (1, 0) } },
                { '<', new List<(int, int)> { (0, -1) } },
                { '>', new List<(int, int)> { (0, 1) } },
                { '.', new List<(int, int)> { (-1, 0), (1, 0), (0, -1), (0, 1) } }
            };

            foreach (var (startRow, startCol) in points)
            {
                var stack = new Stack<(int length, int row, int col)>();
                var seen = new HashSet<(int, int)>();

                stack.Push((0, startRow, startCol));
                seen.Add((startRow, startCol));

                while (stack.Count > 0)
                {
                    var (length, row, col) = stack.Pop();

                    if (length != 0 && points.Contains((row, col)))
                    {
                        graph[(startRow, startCol)][(row, col)] = length;
                        continue;
                    }

                    if (useSlopeRules)
                    {
                        foreach (var (dr, dc) in dirs[grid[row][col]])
                        {
                            var nr = row + dr;
                            var nc = col + dc;

                            if (0 <= nr && nr < grid.Length && 0 <= nc && nc < grid[0].Length && grid[nr][nc] != '#' && !seen.Contains((nr, nc)))
                            {
                                stack.Push((length + 1, nr, nc));
                                seen.Add((nr, nc));
                            }
                        } 
                    }
                    else
                    {
                        foreach (var (dr, dc) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
                        {
                            var nr = row + dr;
                            var nc = col + dc;

                            if (0 <= nr && nr < grid.Length && 0 <= nc && nc < grid[0].Length && grid[nr][nc] != '#' && !seen.Contains((nr, nc)))
                            {
                                stack.Push((length + 1, nr, nc));
                                seen.Add((nr, nc));
                            }
                        }
                    }
                }
            }

            var seenSet = new HashSet<(int, int)>();

            int DFS((int, int) pt)
            {
                if (pt == end)
                    return 0;

                int m = int.MinValue;

                seenSet.Add(pt);

                foreach (var nx in graph[pt])
                {
                    if (!seenSet.Contains(nx.Key))
                        m = Math.Max(m, DFS(nx.Key) + graph[pt][nx.Key]);
                }

                seenSet.Remove(pt);

                return m;
            }

            return DFS(start);
        }
    }
}
