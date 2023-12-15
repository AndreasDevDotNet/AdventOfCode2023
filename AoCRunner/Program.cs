using AoCToolbox;
using System.Drawing;
using Console = Colorful.Console;

var solverList = createSolverList();

Console.WriteLine("**** AdventOfCode 2023 ****", Color.MediumSpringGreen);
Console.WriteLine("---------------------------", Color.OrangeRed);
foreach (var solver in solverList)
{
    Console.WriteLine(solver.GetDayString(), Color.OrangeRed);
    Console.WriteLine(solver.GetProblemName(), Color.FromArgb(244, 212, 255));
    Console.WriteLine(solver.Run(1), Color.LimeGreen);
    Console.WriteLine(solver.Run(2), Color.LimeGreen);
    Console.WriteLine(solver.GetDivider(), Color.OrangeRed);

    Thread.Sleep(2000);
}

List<SolverBase> createSolverList()
{
    var solverList = new List<SolverBase>
    {
        new Day1.Solver(){InputPath = "1input.txt"},
        new Day2.Solver(){InputPath = "2input.txt"},
        new Day3.Solver(){InputPath = "3input.txt"},
        new Day4.Solver(){InputPath = "4input.txt"},
        //new Day5.Solver(){InputPath = "5input.txt"},
        new Day6.Solver(){InputPath = "6input.txt"},
        new Day7.Solver(){InputPath = "7input.txt"},
        new Day8.Solver(){InputPath = "8input.txt"},
        new Day10.Solver(){InputPath = "10input.txt" },
        new Day11.Solver(){InputPath = "11input.txt" },
        new Day12.Solver(){InputPath = "12input.txt" },
        new Day13.Solver(){InputPath = "13input.txt" }, 
        new Day14.Solver(){InputPath = "14input.txt"},
        new Day15.Solver(){InputPath = "15input.txt"},
    };

    return solverList;
}










