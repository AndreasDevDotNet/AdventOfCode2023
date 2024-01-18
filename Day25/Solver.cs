using AoCToolbox;

namespace Day25
{
    public class Solver : SolverBase
    {
        private  List<(string, string)> edges;
        private  List<string> vertices;

        public override string GetDayString()
        {
            return "* December 25th (Merry Christmas)*";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Snowverload";
        }

        public override void ShowVisualization()
        {
        }

        public override object Run(int part)
        {
            var inputData = GetInputLines();
            processInput(inputData);

            var value = part switch
            {
                1 => RunPart1(),
                2 => RunPart2()
            };

            return part switch
            {
                1 => $" Part #1\n  Size the divided groups multiplied: {value}",
                2 => $" Part #2\n {value}",
            };
        }

        private void processInput(string[] input)
        {
            vertices = new List<string>();
            edges = new List<(string, string)>();

            foreach (var line in input)
            {
                var split = line.Split(": ");
                string component = split[0];
                split = split[1].Split(' ');
                HashSet<string> connected = new HashSet<string>(split);

                if (!vertices.Contains(component)) vertices.Add(component);

                foreach (var c in connected)
                {
                    if (!vertices.Contains(c)) vertices.Add(c);
                    if (!edges.Contains((component, c)) && !edges.Contains((c, component))) edges.Add((component, c));
                }
            }
        }

        private object RunPart1()
        {
            return divideGroups();
        }

        private object RunPart2()
        {
            return "AoC 2023 is over...";
        }

        private int divideGroups()
        {
            List<List<string>> subsets = new List<List<string>>();

            do
            {
                subsets = new List<List<string>>();

                foreach (var vertex in vertices)
                {
                    subsets.Add(new List<string>() { vertex });
                }

                int i;
                List<string> subset1, subset2;

                while (subsets.Count > 2)
                {
                    i = new Random().Next() % edges.Count;

                    subset1 = subsets.Where(s => s.Contains(edges[i].Item1)).First();
                    subset2 = subsets.Where(s => s.Contains(edges[i].Item2)).First();

                    if (subset1 == subset2) continue;

                    subsets.Remove(subset2);
                    subset1.AddRange(subset2);
                }

            } while (CountCuts(subsets) != 3);

            return subsets.Aggregate(1, (p, s) => p * s.Count);
        }

        private int CountCuts(List<List<string>> subsets)
        {
            int cuts = 0;
            for (int i = 0; i < edges.Count; ++i)
            {
                var subset1 = subsets.Where(s => s.Contains(edges[i].Item1)).First();
                var subset2 = subsets.Where(s => s.Contains(edges[i].Item2)).First();
                if (subset1 != subset2) ++cuts;
            }

            return cuts;
        }

    }
}
