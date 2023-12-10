using System.Text.RegularExpressions;

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 6th: *");
Console.WriteLine("   Problem: Wait For It");
Console.WriteLine("   Part #1");

var input = File.ReadAllLines("input.txt");

var regex = new Regex(@"\d+");
var raceMaxTimes = regex.Matches(input[0]).Select(r => int.Parse(r.Value)).ToList();
var raceRecordDistances = regex.Matches(input[1]).Select(r => int.Parse(r.Value)).ToList();

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

Console.WriteLine($"     Number of ways to beat the record (multiplied): {marginOfError}");
Console.WriteLine("   Part #2");

var raceMaxTime = long.Parse(input[0].Split(':')[1].Replace(" ",""));
var raceRecordDistance = long.Parse(input[1].Split(':')[1].Replace(" ", ""));

long timesToBeatRealRecord = 0;
for (long i = 0; i < raceMaxTime; ++i)
{
    if(i * (raceMaxTime - i) > raceRecordDistance)
    {
        timesToBeatRealRecord++;
    }
}

Console.WriteLine($"     Number of ways to beat the longer race record: {timesToBeatRealRecord}");
Console.WriteLine("-------------------------");


