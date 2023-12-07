var inputData = File.ReadAllLines("input.txt");

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 7th: *");
Console.WriteLine("   Part #1");
Console.WriteLine($"     Total winnings: {RunPart1(inputData)}");
Console.WriteLine("   Part #2");
Console.WriteLine($"     Total winnings(with joker): {RunPart2(inputData)}");
Console.WriteLine("-------------------------");

const int FiveOfAKind = 6, FourOfAKind = 5, FullHouse = 4, ThreeOfAKind = 3, TwoPairs = 2, Pair = 1, HighCard = 0;

static long RunPart1(string[] inputLines)
{
    var hands = inputLines
        .Select(l => l.Split())
        .Select(a => (hand: a[0], bid: int.Parse(a[1])))
        .ToArray();

    return SumHandsValue(hands, HandStrength1, 11);
}

static long RunPart2(string[] inputLines)
{
    var hands = inputLines
    .Select(l => l.Split())
    .Select(a => (hand: a[0], bid: int.Parse(a[1])))
    .ToArray();

    return SumHandsValue(hands, HandStrength2, 0);
}

static int SumHandsValue((string hand, int bid)[] hands, Func<string, int> handStrength, int jokerStrength) => hands
    .OrderBy(h => handStrength(h.hand))
    .ThenBy(h => h.hand.Aggregate(0, (score, card) => score * 15 + CardStrength(card, jokerStrength)))
    .Select((h, rankMinusOne) => h.bid * (rankMinusOne + 1))
    .Sum();

static int HandStrengthIgnoringJokers(string hand, out int jokerCount)
{
    var counts = hand.GroupBy(card => card).ToDictionary(group => group.Key, group => group.Count());
    counts.TryGetValue('J', out jokerCount);
    return counts.Count switch
    {
        1 => FiveOfAKind,
        2 => counts.Values.Any(count => count == 4) ? FourOfAKind : FullHouse,
        3 => counts.Values.Any(count => count == 2) ? TwoPairs : ThreeOfAKind,
        4 => Pair,
        _ => HighCard
    };
}

static int HandStrength1(string hand) => HandStrengthIgnoringJokers(hand, out int _);

static int HandStrength2(string hand) => (HandStrengthIgnoringJokers(hand, out int jokerCount), jokerCount) switch
{
    (var handStrengthIgnoringJokers, 0) => handStrengthIgnoringJokers,
    (HighCard, 1) => Pair,
    (Pair, _) => ThreeOfAKind,
    (TwoPairs, 1) => FullHouse,
    (TwoPairs, 2) => FourOfAKind,
    (ThreeOfAKind, _) => FourOfAKind,
    (FullHouse, 1) => FourOfAKind,
    (FullHouse, _) => FiveOfAKind,
    (FourOfAKind, _) => FiveOfAKind,
    (var handStrengthIgnoringJokers, _) => handStrengthIgnoringJokers
};

static int CardStrength(char card, int jokerStrength) => card switch
{
    'A' => 14,
    'K' => 13,
    'Q' => 12,
    'J' => jokerStrength,
    'T' => 10,
    _ => card - '0'
};



