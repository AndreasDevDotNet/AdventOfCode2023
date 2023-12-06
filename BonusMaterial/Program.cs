
using AoCToolbox;
using System.Runtime.CompilerServices;

Console.WriteLine("*** AdventOfCode 2022 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 1st: *");
Console.WriteLine("   Part #1");
Console.WriteLine($"     Number of calories: {RunPart1_dec12022()}");
Console.WriteLine("   Part #2");
Console.WriteLine($"     Number of calories of top 3: {RunPart2_dec12022()}");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 2nd: *");
Console.WriteLine("   Part #1");
Console.WriteLine($"     Total score: {RunPart1_dec22022()}");
Console.WriteLine("   Part #2");
Console.WriteLine($"     Total score (desired outcome): {RunPart2_dec22022()}");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 3rd: *");
Console.WriteLine("   Part #1");
Console.WriteLine($"     Sum of item priority: {RunPart1_dec32022()}");
Console.WriteLine("   Part #2");
Console.WriteLine($"     SUm of badge priority: {RunPart2_dec32022()}");
Console.WriteLine("-------------------------");

#region Dec1 2022
static int RunPart1_dec12022()
{
    var caloriesInput = File.ReadAllLines("input_day1_2022.txt").ToList();
    var caloriesSplitted = caloriesInput.SplitStringListBy("");

    List<Elf> elfs = parseElfList(caloriesSplitted);

    return elfs.Max(x => x.TotalCalories);
}

static int RunPart2_dec12022()
{
    var caloriesInput = File.ReadAllLines("input_day1_2022.txt").ToList();
    var caloriesSplitted = caloriesInput.SplitStringListBy("");

    List<Elf> elfs = parseElfList(caloriesSplitted);

    var top3Elfs = elfs.OrderByDescending(x => x.TotalCalories).Take(3);

    return top3Elfs.Sum(x => x.TotalCalories);
}

static List<Elf> parseElfList(List<List<string>> caloriesSplitted)
{
    int elfId = 1;
    var elfList = new List<Elf>();

    foreach (var caloriesList in caloriesSplitted)
    {
        var elf = new Elf();
        elf.ElfId = elfId;
        
        int totalCalories = 0;

        foreach (var calorie in caloriesList)
        {
            totalCalories += int.Parse(calorie);
        }
        elf.TotalCalories = totalCalories;

        elfList.Add(elf);

        elfId++;
    }

    return elfList;
}

#endregion

#region Dec2 2022

const string OpponentDrawRock = "A";
const string OpponentDrawPaper = "B";
const string OpponentDrawScissor = "C";

const string YourDrawRock = "X";
const string YourDrawPaper = "Y";
const string YourDrawScissor = "Z";


static int RunPart1_dec22022()
{
    var rounds = new InputProvider<RoundRecord?>("input_day2_2022.txt", GetRoundRecord).Where(w => w != null).Cast<RoundRecord>().ToList();

    return rounds.Select(CalculateRoundScore).Sum();
}

static int RunPart2_dec22022()
{
    var rounds = new InputProvider<RoundRecord?>("input_day2_2022.txt", GetRoundRecord).Where(w => w != null).Cast<RoundRecord>().ToList();
    return rounds.Select(CalculateMoveForOutcome).Select(CalculateRoundScore).Sum();
}

static int CalculateRoundScore(RoundRecord record)
{
    int score = 0;

    // shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors)
    score += record.Player2Move switch
    {
        Move.Rock => 1,
        Move.Paper => 2,
        Move.Scisors => 3,
        _ => throw new Exception()
    };

    // plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won)
    if (record.Player1Move == record.Player2Move)
    {
        score += 3;
    }
    else
    {
        if (record.Player2Move == Move.Rock)
        {
            if (record.Player1Move == Move.Paper)
            {
                // I lose
                score += 0;
            }
            else
            {
                score += 6;
            }
        }
        else if (record.Player2Move == Move.Paper)
        {
            if (record.Player1Move == Move.Scisors)
            {
                // I lose
                score += 0;
            }
            else
            {
                score += 6;
            }
        }
        else if (record.Player2Move == Move.Scisors)
        {
            if (record.Player1Move == Move.Rock)
            {
                // I lose
                score += 0;
            }
            else
            {
                score += 6;
            }
        }
    }

    return score;
}

