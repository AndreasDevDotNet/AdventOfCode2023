using AoCToolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return "Problem: ?";
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
                1 => $" Part #1\n  ??: {value}",
                2 => $" Part #2\n  ??: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            return null;
        }

        private object RunPart2(string[] inputData)
        {
            return null;
        }
    }
}
