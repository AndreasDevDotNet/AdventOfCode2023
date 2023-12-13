using AoCToolbox;

var solverList = createSolverList();

Console.WriteLine("**** AdventOfCode 2023 ****");
Console.WriteLine("---------------------------");
foreach (var solver in solverList)
{
    Console.WriteLine(solver.GetDayString());
    Console.WriteLine(solver.GetProblemName());
    Console.WriteLine(solver.Run(1));
    Console.WriteLine(solver.Run(2));
    Console.WriteLine(solver.GetDivider());
}

List<SolverBase> createSolverList()
{
    var solverList = new List<SolverBase>
    {
        new Day1.Solver()
    };


    foreach (var solver in solverList)
    {
        solver.InputPath = "input.txt";
    }

    return solverList;
}










