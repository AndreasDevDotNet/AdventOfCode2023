using AoCToolbox;
using System.Text;

namespace Day13
{
    public class Solver : SolverBase
    {
        List<string> maps = new();
        Dictionary<int, (int num, bool isRow)> initialMirrors = new();
        

        public override string GetDayString()
        {
            return "* December 13th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Point of Incidence";
        }

        public override void ShowVisualization()
        {
            throw new NotImplementedException();
        }

        public override object Run(int part)
        {
            var inputData = GetInputText();
            maps = inputData.SplitByDoubleNewline();

            var value = part switch
            {
                1 => RunPart1(),
                2 => RunPart2()
            };

            return part switch
            {
                1 => $" Part #1\n  Line of reflection: {value}",
                2 => $" Part #2\n  Line of reflection with the smudge removed: {value}",
            };
        }

        private object RunPart1()
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

        private object RunPart2()
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

        private bool tryFindReflection(string block, int Id, out int result)
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
    }
}
