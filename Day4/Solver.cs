using AoCToolbox;

namespace Day4
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 4th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Scratchcards";
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
            var scratchCards = parseScratchCards(inputData.ToList());

            int numberOfPoints = 0;

            foreach (var scratchCard in scratchCards)
            {
                int pointsForCard = 1;

                var numberOfWinningMatches = scratchCard.NumberOfWinningMatches;

                for (int i = 1; i < numberOfWinningMatches; i++)
                {
                    pointsForCard = pointsForCard + pointsForCard;
                }

                if (numberOfWinningMatches == 0)
                {
                    pointsForCard = 0;
                }

                numberOfPoints += pointsForCard;
            }

            return numberOfPoints;
        }

        private object RunPart2(string[] inputData)
        {
            var scratchCards = parseScratchCards(inputData.ToList());
            var cardDict = scratchCards.ToDictionary(x => x.CardNumber, x => x);
            var cardDictIncludingCopies = new Dictionary<int, List<ScratchCard>>();

            int numberOfCardsWon = 0;

            // 1st run
            foreach (var scratchCard in scratchCards)
            {
                if (scratchCard.NumberOfWinningMatches > 0)
                {
                    getCopiesOfFollowupCards(scratchCard.CardNumber, scratchCard.NumberOfWinningMatches, cardDict, cardDictIncludingCopies);
                }
            }

            // 2nd run

            foreach (var scratchCardCopies in cardDictIncludingCopies)
            {
                foreach (var scratchCard in scratchCardCopies.Value)
                {
                    getCopiesOfFollowupCards(scratchCard.CardNumber, scratchCard.NumberOfWinningMatches, cardDict, cardDictIncludingCopies);
                }
            }


            // Add orignal cards
            foreach (var scratchCard in scratchCards)
            {
                if (cardDictIncludingCopies.ContainsKey(scratchCard.CardNumber))
                {
                    cardDictIncludingCopies[scratchCard.CardNumber].Add(scratchCard);
                }
                else
                {
                    cardDictIncludingCopies.Add(scratchCard.CardNumber, new List<ScratchCard> { scratchCard });
                }
            }

            numberOfCardsWon = cardDictIncludingCopies.Sum(x => x.Value.Count);

            return numberOfCardsWon;
        }

        private void getCopiesOfFollowupCards(int cardNumber, int numberOfWinningMatches, Dictionary<int, ScratchCard> cardDict, Dictionary<int, List<ScratchCard>> cardDictIncludingCopies)
        {
            int nextCardNumber = cardNumber + 1;

            for (int cardNo = 0; cardNo < numberOfWinningMatches; cardNo++)
            {
                if (cardDict.ContainsKey(nextCardNumber))
                {
                    if (cardDictIncludingCopies.ContainsKey(nextCardNumber))
                    {
                        cardDictIncludingCopies[nextCardNumber].Add(cardDict[nextCardNumber]);
                    }
                    else
                    {
                        cardDictIncludingCopies.Add(nextCardNumber, new List<ScratchCard> { cardDict[nextCardNumber] });
                    }
                }

                nextCardNumber++;
            }
        }

        private List<ScratchCard> parseScratchCards(List<string> scratchCardInput)
        {
            var scratchCards = new List<ScratchCard>();

            foreach (var cardInput in scratchCardInput)
            {
                var splittedCardInput = cardInput.Split(':');

                var scratchCard = new ScratchCard();
                scratchCard.CardNumber = getCardNumber(splittedCardInput[0]);

                var cardNumbers = splittedCardInput[1].Trim().Split("|");
                var winningNumbers = cardNumbers[0].Trim().Split(' ');
                var scratchedNumbers = cardNumbers[1].Trim().Split(" ");

                foreach (var winningNumber in winningNumbers)
                {
                    if (winningNumber != string.Empty)
                    {
                        scratchCard.WinningNumbers.Add(int.Parse(winningNumber));
                    }
                }

                foreach (var scratchedNumber in scratchedNumbers)
                {
                    if (scratchedNumber != string.Empty)
                    {
                        scratchCard.ScratchedNumbers.Add(int.Parse(scratchedNumber));
                    }
                }

                scratchCards.Add(scratchCard);

            }

            return scratchCards;
        }

        private int getCardNumber(string value)
        {
            return int.Parse(value.Replace("Card ", ""));
        }
    }

    class ScratchCard
    {
        public ScratchCard()
        {
            WinningNumbers = new List<int>();
            ScratchedNumbers = new List<int>();
        }

        public int CardNumber { get; set; }
        public List<int> WinningNumbers { get; set; }
        public List<int> ScratchedNumbers { get; set; }
        public int NumberOfWinningMatches => WinningNumbers.Intersect(ScratchedNumbers).ToList().Count();
    }
}
