using Day7;

var solver = new Solver();
solver.InputPath = "7input.txt";

Console.WriteLine("**** AdventOfCode 2023 ****");
Console.WriteLine("---------------------------");
Console.WriteLine(solver.GetDayString());
Console.WriteLine(solver.GetProblemName());
Console.WriteLine(solver.Run(1));
Console.WriteLine(solver.Run(2));
Console.WriteLine(solver.GetDivider());