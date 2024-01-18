using AoCToolbox;

namespace Day17
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 17th *";
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
            return findLeastHeatLoss(inputData, 1);
        }

        private object RunPart2(string[] inputData)
        {
            return findLeastHeatLoss(inputData, 2);
        }

        private int findLeastHeatLoss(string[] inputData, int part)
        {
            var grid = inputData.Select(line => line.Trim().Select(ch => int.Parse(ch.ToString())).ToList()).ToList();

            var seen = new HashSet<(int, int, int, int, int)>();
            var pq = new PriorityQueue<(int, int, int, int, int, int)>();

            pq.Enqueue((0, 0, 0, 0, 0, 0));

            while (pq.Count > 0)
            {
                (int heatloss, int row, int col, int dirRow, int dirCol, int numOfSteps) = pq.Dequeue();

                if (part == 1)
                {
                    if (row == grid.Count - 1 && col == grid[0].Count - 1)
                    {
                        return heatloss;
                    } 
                }
                else
                {
                    if (row == grid.Count - 1 && col == grid[0].Count - 1 && numOfSteps >= 4)
                    {
                        return heatloss;
                    }
                }

                if (seen.Contains((row, col, dirRow, dirCol, numOfSteps)))
                {
                    continue;
                }

                seen.Add((row, col, dirRow, dirCol, numOfSteps));

                if (part == 1)
                {
                    if (numOfSteps < 3 && (dirRow, dirCol) != (0, 0))
                    {
                        int nextRow = row + dirRow;
                        int nextCol = col + dirCol;

                        if (0 <= nextRow && nextRow < grid.Count && 0 <= nextCol && nextCol < grid[0].Count)
                        {
                            pq.Enqueue((heatloss + grid[nextRow][nextCol], nextRow, nextCol, dirRow, dirCol, numOfSteps + 1));
                        }
                    }

                    foreach ((int nextDirRow, int nextDirCol) in new[] { (0, 1), (1, 0), (0, -1), (-1, 0) })
                    {
                        if ((nextDirRow, nextDirCol) != (dirRow, dirCol) && (nextDirRow, nextDirCol) != (-dirRow, -dirCol))
                        {
                            int nextRow = row + nextDirRow;
                            int nextCol = col + nextDirCol;

                            if (0 <= nextRow && nextRow < grid.Count && 0 <= nextCol && nextCol < grid[0].Count)
                            {
                                pq.Enqueue((heatloss + grid[nextRow][nextCol], nextRow, nextCol, nextDirRow, nextDirCol, 1));
                            }
                        }
                    }
                }
                else
                {
                    if (numOfSteps < 10 && (dirRow, dirCol) != (0, 0))
                    {
                        int nextRow = row + dirRow;
                        int nextCol = col + dirCol;

                        if (0 <= nextRow && nextRow < grid.Count && 0 <= nextCol && nextCol < grid[0].Count)
                        {
                            pq.Enqueue((heatloss + grid[nextRow][nextCol], nextRow, nextCol, dirRow, dirCol, numOfSteps + 1));
                        }
                    }

                    if (numOfSteps >= 4 || (dirRow, dirCol) == (0, 0))
                    {
                        foreach ((int nextDirRow, int nextDirCol) in new[] { (0, 1), (1, 0), (0, -1), (-1, 0) })
                        {
                            if ((nextDirRow, nextDirCol) != (dirRow, dirCol) && (nextDirRow, nextDirCol) != (-dirRow, -dirCol))
                            {
                                int nextRow = row + nextDirRow;
                                int nextCol = col + nextDirCol;

                                if (0 <= nextRow && nextRow < grid.Count && 0 <= nextCol && nextCol < grid[0].Count)
                                {
                                    pq.Enqueue((heatloss + grid[nextRow][nextCol], nextRow, nextCol, nextDirRow, nextDirCol, 1));
                                }
                            }
                        }
                    }
                }
            }

            return -1;
        }
    }

    class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> heap;

        public int Count { get { return heap.Count; } }

        public PriorityQueue()
        {
            heap = new List<T>();
        }

        public void Enqueue(T item)
        {
            heap.Add(item);
            int i = heap.Count - 1;

            while (i > 0)
            {
                int parent = (i - 1) / 2;

                if (heap[i].CompareTo(heap[parent]) >= 0)
                    break;

                Swap(i, parent);
                i = parent;
            }
        }

        public T Dequeue()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Queue is empty.");

            T root = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);

            int i = 0;
            while (true)
            {
                int leftChild = 2 * i + 1;
                int rightChild = 2 * i + 2;

                if (leftChild >= heap.Count)
                    break;

                int smallestChild = leftChild;
                if (rightChild < heap.Count && heap[rightChild].CompareTo(heap[leftChild]) < 0)
                    smallestChild = rightChild;

                if (heap[i].CompareTo(heap[smallestChild]) <= 0)
                    break;

                Swap(i, smallestChild);
                i = smallestChild;
            }

            return root;
        }

        private void Swap(int i, int j)
        {
            T temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
    }
}
