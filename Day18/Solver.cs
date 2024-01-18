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
            var polygonResult = digOutPolygon(inputData, 1);

            return polygonResult.polygon.ShoelaceArea() + polygonResult.boundry / 2 + 1;
        }

        private object RunPart2(string[] inputData)
        {
            var polygonResult = digOutPolygon(inputData, 2);

            return polygonResult.polygon.ShoelaceArea() + polygonResult.boundry / 2 + 1;
        }

        private (List<(long row, long col)> polygon, double boundry) digOutPolygon(string[] inputData, int part)
        {
            var points = new List<(long, long)> { (0, 0) };
            double boundry = 0.0;

            foreach (string line in inputData)
            {
                var ld = parseLengthAndDirection(line, part);
                boundry += ld.length;

                (long row, long col) = points[^1];
                points.Add((row + ld.direction.dirRow * ld.length, col + ld.direction.dirCol * ld.length));
            }

            return (points, boundry);
        }

        private ((int dirRow, int dirCol) direction, long length) parseLengthAndDirection(string line, int part)
        {
            var dirs = new Dictionary<char, (int, int)>
            {
                { 'U', (-1, 0) },
                { 'D', (1, 0) },
                { 'L', (0, -1) },
                { 'R', (0, 1) }
            };
            var dirs2 = new Dictionary<char, (int, int)>
            {
                { '3', (-1, 0) },
                { '2', (0, -1) },
                { '1', (1, 0) },
                { '0', (0, 1) }
            };

            if (part == 1)
            {
                string[] parts = line.Split();
                char dir = parts[0][0];
                long length = long.Parse(parts[1]);

                (int dirRow, int dirCol) = dirs[dir];
                return ((dirRow, dirCol), length);
            }
            else
            {
                var hexNumber = line.Split(' ')[2].TrimStart('(').TrimEnd(')');
                var length = long.Parse(hexNumber[1..^1], NumberStyles.HexNumber);
                (int dirRow, int dirCol) = dirs2[hexNumber.Last()];
                return ((dirRow, dirCol), length);
            }
            
        }


    }
}
