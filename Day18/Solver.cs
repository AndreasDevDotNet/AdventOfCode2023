using AoCToolbox;
using System.Globalization;

namespace Day18
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 18th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Lavaduct Lagoon";
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
                1 => $" Part #1\n  How many cubic meters of lava could it hold?: {value}",
                2 => $" Part #2\n  How many cubic meters of lava could it hold (heaxadecimal parse)?: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            var polygon = new List<(long Row, long Col)>();

            (long Row, long Col) currentPosition = (0, 0);
            
            var edge = 0.0;

            foreach (var line in inputData)
            {
                polygon.Add(currentPosition);
                var move = line.Split(' ');

                var length = int.Parse(move[1]);
                currentPosition = move[0] switch
                {
                    "R" => (currentPosition.Row, currentPosition.Col + length),
                    "D" => (currentPosition.Row + length, currentPosition.Col),
                    "L" => (currentPosition.Row, currentPosition.Col - length),
                    "U" => (currentPosition.Row - length, currentPosition.Col),
                    _ => throw new Exception("Unknown direction")
                };

                edge += length;
            }

            return Area(polygon) + edge / 2 + 1;
        }

        private object RunPart2(string[] inputData)
        {
            var polygon = new List<(long Row, long Col)>();

            (long Row, long Col) currentPosition = (0, 0);

            var edge = 0.0;

            foreach (var line in inputData)
            {
                polygon.Add(currentPosition);

                var hexNumber = line.Split(' ')[2].TrimStart('(').TrimEnd(')');
                var length = long.Parse(hexNumber[1..^1], NumberStyles.HexNumber);

                currentPosition = hexNumber.Last() switch
                {
                    '0' => (currentPosition.Row, currentPosition.Col + length), // R
                    '1' => (currentPosition.Row + length, currentPosition.Col), // D
                    '2' => (currentPosition.Row, currentPosition.Col - length), // L
                    '3' => (currentPosition.Row - length, currentPosition.Col), // U
                    _ => throw new Exception("Unknown direction")
                };

                edge += length;
            }

            return Area(polygon) + edge / 2 + 1;
        }

        // Shoelace formula, found it on the internet:
        // https://rosettacode.org/wiki/Shoelace_formula_for_polygonal_area#C#
        private double Area(List<(long Row, long Col)> polygon)
        {
            var n = polygon.Count;
            var result = 0.0;
            for (var i = 0; i < n - 1; i++)
            {
                result += polygon[i].Row * polygon[i + 1].Col - polygon[i + 1].Row * polygon[i].Col;
            }

            result = Math.Abs(result + polygon[n - 1].Row * polygon[0].Col - polygon[0].Row * polygon[n - 1].Col) / 2.0;
            return result;
        }
    }
}
