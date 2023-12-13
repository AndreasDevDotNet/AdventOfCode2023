var inputData = File.ReadAllLines("testinput.txt");

var cache = new Dictionary<string, long>();

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 12th: *");
Console.WriteLine("   Problem: Hot Springs");
Console.WriteLine("   Part #1");
Console.WriteLine($"      Number of possible arrangements : {RunPart1(inputData)}");
Console.WriteLine("   Part #2");
Console.WriteLine($"      Number of possible arrangements unfolded : {RunPart2(inputData)}");
Console.WriteLine("-------------------------");

long RunPart1(string[] inputData)
{
    long arrangements = 0;

    foreach (var line in inputData.Select(l => l.Split(' ')))
    {
        var springs = line[0];
        var groups = line[1].Split(',').Select(int.Parse).ToList();

        arrangements += calculate(springs, groups);
    }

    return arrangements;
}

long? RunPart2(string[] inputData)
{
    long arrangements = 0;

    foreach (var line in inputData.Select(l => l.Split(' ')))
    {
        var springs = line[0];
        var groups = line[1].Split(',').Select(int.Parse).ToList();
        springs = string.Join('?', Enumerable.Repeat(springs, 5));
        groups = Enumerable.Repeat(groups, 5).SelectMany(g => g).ToList();

        arrangements += calculate(springs, groups);
    }

    return arrangements;
}

long calculate(string springs, List<int> groups)
{
    var key = $"{springs},{string.Join(',', groups)}";  // Cache key: spring pattern + group lengths

    if (cache.TryGetValue(key, out var value))
    {
        return value;
    }

    value = getCount(springs, groups);
    cache[key] = value;

    return value;
}

long getCount(string springs, List<int> groups)
{
    while (true)
    {
        if (groups.Count == 0)
        {
            return springs.Contains('#') ? 0 : 1; // No more groups to match: if there are no springs left, we have a match
        }

        if (string.IsNullOrEmpty(springs))
        {
            return 0; // No more springs to match, although we still have groups to match
        }

        if (springs.StartsWith('.'))
        {
            springs = springs.Trim('.'); // Remove all dots from the beginning
            continue;
        }

        if (springs.StartsWith('?'))
        {
            return calculate("." + springs[1..], groups) + calculate("#" + springs[1..], groups); // Try both options recursively
        }

        if (springs.StartsWith('#')) // Start of a group
        {
            if (groups.Count == 0)
            {
                return 0; // No more groups to match, although we still have a spring in the input
            }

            if (springs.Length < groups[0])
            {
                return 0; // Not enough characters to match the group
            }

            if (springs[..groups[0]].Contains('.'))
            {
                return 0; // Group cannot contain dots for the given length
            }

            if (groups.Count > 1)
            {
                if (springs.Length < groups[0] + 1 || springs[groups[0]] == '#')
                {
                    return 0; // Group cannot be followed by a spring, and there must be enough characters left
                }

                springs = springs[(groups[0] + 1)..]; // Skip the character after the group - it's either a dot or a question mark
                groups = groups[1..];
                continue;
            }

            springs = springs[groups[0]..]; // Last group, no need to check the character after the group
            groups = groups[1..];
            continue;
        }

        throw new Exception("Invalid input");
    }
}

