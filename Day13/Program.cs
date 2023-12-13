using AoCToolbox;
using System.Text;

var inputData = File.ReadAllText("input.txt");

List<string> maps = new();
Dictionary<int, (int num, bool isRow)> initialMirrors = new();
maps = inputData.SplitByDoubleNewline();

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 13th: *");
Console.WriteLine("   Problem: Point of Incidence");
Console.WriteLine("   Part #1");
Console.WriteLine($"      Sum of reflections : {RunPart1()}");
Console.WriteLine("   Part #2");
Console.WriteLine($"      Sum of reflections after smudge removed : {RunPart2()}");
Console.WriteLine("-------------------------");

int RunPart1()
{
    int sum = 0;
    for (int i = 0; i < maps.Count; i++)
    {
        string block = maps[i];
        tryFindReflection(block, i, out int res);
        sum += res;
    }

    return sum;
}

long RunPart2()
{
    int sum = 0;
    for (int j = 0; j < maps.Count; j++)
    {
        string block = maps[j];
        for (int i = 0; i < block.Length; i++)
        {
            if (!".#".Contains(block[i])) continue;
            StringBuilder sb = new StringBuilder(block);

            sb[i] = sb[i] == '.' ? '#' : '.';
            int res = 0;
            if (tryFindReflection(sb.ToString(), j, out res))
            {
                sum += res;
                break;
            }

        }
    }

    return sum;
}

bool tryFindReflection(string block, int Id, out int result)
{
    var asRows = block.SplitByNewline();
    var asColumns = block.SplitIntoColumns().ToList();
    //Check rows
    for (int i = 1; i < asRows.Count; i++)
    {
        if (asRows.Take(i).Reverse().Zip(asRows.Skip(i)).All(x => x.First == x.Second))
        {
            if (initialMirrors.TryGetValue(Id, out (int num, bool isRow) x))
            {
                if (x.isRow && x.num == i) continue;
            }
            initialMirrors[Id] = (i, true);
            result = i * 100;
            return true;
        }
    }

    for (int i = 1; i < asColumns.Count; i++)
    {
        if (asColumns.Take(i).Reverse().Zip(asColumns.Skip(i)).All(x => x.First == x.Second))
        {
            if (initialMirrors.TryGetValue(Id, out (int num, bool isRow) x))
            {
                if (!x.isRow && x.num == i) continue;

            }
            initialMirrors[Id] = (i, false);
            result = i;
            return true;
        }
    }

    result = 0;
    return false;
}

