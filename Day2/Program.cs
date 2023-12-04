using AoCToolbox;
using System.Text.RegularExpressions;

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 2nd: *");
Console.WriteLine("   Part #1");
Console.WriteLine($"     Sum Of GameIds: {RunPart1()}");
Console.WriteLine("   Part #2");
Console.WriteLine($"     Sum of power of games: {RunPart2()}");
Console.WriteLine("-------------------------");


static int RunPart1()
{
    int fixedNumberOfRed = 12;
    int fixedNumberOfGreen = 13;
    int fixedNumberOfBlue = 14;

    var gameRecords = parseGameRecords();

    foreach (var gameRecord in gameRecords)
    {
        gameRecord.IsPossibleToPlay = isPossibleToPlay(fixedNumberOfRed, fixedNumberOfGreen, fixedNumberOfBlue, gameRecord.GameSessions);
    }

    return gameRecords.Where(x => x.IsPossibleToPlay).Sum(x => x.GameId);
}

static int RunPart2()
{
    var gameRecords = parseGameRecords();

    int sumOfPowerOfSets = 0;

    foreach (var gameRecord in gameRecords)
    {
        int minNumOfRed = gameRecord.GameSessions.Max(x => x.NumberOfRed);
        int minNumOfBlue = gameRecord.GameSessions.Max(x => x.NumberOfBlue);
        int minNumOfGreen = gameRecord.GameSessions.Max(x => x.NumberOfGreen);

        int power = minNumOfRed * minNumOfBlue * minNumOfGreen;

        sumOfPowerOfSets += power;
    }

    return sumOfPowerOfSets;
}

static bool isPossibleToPlay(int numberOfRed, int numberOfGreen, int numberOfBlue, List<GameSession> gameSessions)
{
    foreach (var gameSession in gameSessions)
    {
        if (gameSession.NumberOfBlue > numberOfBlue)
        {
            return false;
        }

        if (gameSession.NumberOfGreen > numberOfGreen)
        {
            return false;
        }

        if (gameSession.NumberOfRed > numberOfRed)
        {
            return false;
        }
    }

    return true;
}

static List<GameRecord> parseGameRecords()
{
    var gameRecords = new List<GameRecord>();
    var rawRecords = new StringInputProvider("Input.txt").ToList();

    foreach (var record in rawRecords)
    {
        var rawRecord = record.Split(':');
        var gameRecord = new GameRecord();
        gameRecord.GameId = getGameId(rawRecord[0]);
        gameRecord.GameSessions = parseGameSessions(rawRecord[1].Trim());

        gameRecords.Add(gameRecord);
    }

    return gameRecords;
}
static List<GameSession> parseGameSessions(string rawSessionData)
{
    var gameSessionList = new List<GameSession>();
    var gameSessions = rawSessionData.Split(';');

    foreach (var session in gameSessions)
    {
        var gameSession = new GameSession();
        string pattern = @"^(\d.*) \w.*$";

        var colorsOfSession = session.Split(",");

        foreach (var colorOfSession in colorsOfSession)
        {
            if (colorOfSession.Contains("red"))
            {
                var match = Regex.Match(colorOfSession.Trim(), pattern);
                gameSession.NumberOfRed = int.Parse(match.Groups[1].Value);
            }

            if (colorOfSession.Contains("blue"))
            {
                var match = Regex.Match(colorOfSession.Trim(), pattern);
                gameSession.NumberOfBlue = int.Parse(match.Groups[1].Value);
            }

            if (colorOfSession.Contains("green"))
            {
                var match = Regex.Match(colorOfSession.Trim(), pattern);
                gameSession.NumberOfGreen = int.Parse(match.Groups[1].Value);
            }
        }

        gameSessionList.Add(gameSession);
    }

    return gameSessionList;
}

static int getGameId(string value)
{
    return int.Parse(value.Replace("Game ", ""));
}

class GameRecord
{
    public int GameId { get; set; }
    public List<GameSession> GameSessions { get; set; }
    public bool IsPossibleToPlay { get; set; }
}

class GameSession
{
    public int NumberOfBlue { get; set; }
    public int NumberOfRed { get; set; }
    public int NumberOfGreen { get; set; }
}
