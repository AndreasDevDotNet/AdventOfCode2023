
using AoCToolbox;

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 4th: *");
Console.WriteLine("   Problem: Scratchcards");
Console.WriteLine("   Part #1");
Console.WriteLine($"     Number of points: {RunPart1()}");
Console.WriteLine("   Part #2");
Console.WriteLine($"     Number of cards won: {RunPart2()}");
Console.WriteLine("-------------------------");


static int RunPart1()
{
    var scratchCards = parseScratchCards();

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

static int RunPart2()
{
    var scratchCards = parseScratchCards();
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
        if(cardDictIncludingCopies.ContainsKey(scratchCard.CardNumber))
        {
            cardDictIncludingCopies[scratchCard.CardNumber].Add(scratchCard);
        }
        else
        {
            cardDictIncludingCopies.Add(scratchCard.CardNumber, new List<ScratchCard> { scratchCard});
        }
    }

    numberOfCardsWon = cardDictIncludingCopies.Sum(x => x.Value.Count);

    return numberOfCardsWon;
}

static void getCopiesOfFollowupCards(int cardNumber, int numberOfWinningMatches, Dictionary<int, ScratchCard> cardDict, Dictionary<int, List<ScratchCard>> cardDictIncludingCopies)
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

static List<ScratchCard> parseScratchCards()
{
    var scratchCards = new List<ScratchCard>();
    var scratchCardInput = new StringInputProvider("input.txt").ToList();

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

static int getCardNumber(string value)
{
    return int.Parse(value.Replace("Card ", ""));
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