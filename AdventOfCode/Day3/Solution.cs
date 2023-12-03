namespace AdventOfCode.Day3;

public class Solution
{
 
    public char[,] ReadFromFile()
    {
        var contents = new List<string>();

        string line;
        try
        {
            StreamReader sr = new StreamReader("Day3/input1.txt");

            line = sr.ReadLine();

            while (line != null)
            {
                contents.Add(line);
                line = sr.ReadLine();
            }

            sr.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        var height = contents.Count;
        var width = contents.Select(x => x.Length).Max();

        var result = new char[height, width];

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                result[i,j] = contents[i][j];
            }
        }

        return result;
    }

    private readonly Predicate<char> IsSymbol = (x) => x != 'x' && !char.IsDigit(x);

    public bool CheckForSymbol(int i, int j, char[,] data)
    {
        var result = false;

        if (i - 1 >= 0)
        {
            var symbol = data[i - 1, j];
            
            result = IsSymbol(symbol);

            if (j - 1 >= 0)
            {
                var symbolLeftUp = data[i - 1, j-1];
                result = IsSymbol(symbolLeftUp);
            }

            if(j + 1 < data.GetLength(1))
            {
                var symbolRightUp = data[i - 1, j + 1];
                result = IsSymbol(symbolRightUp);
            }
        }

        if (i + 1 < data.GetLength(0))
        {
            var symbol = data[i+1, j];

            result = IsSymbol(symbol);

            if (j - 1 >= 0)
            {
                var symbolLeftDown = data[i + 1, j - 1];
                result = IsSymbol(symbolLeftDown);
            }

            if (j + 1 < data.GetLength(1))
            {
                var symbolRightDown = data[i + 1, j + 1];
                result = IsSymbol(symbolRightDown);
            }
        }

        if (j - 1 >= 0)
        {
            var symbol = data[i, j - 1];
            result = IsSymbol(symbol);

        }

        if (j + 1 < data.GetLength(1))
        {
            var symbol = data[i, j + 1];
            result = IsSymbol(symbol);
        }

        return result;
    }

    public int Execute()
    {
        var data = ReadFromFile();

        var resultOne = new List<int>();

        var currentNumber = "";
        var currentNext = false;

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                var x = data[i,j];

                if (char.IsDigit(x))
                {
                    currentNumber = currentNumber + x;

                    if (!currentNext)
                    {
                        currentNext = CheckForSymbol(i, j, data);
                    }
                    
                }
                else
                {
                    if (currentNumber != "")
                    {
                        if (currentNext)
                        {
                            resultOne.Add(int.Parse(currentNumber));
                        }

                        currentNumber = "";
                        currentNext = false;
                    }
                }

            }

        }

        var resultTwo = new List<int>();

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                var x = data[i, j];

                if (x == '*')
                {

                    var numberUp = TakeNumber(i - 1, j, data);
                    var numberRow = TakeNumber(i, j, data);
                    var numberDown = TakeNumber(i + 1, j, data);

                    var numbersList = numberUp.Concat(numberRow).Concat(numberDown);

                    var finalNumbers = numbersList.Where(x => x != 0).ToList();

                    if (finalNumbers.Count == 2)
                    {
                        var result = finalNumbers.Aggregate(1, (cur, nxt) => cur * nxt);

                        resultTwo.Add(result);
                    }
                }
            }
        }


        return resultTwo.Sum();
    }

    private List<int> TakeNumber(int i, int j, char[,] data)
    {
        var result = new List<int>();
        var number = "";

        var current = data[i,j];
        var idxFirst = j - 1;

        while(idxFirst >= 0 && char.IsDigit(data[i, idxFirst]))
        {
            number = data[i, idxFirst] + number;
            idxFirst = idxFirst - 1;
        }

        if (char.IsDigit(current))
        {
            number = number + current;
        } else
        {
            if (number != "")
            {
                result.Add(int.Parse(number));
                number = "";
            }
        }

        var idxSecond = j+1;

        while (idxSecond < data.GetLength(1) && char.IsDigit(data[i, idxSecond]))
        {
            number = number + data[i, idxSecond];
            idxSecond = idxSecond + 1;
        }

        if (number != "")
        {
            result.Add(int.Parse(number));
        }

        return result;

        
    }
}
