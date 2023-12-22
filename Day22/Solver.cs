using AoCToolbox;

namespace Day22
{
    public class Solver : SolverBase
    {
        List<Brick> _allBricks = new();
        Dictionary<char, int> _brickFallCounts = new();

        public Solver(string filePath)
        {
            InputPath = filePath;
            var inputData = GetInputLines();

            char id = 'A';
            foreach (var line in inputData)
            {
                var numbers = line.ExtractInts().ToList();
                _allBricks.Add(new Brick(id, new(numbers[0], numbers[1], numbers[2]), new(numbers[3], numbers[4], numbers[5])));
                id++;
            }

            simulateSettlingOfBricks();

            foreach (var brick in _allBricks)
            {
                checkSafeDisintegrations(brick, new(), true);
            }
        }

        public override string GetDayString()
        {
            return "* December 22nd: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Sand Slabs";
        }

        public override void ShowVisualization()
        {
        }

        public override object Run(int part)
        {
            var value = part switch
            {
                1 => RunPart1(),
                2 => RunPart2()
            };

            return part switch
            {
                1 => $" Part #1\n  Number of bricks could be safely chosen to disintegrate: {value}",
                2 => $" Part #2\n  Number of bricks that would fall Sum(for each brick): {value}",
            };
        }

        private object RunPart1()
        {
            return _brickFallCounts.Count(x => x.Value == 0);
        }

        private object RunPart2()
        {
            return _brickFallCounts.Sum(x => x.Value);
        }

        private void simulateSettlingOfBricks()
        {
            int numBricksShifted;
            do
            {
                numBricksShifted = 0;

                HashSet<Vector3D> allBrickPoints = new();

                foreach (var b in _allBricks) foreach (var p in b.Members()) allBrickPoints.Add(p);

                foreach (var b in _allBricks)
                {
                    if (b.Members().Any(m => m.Z == 1 || (allBrickPoints.Contains(m - (0, 0, 1)) && !b.Members().Contains(m - (0, 0, 1))))) continue;

                    b.Start = b.Start - (0, 0, 1);
                    b.End = b.End - (0, 0, 1);
                    numBricksShifted++;
                }


            } while (numBricksShifted != 0);

            _allBricks.Sort((a, b) => a.Start.Z.CompareTo(b.Start.Z));

            //Get everyone's interactions with one another.
            foreach (var b in _allBricks)
            {
                foreach (var p in _allBricks.Where(a => a.Start.Z == b.End.Z + 1))
                {
                    foreach (var m in b.Members())
                    {
                        if (p.Members().Any(a => a.X == m.X && a.Y == m.Y && a.Z - 1 == m.Z))
                        {
                            b.Supports.Add(p);
                            p.SupportedBy.Add(b);
                            break;
                        }
                    }
                }
            }
        }

        private void checkSafeDisintegrations(Brick brick, List<char> bricksToIgnore, bool updateDictionary = false)
        {
            bricksToIgnore.Add(brick.Id);

            if(brick.Supports.Count == 0)
            {
                if (updateDictionary)
                {
                    _brickFallCounts[brick.Id] = 0; 
                }
            }

            var bricksThatWouldFall = brick.Supports.Where(b => b.SupportedBy.Count(b2 => !bricksToIgnore.Contains(b2.Id)) == 0).ToList();
            bricksThatWouldFall.ForEach(b => bricksToIgnore.Add(b.Id));

            bricksThatWouldFall.ForEach(b => checkSafeDisintegrations(b, bricksToIgnore));

            if(updateDictionary)
            {
                _brickFallCounts[brick.Id] = bricksToIgnore.Distinct().Count() - 1;
            }
        }

        class Brick
        {
            public char Id;
            public Vector3D Start;
            public Vector3D End;

            public List<Brick> Supports = new();
            public List<Brick> SupportedBy = new();

            public int Length => Vector3D.Distance(Start, End, Metric.Taxicab);

            public Brick(char id, Vector3D coord1, Vector3D coord2)
            {
                Id = id;
                if(coord1.X < coord2.X || coord1.Y < coord2.Y || coord1.Z < coord2.Z)
                {
                    Start = coord1;
                    End = coord2;
                }
                else
                {
                    Start = coord2;
                    End = coord1;
                }
            }

            public IEnumerable<Vector3D> Members()
            {
                yield return Start;
                var curLoc = Start;
                while (curLoc != End)
                {
                    if (Start.X == End.X && Start.Y == End.Y) curLoc = curLoc + (0, 0, 1); //Vertically Oriented Brick 
                    else if (Start.X == End.X && Start.Z == End.Z) curLoc = curLoc + (0, 1, 0); //Oriented Along Y- Axis
                    else if (Start.Y == End.Y && Start.Z == End.Z) curLoc = curLoc + (1, 0, 0); //Oriented Along x- Axis
                    yield return curLoc;
                }
            }

            public override string ToString() 
            {
                return Id.ToString();
            }
        }
    }
}
