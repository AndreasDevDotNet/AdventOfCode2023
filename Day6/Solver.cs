using AoCToolbox;
using System.Text.RegularExpressions;

namespace Day6
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 6th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Wait For It";
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
                1 => $" Part #1\n  Number of ways to beat the record (multiplied): {value}",
                2 => $" Part #2\n  Number of ways to beat the longer race record: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            var regex = new Regex(@"\d+");
            var raceMaxTimes = regex.Matches(inputData[0]).Select(r => int.Parse(r.Value)).ToList();
            var raceRecordDistances = regex.Matches(inputData[1]).Select(r => int.Parse(r.Value)).ToList();

            var marginOfError = 1;
            for (var i = 0; i < raceMaxTimes.Count; ++i)
            {
                var timesToBeatRecord = 0;
                for (var j = 0; j < raceMaxTimes[i]; ++j)
                {
                    if (j * (raceMaxTimes[i] - j) > raceRecordDistances[i])
                    {
                        timesToBeatRecord++;
                    }
                }

                marginOfError *= timesToBeatRecord;
            }

            return marginOfError;
        }

        private object RunPart2(string[] inputData)
        {
            var raceMaxTime = long.Parse(inputData[0].Split(':')[1].Replace(" ", ""));
            var raceRecordDistance = long.Parse(inputData[1].Split(':')[1].Replace(" ", ""));

            long timesToBeatRealRecord = 0;
            for (long i = 0; i < raceMaxTime; ++i)
            {
                if (i * (raceMaxTime - i) > raceRecordDistance)
                {
                    timesToBeatRealRecord++;
                }
            }

            return timesToBeatRealRecord;
        }
    }
}