static RoundRecord CalculateMoveForOutcome(RoundRecord record)
{
    Move augmentedPlayerMove;

    if (record.DesiredOutcome == Outcome.Draw)
    {
        augmentedPlayerMove = record.Player1Move;
    }
    else if (record.DesiredOutcome == Outcome.Lose)
    {
        if (record.Player1Move == Move.Rock)
        {
            augmentedPlayerMove = Move.Scisors;
        }
        else if (record.Player1Move == Move.Paper)
        {
            augmentedPlayerMove = Move.Rock;
        }
        else
        {
            augmentedPlayerMove = Move.Paper;
        }
    }
    else
    {
        if (record.Player1Move == Move.Rock)
        {
            augmentedPlayerMove = Move.Paper;
        }
        else if (record.Player1Move == Move.Paper)
        {
            augmentedPlayerMove = Move.Scisors;
        }
        else
        {
            augmentedPlayerMove = Move.Rock;
        }
    }

    return new RoundRecord(record.Player1Move, augmentedPlayerMove, record.DesiredOutcome);
}

static bool GetRoundRecord(string? input, out RoundRecord? value)
{
    value = null;

    if (input == null) return false;

    if (input.Length != 3) throw new Exception();

    var player1Move = input[0] switch
    {
        'A' => Move.Rock,
        'B' => Move.Paper,
        'C' => Move.Scisors,
        _ => throw new Exception()
    };

    var player2Move = input[2] switch
    {
        'X' => Move.Rock,
        'Y' => Move.Paper,
        'Z' => Move.Scisors,
        _ => throw new Exception()
    };

    var desiredOutcome = input[2] switch
    {
        'X' => Outcome.Lose,
        'Y' => Outcome.Draw,
        'Z' => Outcome.Win,
        _ => throw new Exception()
    };

    value = new RoundRecord(player1Move, player2Move, desiredOutcome);

    return true;
}

#endregion

#region Dec3 2022

static int RunPart1_dec32022()
{
    var backpacks = new StringInputProvider("input_day3_2022.txt").ToArray();

    int prioritySum = 0;

    foreach ( var backpack in backpacks )
    {
        int compartmentSize = backpack.Length / 2;
        var compartment1 = backpack[..compartmentSize];
        var compartment2 = backpack[compartmentSize..];

        for (int i = 0; i < compartmentSize; i++)
        {
            var item = compartment1[i];
            if(compartment2.Contains(item))
            {
                prioritySum += getItemPriority(item);
                break;
            }
        }
    }


    return prioritySum;
}

static int RunPart2_dec32022()
{
    var backpacks = new StringInputProvider("input_day3_2022.txt").ToArray();

    int prioritySum = 0;

    var backPackQueue = new Queue<string>(backpacks);

    for (int i = 0; i < backpacks.Length / 3; i++)
    {
        var backPackGroupList = new List<string>();
        backPackGroupList.Add(backPackQueue.Dequeue());
        backPackGroupList.Add(backPackQueue.Dequeue());
        backPackGroupList.Add(backPackQueue.Dequeue());

        var badgeItem = findBadgeItem(backPackGroupList);
        if(badgeItem != null)
        {
            prioritySum += getItemPriority(badgeItem.Value);
        }
    }

    return prioritySum;
}

static char? findBadgeItem(List<string> backPackGroupList)
{
    var firstRow = backPackGroupList.FirstOrDefault();
    for (int i = 0; i < firstRow.Length; i++)
    {
        if(backPackGroupList.All(x => x.Contains(firstRow[i])))
        {
            return firstRow[i];
        }
    }

    return null;
}

static int getItemPriority(char item)
{
    return item - (char.IsLower(item) ? 96 : 38);
}

#endregion


enum Move { Rock, Paper, Scisors };
enum Outcome { Lose, Draw, Win }
record RoundRecord(Move Player1Move, Move Player2Move, Outcome DesiredOutcome);

class Elf
{
    public int ElfId { get; set; }
    public int TotalCalories { get; set; }
}

