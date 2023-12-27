using AoCToolbox;
using System.Collections.Generic;

namespace Day1
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 1st *";
        }
        public override string GetProblemName()
        {
            return "Problem: Trebuchet?!";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override void ShowVisualization()
        {
            throw new NotImplementedException();
        }

        public override object Run(int part)
        {
            var inputData = GetInputLines().ToList();

            var value = part switch
            {
                1 => RunPart1(inputData),
                2 => RunPart2(inputData)
            };

            return part switch
            {
                1 => $" Part #1\n  Calibration value: {value}",
                2 => $" Part #2\n  Real calibration value: {value}"
            };
        }

        private int RunPart1(List<string> inputData)
        {
            var calibrationValuesNumbers = new List<int>();

            foreach (var value in inputData)
            {
                var digitsFromString = string.Concat(value.Where(char.IsDigit));
                var first = firstDigit(int.Parse(digitsFromString));
                var last = lastDigit(int.Parse(digitsFromString));
                var calibrationValue = first + last;
                calibrationValuesNumbers.Add(int.Parse(calibrationValue));
            }

            return calibrationValuesNumbers.Sum();
        }

        private int RunPart2(List<string> inputData)
        {
            var stringDigits = new Dictionary<string, string>() { { "one", "1" }, { "two", "2" }, { "three", "3" }, { "four", "4" }, { "five", "5" }, { "six", "6" }, { "seven", "7" }, { "eight", "8" }, { "nine", "9" }, };

            var calibrationValuesNumbers = new List<int>();

            foreach (var value in inputData)
            {
                var first = firstRealDigit(value.Trim(), stringDigits);
                var last = lastRealDigit(value.Trim(), stringDigits);

                var calibrationValue = first + last;
                calibrationValuesNumbers.Add(int.Parse(calibrationValue));
            }

            return calibrationValuesNumbers.Sum();
        }

        private string firstRealDigit(string input, Dictionary<string, string> stringDigits)
        {
            var indexOfFirstDigit = input.IndexOfAny("0123456789".ToCharArray());

            var stringDigitIndexList = new List<StringDigitIndex>();

            foreach (var value in stringDigits)
            {
                stringDigitIndexList.Add(new StringDigitIndex { Index = input.IndexOf(value.Key), Digit = value.Value });
            }
            int indexOfFirstStringDigit = 99;

            if (stringDigitIndexList.Any(x => x.Index != -1))
            {
                indexOfFirstStringDigit = stringDigitIndexList.Where(x => x.Index != -1).Min(x => x.Index);
            }

            if (indexOfFirstDigit > -1 && indexOfFirstDigit < indexOfFirstStringDigit)
            {
                return firstDigit(int.Parse(input.Substring(indexOfFirstDigit, 1)));
            }
            else
            {
                return stringDigitIndexList.First(x => x.Index == indexOfFirstStringDigit).Digit;
            }
        }



        private string lastRealDigit(string input, Dictionary<string, string> stringDigits)
        {
            var indexOfLastDigit = input.LastIndexOfAny("0123456789".ToCharArray());

            var stringDigitIndexList = new List<StringDigitIndex>();

            foreach (var value in stringDigits)
            {
                stringDigitIndexList.Add(new StringDigitIndex { Index = input.LastIndexOf(value.Key), Digit = value.Value });
            }
            int indexOfLastStringDigit = -1;
            if (stringDigitIndexList.Any(x => x.Index != -1))
            {
                indexOfLastStringDigit = stringDigitIndexList.Where(x => x.Index != -1).Max(x => x.Index);
            }

            if (indexOfLastDigit > -1 && indexOfLastDigit > indexOfLastStringDigit)
            {
                return firstDigit(int.Parse(input.Substring(indexOfLastDigit, 1)));
            }
            else
            {
                return stringDigitIndexList.Last(x => x.Index == indexOfLastStringDigit).Digit;
            }
        }
        private string firstDigit(int n)
        {

            while (n >= 10)
                n /= 10;

            return n.ToString();
        }

        private string lastDigit(int n)
        {
            return (n % 10).ToString();
        }



        private class StringDigitIndex
        {
            public int Index { get; set; }
            public string Digit { get; set; }
        }
    }
}
