using AoCToolbox;
using System.Drawing;
using System.Numerics;
using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

namespace Day10
{
    public class Solver : SolverBase
    {
        private Complex Up = -Complex.ImaginaryOne;
        private Complex Down = Complex.ImaginaryOne;
        private Complex Left = -Complex.One;
        private Complex Right = Complex.One;
        private Complex[] Dirs;

        public Solver()
        {
            HasVisualization = true;
        }

        public override string GetDayString()
        {
            return "* December 10th: *";
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
            var inputData = GetInputText();

            var map = ParseMap(inputData);
            var loop = LoopPositions(map);


        }

        public override object Run(int part)
        {
            var inputData = GetInputText();
            Dirs = [Up, Right, Down, Left];

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

        private object RunPart1(string inputData)
        {
            var map = ParseMap(inputData);
            var loop = LoopPositions(map);
            return loop.Count / 2;
        }

        private object RunPart2(string inputData)
        {
            var map = ParseMap(inputData);
            var loop = LoopPositions(map);
            map = Filter(map, loop);
            map = Replace(map, '.', 'I');
            map = ScaleUp(map);
            map = Fill(map, Complex.Zero, ".I", 'O');
            return map.Values.Count(v => v == 'I');
        }

        // Finds 'S' in the map and returns the coordinates that make up the loop
        private HashSet<Complex> LoopPositions(Map map)
        {
            var curr = map.Keys.Single(k => map[k] == 'S');
            var positions = new HashSet<Complex>() { };
            var dir = Dirs.First(dir => DirsIn(map[curr + dir]).Contains(dir));

            for (; !positions.Contains(curr);)
            {
                positions.Add(curr);
                curr += dir;
                if (map[curr] == 'S')
                {
                    break;
                }
                var outs = DirsOut(map[curr]);
                dir = DirsOut(map[curr]).Single(dirOut => dirOut != -dir);
            }
            return positions;
        }

        // Fills the map using flood fill replacing chars of charsToFill with the
        // given fillChar. Every other character of the map is considered as a wall.
        private Map Fill(Map map, Complex start, string charsToFill, char fillChar)
        {
            var q = new Queue<Complex>();
            q.Enqueue(start);
            while (q.Any())
            {
                var p = q.Dequeue();
                if (!charsToFill.Contains(map[p]))
                {
                    continue;
                }
                map[p] = fillChar;
                foreach (var d in Dirs)
                {
                    if (map.ContainsKey(p + d))
                    {
                        q.Enqueue(p + d);
                    }
                }
            }
            return map;
        }

        // In which directions can a cell left
        private Complex[] DirsOut(char ch) => ch switch
        {
            '7' => [Left, Down],
            'F' => [Right, Down],
            'L' => [Up, Right],
            'J' => [Up, Left],
            '|' => [Up, Down],
            '-' => [Left, Right],
            'S' => [Up, Down, Left, Right],
            _ => []
        };

        // In which directions can a cell entered
        private Complex[] DirsIn(char ch) =>
            DirsOut(ch).Select(ch => -ch).ToArray();

        private Map ParseMap(string input)
        {
            var rows = input.Split("\n");
            var crow = rows.Length;
            var ccol = rows[0].Length;
            var res = new Map();
            for (var irow = 0; irow < crow; irow++)
            {
                for (var icol = 0; icol < ccol; icol++)
                {
                    res[new Complex(icol, irow)] = rows[irow][icol];
                }
            }
            return res;
        }

        private Map Filter(Map map, HashSet<Complex> keep) =>
            map.Keys.ToDictionary(k => k, k => keep.Contains(k) ? map[k] : '.');

        private Map Replace(Map map, char chSrc, char chDst) =>
            map.Keys.ToDictionary(k => k, k => map[k] == chSrc ? chDst : map[k]);


        // Adds a 1 width border of '.' around the map
        private Map Pad(Map map)
        {
            var ccol = map.Keys.Select(k => k.Real).Max();
            var crow = map.Keys.Select(k => k.Imaginary).Max();

            for (var irow = -1; irow <= crow; irow++)
            {
                map[new Complex(0, irow)] = '.';
                map[new Complex(ccol + 1, irow)] = '.';
            }
            for (var icol = -1; icol <= ccol; icol++)
            {
                map[new Complex(icol, 0)] = '.';
                map[new Complex(icol, crow + 1)] = '.';
            }

            return map.Keys.ToDictionary(k => k + new Complex(1, 1), k => map[k]);
        }

        // Creates a 3x scaled up map applying the patterns of MagnifyCh
        private Map ScaleUp(Map map)
        {
            var ccol = map.Keys.Select(k => k.Real).Max();
            var crow = map.Keys.Select(k => k.Imaginary).Max();
            var newMap = new Map();
            for (var irow = 0; irow < crow; irow++)
            {
                for (var icol = 0; icol < ccol; icol++)
                {
                    var ch = map[new Complex(icol, irow)];
                    var m = MagnifyCh(ch);
                    for (var v = 0; v < 9; v++)
                    {
                        newMap[new Complex(3 * icol + (v % 3), 3 * irow + v / 3)] = m[v];
                    }
                }
            }
            return newMap;
        }

        // Defines a 3x3 magnifiying pattern for a char
        private string MagnifyCh(char ch) => ch switch
        {
            '7' => "...-7..|.",
            'F' => "....F-.|.",
            'L' => ".|..L-...",
            'J' => ".|.-J....",
            '|' => ".|..|..|.",
            '-' => "...---...",
            'S' => ".|.-S-.|.",
            _ => "...." + ch + "....", // just leave it in the middle
        };
    }
}
