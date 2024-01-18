using AoCToolbox;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        List<(int, int)> points = new List<(int, int)> { (0, 0) };
        Dictionary<char, (int, int)> dirs = new Dictionary<char, (int, int)>
        {
            { 'U', (-1, 0) },
            { 'D', (1, 0) },
            { 'L', (0, -1) },
            { 'R', (0, 1) }
        };

        int b = 0;

        foreach (string line in File.ReadLines("testinput.txt"))
        {
            string[] parts = line.Split();
            string x = parts[2][2..^1];
            (int dr, int dc) = dirs["RDLU"[int.Parse(x[^1].ToString())]];
            int n = Convert.ToInt32(x[..^1], 16);
            b += n;

            (int r, int c) = points[^1];
            points.Add((r + dr * n, c + dc * n));
        }

        int A = Math.Abs(points.Select((point, i) => point.Item1 * (points[(i + 1) % points.Count].Item2 - points[i - 1].Item2)).Sum()) / 2;

        int i = A - b / 2 + 1;

        Console.WriteLine(i+ b);
    }
}
