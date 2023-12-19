using AoCToolbox;
using System.Drawing;
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
                1 => $" Part #1\n  How many cubic meters of lava could it hold?: {value}",
                2 => $" Part #2\n  How many cubic meters of lava could it hold (heaxadecimal parse)?: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            var polygon = new List<Point>();

            var currentPosition = new Point(0, 0);
            
            var edge = 0.0;

            foreach (var line in inputData)
            {
                polygon.Add(currentPosition);
                var move = line.Split(' ');

                var length = int.Parse(move[1]);
                currentPosition = move[0] switch
                {
                    "R" => new Point(currentPosition.X + length, currentPosition.Y),
                    "D" => new Point(currentPosition.X, currentPosition.Y + length),
                    "L" => new Point(currentPosition.X - length, currentPosition.Y),
                    "U" => new Point(currentPosition.X, currentPosition.Y - length),
                    _ => throw new Exception("Unknown direction")
                };

                edge += length;
            }

            return polygon.ShoelaceArea() + edge / 2 + 1;
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

            return polygon.ShoelaceArea() + edge / 2 + 1;
        }
    }
}
