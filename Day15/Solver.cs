using AoCToolbox;

namespace Day15
{
    public class Solver : SolverBase
    {
        public override string GetDayString()
        {
            return "* December 15th: *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Lens Library";
        }

        public override object Run(int part)
        {
            var inputData = GetInputText();

            var value = part switch
            {
                1 => RunPart1(inputData),
                2 => RunPart2(inputData)
            };

            return part switch
            {
                1 => $" Part #1\n  Sum of the Hash algorithm: {value}",
                2 => $" Part #2\n  Focusing power of the resulting lens configuration: {value}",
            };
        }

        private object RunPart1(string inputData)
        {
            return inputData.Split(',').Select(s => s.Aggregate(0,(h,c) => (h + c)*17%256)).Sum();             
        }

        private object RunPart2(string inputData)
        {
            var stepsToParse = inputData.Split(',');

            var boxes = new List<List<(string Name, int FocalLength)>>();
            Enumerable.Range(0, 256).ToList().ForEach(_ => boxes.Add(new()));

            foreach (var step in stepsToParse)
            {
                if (step.Contains('='))
                {
                    var lensName = step.Split('=')[0].Trim();
                    var box = boxes[hashStep(lensName)];
                    var focalLength = int.Parse(step.Split('=')[1].Trim());

                    var lens = (lensName, focalLength);
                    var lensIndex = box.FindIndex(l => l.Name == lensName);

                    if (lensIndex > -1)
                    {
                        box[lensIndex] = lens;
                    }
                    else
                    {
                        box.Add(lens);
                    }
                }
                else
                {
                    var lensName = step.Trim('-');
                    var box = boxes[hashStep(lensName)];
                    var lens = box.FirstOrDefault(l => l.Name == lensName);

                    if (lens != default)
                    {
                        box.Remove(lens);
                    }
                }
            }

            var focusPower = 0;
            for (var boxIndex = 0; boxIndex < boxes.Count; boxIndex++)
            {
                focusPower += boxes[boxIndex].Select((lens, lensIndex) => lens.FocalLength * (lensIndex + 1) * (boxIndex + 1)).Sum();
            }

            return focusPower;
        }

        private int hashStep(string step)
        {
            int hash = 0;
            for (var i = 0; i < step.Length; i++)
            {
                var charAtIndex = step[i];
                hash += charAtIndex;
                hash *= 17;
                hash %= 256;
            }

            return hash;
        }

    }
}
