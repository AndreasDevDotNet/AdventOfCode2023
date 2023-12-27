using AoCToolbox;

namespace Day4
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 4th *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Scratchcards";
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
                1 => $" Part #1\n  Number of points: {value}",
                2 => $" Part #2\n  Number of cards won: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            int totalPoints = 0;

            foreach (var line in inputData)
            {
                var numbers = line.Split(":")[1].Trim();
                var numberArrays = numbers.Split(" | ").Select(x => x.ExtractInts().ToList()).ToList();
                var winningNumbers = numberArrays[0];
                var yourNumbers = numberArrays[1];

                var cardPoints = yourNumbers.Count(q => winningNumbers.Contains(q));
                if (cardPoints > 0)
                {
                    totalPoints += (int)Math.Pow(2, cardPoints - 1);
                }
            }

            return totalPoints;
        }

        private object RunPart2(string[] inputData)
        {
            Dictionary<int, int> cardMap = new Dictionary<int, int>();

            foreach ((string line, int i) in inputData.Select((line, index) => (line, index)))
            {
                if (!cardMap.ContainsKey(i))
                {
                    cardMap[i] = 1;
                }

                var numbers = line.Split(":")[1].Trim();
                var numberArrays = numbers.Split(" | ").Select(x => x.ExtractInts().ToList()).ToList();
                var winningNumbers = numberArrays[0];
                var yourNumbers = numberArrays[1];

                var cardPoints = yourNumbers.Count(q => winningNumbers.Contains(q));

                for (int n = i + 1; n <= i + cardPoints; n++)
                {
                    if (!cardMap.ContainsKey(n))
                    {
                        cardMap[n] = 1;
                    }

                    cardMap[n] += cardMap[i];
                }
            }

            var numberOfCardsWon = cardMap.Values.Sum();

            return numberOfCardsWon;
        }
    }
}
