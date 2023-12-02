namespace AdventOfCode.Day1;

public class Solution
{
    public List<string> ReadFromFile()
    {
        var result = new List<string>();

        string line;
        try
        {
            StreamReader sr = new StreamReader("Day1/input.txt");

            line = sr.ReadLine();

            while(line != null)
            {
                var parsedLine = line;
                foreach(var elm in Replacements)
                {
                    parsedLine = parsedLine.Replace(elm.Key, elm.Value);
                }

                result.Add(parsedLine);
                line = sr.ReadLine();
            }

            sr.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        return result;
    }

    public int Execute()
    {
        var lines = ReadFromFile();

        var numbers = lines
            .Select(x => new string(x.Where(x => char.IsDigit(x)).ToArray()))
            .Select(x => x.Length >= 2 ? new string(x.First() + "" + x.Last()) : new string(x + "" + x))
            .Select(int.Parse)
            .ToList();

        return numbers.Sum();
    }

    private static readonly Dictionary<string, string> Replacements = new()
    {
        { "one", "o1e" },
        { "two", "t2o" },
        { "three", "t3e" },
        { "four", "f4r" },
        { "five", "f5e" },
        { "six", "s6x" },
        { "seven", "s7n" },
        { "eight", "e8t" },
        { "nine", "n9e" }
    };
}
