using AoCToolbox;
using System.Diagnostics;

namespace Day23
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 23rd: *";
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
            };

            return part switch
            {
                1 => $" Part #1\n  Number of steps for the longest hike: {value}",
                2 => $" Part #2\n  Number of steps for the longest hike without caring about slope rules: {value}",
            };
        }

        private object RunPart1(string[] map)
        {
            var start = new MapPosition(0, 1);
            var end = new MapPosition(map.Length - 1, map[0].Length - 2);

            var graph = BuildGraph(map);

            return DFS(graph, [], start, end);
        }

        private object RunPart2(string[] map)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var start = new MapPosition(0, 1);
            var end = new MapPosition(map.Length - 1, map[0].Length - 2);

            var graph = BuildGraph(map,false);

            var longestHike = DFS(graph, [], start, end);
            stopWatch.Stop();

            return longestHike;
        }

        // Recursive Depth First Search (DFS) using brute force
        int DFS(Dictionary<MapPosition, HashSet<MapPosition>> graph, HashSet<MapPosition> visited, MapPosition current, MapPosition end)
        {
            var result = 0;

            if (current == end)
            {
                return visited.Count;
            }

            foreach (var neighbor in graph[current])
            {
                if (!visited.Add(neighbor))
                {
                    continue;
                }

                var length = DFS(graph, visited, neighbor, end);
                result = Math.Max(result, length);
                visited.Remove(neighbor);
            }

            return result;
        }

        static Dictionary<MapPosition, HashSet<MapPosition>> BuildGraph(string[] input, bool useSlopeRules = true)
        {
            var graph = new Dictionary<MapPosition, HashSet<MapPosition>>();
            for (var row = 0; row < input.Length; row++)
            {
                var line = input[row];

                for (var col = 0; col < line.Length; col++)
                {
                    if (line[col] == '#')
                    {
                        continue;
                    }

                    var pos = new MapPosition(row, col);
                    graph[pos] = [];

                    if (useSlopeRules)
                    {
                        switch (line[col])
                        {
                            case '>':
                                graph[pos].Add(pos.Move(Direction.Right));
                                continue;
                            case 'v':
                                graph[pos].Add(pos.Move(Direction.Down));
                                continue;
                            case '<':
                                graph[pos].Add(pos.Move(Direction.Left));
                                continue;
                        }
                    }

                    if (row > 0 && input[row - 1][col] != '#')
                    {
                        graph[pos].Add(pos.Move(Direction.Up));
                    }

                    if (row < input.Length - 1 && input[row + 1][col] != '#')
                    {
                        graph[pos].Add(pos.Move(Direction.Down));
                    }

                    if (col > 0 && input[row][col - 1] != '#')
                    {
                        graph[pos].Add(pos.Move(Direction.Left));
                    }

                    if (col < line.Length - 1 && input[row][col + 1] != '#')
                    {
                        graph[pos].Add(pos.Move(Direction.Right));
                    }
                }
            }

            return graph;
        }

        record Direction(int Row, int Col)
        {
            public static Direction Up = new(-1, 0);
            public static Direction Down = new(1, 0);
            public static Direction Left = new(0, -1);
            public static Direction Right = new(0, 1);
        }

        record MapPosition(int Row, int Col)
        {
            public MapPosition Move(Direction dir)
            {
                return new MapPosition(Row + dir.Row, Col + dir.Col);
            }
        }
    }
}
