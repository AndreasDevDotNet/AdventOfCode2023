using AoCToolbox;

namespace Day17
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 17th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Clumsy Crucible";
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
                1 => $" Part #1\n  The least heat loss is: {value}",
                2 => $" Part #2\n  The least heat loss with ultra crucible is: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            var inputList = inputData.Select(x => x.Select(y => y - '0').ToList()).ToList();

            return findLeastHeatLoss(inputList, 1);
        }

        private object RunPart2(string[] inputData)
        {
            var inputList = inputData.Select(x => x.Select(y => y - '0').ToList()).ToList();

            return findLeastHeatLoss(inputList, 2);
        }

        private long findLeastHeatLoss(List<List<int>> input, int part)
        {
            long minHeatloss = 0;
            var directions = new (int x, int y)[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
            

            if (part == 1)
            {
                var visited = new bool[input[0].Count, input.Count, 4, 4];
                var work = new PriorityQueue<(int x, int y, int streak, int direction), long>();
                work.Enqueue((0, 0, 0, 0), 0);
                work.Enqueue((0, 0, 0, 1), 0);
                var mindist = new Dictionary<(int x, int y, int streak, int direction), long>();
                mindist[(0, 0, 0, 0)] = 0;
                mindist[(0, 0, 0, 1)] = 0;
                bool found = false;
                while (work.Count > 0 && !found)
                {
                    var cur = work.Dequeue();
                    visited[cur.x, cur.y, cur.direction, cur.streak] = true;
                    //add new work to the queue
                    if (cur.streak < 2)
                    {
                        var dir = cur.direction;
                        var urge = cur.streak + 1;
                        TryToAddWork(input, directions, work, mindist, cur, dir, urge, visited);

                    }
                    if (cur.direction == 0 || cur.direction == 2)
                    {
                        TryToAddWork(input, directions, work, mindist, cur, 1, 0, visited);
                        TryToAddWork(input, directions, work, mindist, cur, 3, 0, visited);
                    }
                    else if (cur.direction == 1 || cur.direction == 3)
                    {
                        TryToAddWork(input, directions, work, mindist, cur, 0, 0, visited);
                        TryToAddWork(input, directions, work, mindist, cur, 2, 0, visited);
                    }
                    if (cur.x == input[0].Count - 1 && cur.y == input.Count - 1)
                    {
                        found = true;
                        minHeatloss = mindist[cur];
                    }
                }
            }
            else
            {
                var work = new PriorityQueue<(int x, int y, int streak, int direction), long>();
                work.Enqueue((0, 0, 0, 0), 0);
                work.Enqueue((0, 0, 0, 1), 0);
                var mindist = new Dictionary<(int x, int y, int streak, int direction), long>();
                mindist[(0, 0, 0, 0)] = 0;
                mindist[(0, 0, 0, 1)] = 0;
                bool found = false;
                var visited = new bool[input[0].Count, input.Count, 4, 10];
                while (work.Count > 0 && !found)
                {

                    var cur = work.Dequeue();
                    visited[cur.x, cur.y, cur.direction, cur.streak] = true;

                    //add new work to the queue
                    if (cur.streak < 9)
                    {
                        var dir = cur.direction;
                        var urge = cur.streak + 1;
                        TryToAddWork(input, directions, work, mindist, cur, dir, urge, visited);

                    }
                    if (cur.streak >= 3)
                    {
                        if (cur.direction == 0 || cur.direction == 2)
                        {
                            TryToAddWork(input, directions, work, mindist, cur, 1, 0, visited);
                            TryToAddWork(input, directions, work, mindist, cur, 3, 0, visited);
                        }
                        else if (cur.direction == 1 || cur.direction == 3)
                        {
                            TryToAddWork(input, directions, work, mindist, cur, 0, 0, visited);
                            TryToAddWork(input, directions, work, mindist, cur, 2, 0, visited);
                        }
                        if (cur.x == input[0].Count - 1 && cur.y == input.Count - 1)
                        {
                            found = true;
                            minHeatloss = mindist[cur];
                        }
                    }
                }
            }

            return minHeatloss;
        }

        static void TryToAddWork(List<List<int>> S, (int x, int y)[] directions,
            PriorityQueue<(int x, int y, int streak, int direction), long> work,
            Dictionary<(int x, int y, int streak, int direction), long> mindist,
            (int x, int y, int streak, int direction) cur, int dir, int urge,
            bool[,,,] visited)
        {
            (int x, int y, int streak, int direction) newpos = (cur.x + directions[dir].x, cur.y + directions[dir].y, urge, dir);
            if (newpos.x >= 0 && newpos.y >= 0 && newpos.x < S[0].Count && newpos.y < S.Count && !visited[newpos.x, newpos.y, newpos.direction, newpos.streak])
            {
                var newval = mindist[cur] + S[newpos.y][newpos.x];
                if (!mindist.ContainsKey(newpos))
                {
                    mindist[newpos] = newval;
                    work.Enqueue(newpos, newval);
                }
                else if (mindist[newpos] > newval)
                {
                    mindist[newpos] = newval;
                    work.Enqueue(newpos, newval);
                }

            }
        }
    }
}
