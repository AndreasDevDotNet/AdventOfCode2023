
using AoCToolbox;

Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 3rd: *");
Console.WriteLine("   Part #1");
Console.WriteLine($"     Sum of partnumbers: {RunPart1()}");
Console.WriteLine("   Part #2");
Console.WriteLine($"     Sum of gear ratios: {RunPart2()}");
Console.WriteLine("-------------------------");


static int RunPart1()
{
    var engineSchematic = new StringInputProvider("input.txt").ToList();

    int sumOfParts = 0;

    for (int row = 0; row < engineSchematic.Count; row++)
    {
        var schematicRow = engineSchematic[row];
        for (int col = 0; col < schematicRow.Length; col++)
        {
            if (schematicRow[col] != '.')
            {
                if(isAdjacentToSymbol(row, col, engineSchematic, out char symbol, out int symbolRow, out int symbolCol))
                {
                    var partnumber = getPartNumber(row, col, engineSchematic, out int startIndexOfNumber);

                    // step forward to the end of the partnumber
                    col = startIndexOfNumber + partnumber.Length;

                    sumOfParts += int.Parse(partnumber);
                }
            }
        }
    }

    return sumOfParts;
}

static int RunPart2()
{
    var engineSchematic = new StringInputProvider("input.txt").ToList();

    int gearRatio = 0;

    for (int row = 0; row < engineSchematic.Count; row++)
    {
        var schematicRow = engineSchematic[row];
        for (int col = 0; col < schematicRow.Length; col++)
        {
            if (schematicRow[col] != '.')
            {
                if (isAdjacentToSymbol(row, col, engineSchematic, out char symbol, out int symbolRow, out int symbolCol, false))
                {
                    if (symbol == '*')
                    {
                        var partnumber1 = getPartNumber(row, col, engineSchematic, out int startIndexOfNumber);

                        // step forward to the end of the partnumber
                        col = startIndexOfNumber + partnumber1.Length;

                        if (isAdjecentToPart(symbolRow, symbolCol, row,engineSchematic, out int partNoRow, out int partNoCol))
                        {
                            var partnumber2 = getPartNumber(partNoRow, partNoCol,engineSchematic, out int startIndexOfNumber1);

                            if (partnumber2 != string.Empty)
                            {
                                gearRatio += int.Parse(partnumber1) * int.Parse(partnumber2);
                            } 

                            // If the adjecent part is on the same row skip it
                            if(partNoRow == row)
                            {
                                col += partnumber2.Length;
                            }
                        }
                    }

                }
            }
        }
    }

    return gearRatio;
}

static bool isAdjecentToPart(int row, int col, int part1Row,List<string> engineSchematic, out int partNoRow, out int partNoCol)
{
    partNoRow = 0;
    partNoCol = 0;

    // Go one row down 
    if (row + 1 < engineSchematic.Count && col > 0)
    {
        var rowbelow = engineSchematic[row + 1];
        var adjColLeft = rowbelow[col - 1];
        if (adjColLeft != '.' && char.IsNumber(adjColLeft))
        {
            partNoCol = col - 1;
            partNoRow = row + 1;
            return true;
        }

        var adjColAbove = rowbelow[col];
        if (adjColAbove != '.' && char.IsNumber(adjColAbove))
        {
            partNoCol = col;
            partNoRow = row + 1;
            return true;
        }

        if (col + 1 != rowbelow.Length)
        {
            var adjColRight = rowbelow[col + 1];
            if (adjColRight != '.' && char.IsNumber(adjColRight))
            {
                partNoCol = col + 1;
                partNoRow = row + 1;
                return true;
            }
        }
    }

    if (col + 1 != engineSchematic[row].Length)
    {
        // go right
        if (engineSchematic[row][col + 1] != '.' && char.IsNumber(engineSchematic[row][col + 1]))
        {
            partNoCol = col + 1;
            partNoRow = row;
            return true;
        }
    }

    if(col - 1 != engineSchematic[row].Length && row != part1Row) // If part1 is on the same row as the symbol we will get the same part
    {
        // go left
        if (engineSchematic[row][col - 1] != '.' && char.IsNumber(engineSchematic[row][col - 1]))
        {
            partNoCol = col - 1;
            partNoRow = row;
            return true;
        }

    }

    partNoCol = 0;
    partNoRow = 0;
    return false;
}

