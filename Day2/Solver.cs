using AoCToolbox;
using System.Text.RegularExpressions;

namespace Day2
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 2nd: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Cube Conundrum";
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
                1 => $" Part #1\n  Sum Of Game Ids: {value}",
                2 => $" Part #2\n  Sum of power of games: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            int fixedNumberOfRed = 12;
            int fixedNumberOfGreen = 13;
            int fixedNumberOfBlue = 14;

            var gameRecords = parseGameRecords(inputData.ToList());

            foreach (var gameRecord in gameRecords)
            {
                gameRecord.IsPossibleToPlay = isPossibleToPlay(fixedNumberOfRed, fixedNumberOfGreen, fixedNumberOfBlue, gameRecord.GameSessions);
            }

            return gameRecords.Where(x => x.IsPossibleToPlay).Sum(x => x.GameId);
        }

        private object RunPart2(string[] inputData)
        {
            var gameRecords = parseGameRecords(inputData.ToList());

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

        private bool isPossibleToPlay(int numberOfRed, int numberOfGreen, int numberOfBlue, List<GameSession> gameSessions)
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

        private List<GameRecord> parseGameRecords(List<string> rawRecords)
        {
            var gameRecords = new List<GameRecord>();

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
        private List<GameSession> parseGameSessions(string rawSessionData)
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

        private int getGameId(string value)
        {
            return int.Parse(value.Replace("Game ", ""));
        }
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
}
