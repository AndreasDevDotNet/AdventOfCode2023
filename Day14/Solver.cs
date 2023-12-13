using AoCToolbox;
using System;
using System.Collections.Generic;
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
            return "Problem: ?";
        }

        public override object Run(int part)
        {
            var inputData = GetInputLines().ToList();

            var value = part switch
            {
                1 => RunPart1(inputData),
                2 => RunPart2(inputData)
            };

            return $" Part #{part}\n [Result text]: {value}";
        }

        private long RunPart2(List<string> inputData)
        {
            return 0;
        }

        private long RunPart1(List<string> inputData)
        {
            return 0;
        }
    }
}
