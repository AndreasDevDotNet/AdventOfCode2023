using AoCToolbox;

namespace Day7
{
    public class Solver : SolverBase
    {
        const int FiveOfAKind = 6, FourOfAKind = 5, FullHouse = 4, ThreeOfAKind = 3, TwoPairs = 2, Pair = 1, HighCard = 0;

        public override string GetDayString()
        {
            return "* December 7th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Camel Cards";
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
                1 => $" Part #1\n  Total winnings: {value}",
                2 => $" Part #2\n  Total winnings with jokers: {value}",
            };
        }

        private object RunPart1(string[] inputData)
        {
            var hands = inputData
                .Select(l => l.Split())
                .Select(a => (hand: a[0], bid: int.Parse(a[1])))
                .ToArray();

            return SumHandsValue(hands, HandStrength1, 11);
        }

        private object RunPart2(string[] inputData)
        {
            var hands = inputData
                .Select(l => l.Split())
                .Select(a => (hand: a[0], bid: int.Parse(a[1])))
                .ToArray();

            return SumHandsValue(hands, HandStrength2, 0);
        }

        private int SumHandsValue((string hand, int bid)[] hands, Func<string, int> handStrength, int jokerStrength) => hands
            .OrderBy(h => handStrength(h.hand))
            .ThenBy(h => h.hand.Aggregate(0, (score, card) => score * 15 + CardStrength(card, jokerStrength)))
            .Select((h, rankMinusOne) => h.bid * (rankMinusOne + 1))
            .Sum();

        private int HandStrengthIgnoringJokers(string hand, out int jokerCount)
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

        private int HandStrength1(string hand) => HandStrengthIgnoringJokers(hand, out int _);

        private int HandStrength2(string hand) => (HandStrengthIgnoringJokers(hand, out int jokerCount), jokerCount) switch
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

        private int CardStrength(char card, int jokerStrength) => card switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => jokerStrength,
            'T' => 10,
            _ => card - '0'
        };
    }
}
