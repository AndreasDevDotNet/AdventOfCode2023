﻿using AoCToolbox;

namespace Day15
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 15th: *";
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
            var inputData = GetInputLines();

            var value = part switch
            {
                1 => RunPart1(inputData),
                2 => RunPart2(inputData)
            };

            return part switch
            {
                1 => $" Part #1\n  ???: {value}",
                2 => $" Part #2\n  ???: {value}",
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
