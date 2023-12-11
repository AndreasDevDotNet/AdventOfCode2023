﻿using System.Collections.Immutable;
using System.Numerics;

using Map = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;

var inputData = File.ReadAllText("input.txt");

Complex Up = -Complex.ImaginaryOne;
Complex Down = Complex.ImaginaryOne;
Complex Left = -Complex.One;
Complex Right = Complex.One;
Complex[] Dirs = [Up, Right, Down, Left];

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 10th: *");
Console.WriteLine("   Problem: PipeMaze");
Console.WriteLine("   Part #1");
Console.WriteLine($"      Number of steps from starting position : {RunPart1(inputData)}");
Console.WriteLine("   Part #2");
Console.WriteLine($"      Number of tiles inside the loop : {RunPart2(inputData)}");
Console.WriteLine("-------------------------");

long RunPart1(string inputData)
{
    var map = ParseMap(inputData);
    var loop = LoopPositions(map);
    return loop.Count / 2;
}

long RunPart2(string inputData)
{
    var map = ParseMap(inputData);
    var loop = LoopPositions(map);
    map = Filter(map, loop);
    map = Replace(map, '.', 'I');
    map = ScaleUp(map);
    map = Fill(map, Complex.Zero, ".I", 'O');
    return map.Values.Count(v => v == 'I');
}

// Finds 'S' in the map and returns the coordinates that make up the loop
HashSet<Complex> LoopPositions(Map map)
{
    var curr = map.Keys.Single(k => map[k] == 'S');
    var positions = new HashSet<Complex>() { };
    var dir = Dirs.First(dir => DirsIn(map[curr + dir]).Contains(dir));

    for (; !positions.Contains(curr);)
    {
        positions.Add(curr);
        curr += dir;
        if (map[curr] == 'S')
        {
            break;
        }
        var outs = DirsOut(map[curr]);
        dir = DirsOut(map[curr]).Single(dirOut => dirOut != -dir);
    }
    return positions;
}

// Fills the map using flood fill replacing chars of charsToFill with the
// given fillChar. Every other character of the map is considered as a wall.
Map Fill(Map map, Complex start, string charsToFill, char fillChar)
{
    var q = new Queue<Complex>();
    q.Enqueue(start);
    while (q.Any())
    {
        var p = q.Dequeue();
        if (!charsToFill.Contains(map[p]))
        {
            continue;
        }
        map[p] = fillChar;
        foreach (var d in Dirs)
        {
            if (map.ContainsKey(p + d))
            {
                q.Enqueue(p + d);
            }
        }
    }
    return map;
}

// In which directions can a cell left
Complex[] DirsOut(char ch) => ch switch
{
    '7' => [Left, Down],
    'F' => [Right, Down],
    'L' => [Up, Right],
    'J' => [Up, Left],
    '|' => [Up, Down],
    '-' => [Left, Right],
    'S' => [Up, Down, Left, Right],
    _ => []
};

// In which directions can a cell entered
Complex[] DirsIn(char ch) =>
    DirsOut(ch).Select(ch => -ch).ToArray();

Map ParseMap(string input)
{
    var rows = input.Split("\n");
    var crow = rows.Length;
    var ccol = rows[0].Length;
    var res = new Map();
    for (var irow = 0; irow < crow; irow++)
    {
        for (var icol = 0; icol < ccol; icol++)
        {
            res[new Complex(icol, irow)] = rows[irow][icol];
        }
    }
    return res;
}

Map Filter(Map map, HashSet<Complex> keep) =>
    map.Keys.ToDictionary(k => k, k => keep.Contains(k) ? map[k] : '.');

Map Replace(Map map, char chSrc, char chDst) =>
    map.Keys.ToDictionary(k => k, k => map[k] == chSrc ? chDst : map[k]);


// Adds a 1 width border of '.' around the map
Map Pad(Map map)
{
    var ccol = map.Keys.Select(k => k.Real).Max();
    var crow = map.Keys.Select(k => k.Imaginary).Max();

    for (var irow = -1; irow <= crow; irow++)
    {
        map[new Complex(0, irow)] = '.';
        map[new Complex(ccol + 1, irow)] = '.';
    }
    for (var icol = -1; icol <= ccol; icol++)
    {
        map[new Complex(icol, 0)] = '.';
        map[new Complex(icol, crow + 1)] = '.';
    }

    return map.Keys.ToDictionary(k => k + new Complex(1, 1), k => map[k]);
}

// Creates a 3x scaled up map applying the patterns of MagnifyCh
Map ScaleUp(Map map)
{
    var ccol = map.Keys.Select(k => k.Real).Max();
    var crow = map.Keys.Select(k => k.Imaginary).Max();
    var newMap = new Map();
    for (var irow = 0; irow < crow; irow++)
    {
        for (var icol = 0; icol < ccol; icol++)
        {
            var ch = map[new Complex(icol, irow)];
            var m = MagnifyCh(ch);
            for (var v = 0; v < 9; v++)
            {
                newMap[new Complex(3 * icol + (v % 3), 3 * irow + v / 3)] = m[v];
            }
        }
    }
    return newMap;
}

// Defines a 3x3 magnifiying pattern for a char
string MagnifyCh(char ch) => ch switch
{
    '7' => "...-7..|.",
    'F' => "....F-.|.",
    'L' => ".|..L-...",
    'J' => ".|.-J....",
    '|' => ".|..|..|.",
    '-' => "...---...",
    'S' => ".|.-S-.|.",
    _ => "...." + ch + "....", // just leave it in the middle
};