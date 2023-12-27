using AoCToolbox;
using System.Drawing;

namespace Day3
{
    public class Solver : SolverBase
    {
        const char Void = '.';
        const char Gear = '*';

        bool schematicBuilt = false;

        Dictionary<Point,char> symobolDictionary = new Dictionary<Point,char>();
        Dictionary<Point[],int> partDictionary = new Dictionary<Point[],int>();
        Dictionary<Point,char> gearDictionary = new Dictionary<Point,char>();

        public override string GetDayString()
        {
            return "* December 3rd *";
        }

        public override string GetDivider()
        {
            return "---------------------------";
        }

        public override string GetProblemName()
        {
            return "Problem: Gear Ratios";
        }

        public override void ShowVisualization()
        {
            throw new NotImplementedException();
        }

        public override object Run(int part)
        {
            var inputData = GetInputLines();
            buildSchematic(inputData);

            var value = part switch
            {
                1 => RunPart1(),
                2 => RunPart2()
            };

            return part switch
            {
                1 => $" Part #1\n  Sum of partnumbers: {value}",
                2 => $" Part #2\n  Sum of gear ratios: {value}",
            };
        }

        private object RunPart1()
        {
            int sumOfParts = 0;

            foreach (var partPosition in partDictionary)
            {
                foreach (var pos in partPosition.Key)
                {
                    if(isAdjacentToSymbol(pos))
                    {
                        sumOfParts += partPosition.Value;
                        break;
                    }
                }
            }

            return sumOfParts;
        }

        private object RunPart2()
        {
            int gearRatios = 0;

            foreach (var possibleGear in gearDictionary)
            {
                var partNumbersConnectedToGear = new List<int>();
                foreach (var partPosition in partDictionary)
                {
                    if(possibleGear.Key.IsNeighbourWithDiagnoalsOnRange(partPosition.Key))
                    {
                        partNumbersConnectedToGear.Add(partPosition.Value);
                    }
                }
                if(partNumbersConnectedToGear.Count == 2)
                {
                    gearRatios += partNumbersConnectedToGear[0] * partNumbersConnectedToGear[1];
                }
            }

            return gearRatios;
        }

        private bool isAdjacentToSymbol(Point partPos)
        {
            foreach(var symbolPos in symobolDictionary)
            {
                if(partPos.IsNeighbourWithDiagnoals(symbolPos.Key))
                {
                    return true;
                }
            }

            return false;
        }

        private void buildSchematic(string[] inputData)
        {
            if (schematicBuilt)
                return;

            for (int row = 0; row < inputData.Length; row++)
            {
                var schematicRow = inputData[row];
                for (int col = 0; col < schematicRow.Length; col++)
                {
                    if (schematicRow[col] != Void)
                    {
                        if (char.IsNumber(schematicRow[col]))
                        {
                            var numCounter = col;
                            string partNo = "";
                            var numPoints = new List<Point>();
                            while (numCounter < schematicRow.Length && char.IsNumber(schematicRow[numCounter]))
                            {
                                partNo += schematicRow[numCounter];
                                numPoints.Add(new Point(numCounter, row));
                                numCounter++;
                            }
                            partDictionary.Add(numPoints.ToArray(), int.Parse(partNo));
                            col = numCounter-1;
                        }
                        else
                        {
                            if (schematicRow[col] == Gear)
                            {
                                gearDictionary.Add(new Point(col, row), schematicRow[col]);
                            }
                            symobolDictionary.Add(new Point(col, row), schematicRow[col]);
                        }
                    }
                }
            }

            schematicBuilt = true;
        }
    }
}