static string getPartNumber(int row, int col, List<string> engineSchematic, out int startIndexOfNumber)
{
    string partNumber = "";
    startIndexOfNumber = 0;

    // Go forward
    if (col == 0)
    {
        var chars = engineSchematic[row].TakeWhile(x => char.IsNumber(x));
        partNumber = String.Concat(chars);
        return partNumber;
    }
    else
    {
        var schematicRow = engineSchematic[row];

        if (!char.IsNumber(schematicRow[col - 1]))
        {
            startIndexOfNumber = col;
        }

        if (startIndexOfNumber != col)
        {
            // step back until the start of the number
            for (int i = col; i > 0; i--)
            {
                if (!char.IsNumber(schematicRow[i]))
                {
                    startIndexOfNumber = i + 1;
                    i = 0;
                }

                if (startIndexOfNumber == 0)
                {
                    if (i - 1 == 0 && char.IsNumber(schematicRow[i - 1]))
                    {
                        startIndexOfNumber = 0;
                        i = 0;
                    }

                    if (i - 1 == 0 && !char.IsNumber(schematicRow[i - 1]))
                    {
                        startIndexOfNumber = 1;
                        i = 0;
                    } 
                }
            } 
        }

        var numberChars = new List<char>();
        for (int i = startIndexOfNumber;i < schematicRow.Length; i++)
        {
            if (char.IsNumber(schematicRow[i]))
            {
                numberChars.Add(schematicRow[i]);
            }
            else
            {
                i = schematicRow.Length;
            }
        }

        partNumber = String.Concat(numberChars);
        return partNumber;
    }
}

static bool isAdjacentToSymbol(int row, int col, List<string> engineSchematic, out char symbol, out int symbolRow, out int symbolCol, bool checkRowAbove = true)
{
    if (checkRowAbove)
    {
        // Go one row up 
        if (row > 0 && col > 0)
        {
            var rowabove = engineSchematic[row - 1];
            var adjColLeft = rowabove[col - 1];
            if (adjColLeft != '.' && !char.IsNumber(adjColLeft))
            {
                symbol = adjColLeft;
                symbolCol = col - 1;
                symbolRow = row - 1;
                return true;
            }

            var adjColAbove = rowabove[col];
            if (adjColAbove != '.' && !char.IsNumber(adjColAbove))
            {
                symbol = adjColAbove;
                symbolCol = col;
                symbolRow = row - 1;
                return true;
            }

            if (col + 1 != rowabove.Length)
            {
                var adjColRight = rowabove[col + 1];
                if (adjColRight != '.' && !char.IsNumber(adjColRight))
                {
                    symbol = adjColRight;
                    symbolCol = col + 1;
                    symbolRow = row - 1;
                    return true;
                }
            }
        } 
    }

    // Go one row down 
    if (row+1 < engineSchematic.Count && col > 0)
    {
        var rowbelow = engineSchematic[row + 1];
        var adjColLeft = rowbelow[col - 1];
        if (adjColLeft != '.' && !char.IsNumber(adjColLeft))
        {
            symbol = adjColLeft;
            symbolCol = col - 1;
            symbolRow = row + 1;
            return true;
        }

        var adjColAbove = rowbelow[col];
        if (adjColAbove != '.' && !char.IsNumber(adjColAbove))
        {
            symbol = adjColAbove;
            symbolCol = col;
            symbolRow = row + 1;
            return true;
        }

        if (col + 1 != rowbelow.Length)
        {
            var adjColRight = rowbelow[col + 1];
            if (adjColRight != '.' && !char.IsNumber(adjColRight))
            {
                symbol = adjColRight;
                symbolCol = col + 1;
                symbolRow = row + 1;
                return true;
            } 
        }
    }

    // go left
    if (col > 0)
    {
        if (engineSchematic[row][col - 1] != '.' && !char.IsNumber(engineSchematic[row][col - 1]))
        {
            symbol = engineSchematic[row][col - 1];
            symbolCol = col - 1;
            symbolRow = row;
            return true;
        } 
    }

    if (col + 1 != engineSchematic[row].Length)
    {
        // go right
        if (engineSchematic[row][col + 1] != '.' && !char.IsNumber(engineSchematic[row][col + 1]))
        {
            symbol = engineSchematic[row][col + 1];
            symbolCol = col + 1;
            symbolRow = row;
            return true;
        } 
    }

    symbol = '.';
    symbolCol = 0;
    symbolRow = 0;
    return false;
}


