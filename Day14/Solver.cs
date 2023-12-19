using AoCToolbox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 14th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Parabolic Reflector Dish";
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
                1 => $" Part #1\n  Total load on the north support beam: {value}",
                2 => $" Part #2\n  Total load on the north support beam after 1000000000 cycles: {value}",
            };
        }

        private long RunPart1(string[] inputData)
        {
            tiltNorth(inputData);

            var totalLoad = 0;

            for (var row = inputData.Length; row > 0; row--)
            {
                totalLoad += inputData[^row].Count(c => c == 'O') * row;
            }

            return totalLoad;
        }

        private long RunPart2(string[] inputData)
        {
            var cache = new Dictionary<string, int>();
            var cycle = 1;

            while (cycle <= 1_000_000_000)
            {
                tiltNorth(inputData);
                tiltWest(inputData);
                tiltSouth(inputData);
                tiltEast(inputData);

                var current = string.Join(string.Empty, inputData.SelectMany(c => c));

                if (cache.TryGetValue(current, out var cached))
                {
                    var remaining = 1_000_000_000 - cycle - 1;
                    var loop = cycle - cached;

                    var loopRemaining = remaining % loop;
                    cycle = 1_000_000_000 - loopRemaining - 1;
                }

                cache[current] = cycle++;
            }

            var totalLoad = 0;

            for (var row = inputData.Length; row > 0; row--)
            {
                totalLoad += inputData[^row].Count(c => c == 'O') * row;
            }

            return totalLoad;
        }

        private void tiltNorth(string[] input)
        {
            for (var row = 1; row < input.Length; row++)
            {
                for (var col = 0; col < input[row].Length; col++)
                {
                    var c = input[row][col];

                    if (c != 'O')
                    {
                        continue;
                    }

                    var previous = 1;
                    while (input[row - previous][col] == '.')
                    {
                        input[row - previous] = input[row - previous][..col] + 'O' + input[row - previous][(col + 1)..];
                        input[row - previous + 1] = input[row - previous + 1][..col] + '.' + input[row - previous + 1][(col + 1)..];
                        previous++;

                        if (row - previous < 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void tiltSouth(string[] input)
        {
            for (var row = input.Length - 2; row >= 0; row--)
            {
                for (var col = 0; col < input[row].Length; col++)
                {
                    var c = input[row][col];

                    if (c != 'O')
                    {
                        continue;
                    }

                    var previous = 1;
                    while (input[row + previous][col] == '.')
                    {
                        input[row + previous] = input[row + previous][..col] + 'O' + input[row + previous][(col + 1)..];
                        input[row + previous - 1] = input[row + previous - 1][..col] + '.' + input[row + previous - 1][(col + 1)..];
                        previous++;

                        if (row + previous >= input.Length)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void tiltWest(string[] input)
        {
            for (var row = 0; row < input.Length; row++)
            {
                for (var col = 1; col < input[row].Length; col++)
                {
                    var c = input[row][col];

                    if (c != 'O')
                    {
                        continue;
                    }

                    var previous = 1;
                    while (input[row][col - previous] == '.')
                    {
                        input[row] = input[row][..(col - previous)] + 'O' + input[row][(col - previous + 1)..];
                        input[row] = input[row][..(col - previous + 1)] + '.' + input[row][(col - previous + 2)..];
                        previous++;

                        if (col - previous < 0)
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void tiltEast(string[] input)
        {
            for (var row = 0; row < input.Length; row++)
            {
                for (var col = input[row].Length - 2; col >= 0; col--)
                {
                    var c = input[row][col];

                    if (c != 'O')
                    {
                        continue;
                    }

                    var previous = 1;
                    while (input[row][col + previous] == '.')
                    {
                        input[row] = input[row][..(col + previous)] + 'O' + input[row][(col + previous + 1)..];
                        input[row] = input[row][..(col + previous - 1)] + '.' + input[row][(col + previous)..];
                        previous++;

                        if (col + previous >= input[row].Length)
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
