using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdventOfCode.Day6;

public record Race (int time, int distance);

public record LongRace (long time, long distance);

public class Solution
{
    private List<int> TransformToNumbers(string line)
    {
        var delimiter = line.IndexOf(":");
        var numbers = line.Substring(delimiter + 1);

        var numbers1 = numbers
            .Trim()
            .Split(" ")
            .Where(x => x != "")
            .Select(int.Parse)
            .ToList();

        return numbers1;
    }

    public List<Race> ReadFromFile()
    {
        var result = new List<Race>();

        string lineTime;
        string lineDistance;
        try
        {
            StreamReader sr = new StreamReader("Day6/input1.txt");

            lineTime = sr.ReadLine();
            lineDistance = sr.ReadLine();
                
            var time1 = TransformToNumbers(lineTime);
            var distance2 = TransformToNumbers(lineDistance);

            foreach (var number in time1.Select((x, i) => (x, i)))
            {
                result.Add(new Race(number.x, distance2[number.i]));
            }

            sr.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        return result;
    }

    public LongRace ReadFromFile2 ()
    {
        StreamReader sr = new StreamReader("Day6/input1.txt");

        var lineTime = sr.ReadLine();
        var lineDistance = sr.ReadLine();

        var time = long.Parse(new string(lineTime.Where(char.IsDigit).ToArray()));
        var distance = long.Parse(new string(lineDistance.Where(char.IsDigit).ToArray()));

        return new LongRace(time, distance);
    }

    public int Execute(int number)
    {

        if (number == 1)
        {

            var races = ReadFromFile();

            var results1 = races.Select(race =>
            {
                return Enumerable
                    .Range(0, race.time)
                    .Select(x => (race.time - x) * x)
                    .Where(x => x > race.distance)
                    .ToList();

            })
            .Select(x => x.Count)
            .Aggregate(1, (cur, nxt) => cur * nxt);

            return results1;
        } else
        {
            var race = ReadFromFile2();

            var count = 0;
            for (var i = 0; i < race.time; i++)
            {
                if (i * (race.time - i) > race.distance)
                {
                    count++;
                }
            }


            /* 
             * Better :
                traveled distance = (time - button) * button
                traveled distance = time * button - button * button
                0 = button * button - time * button - traveled distance

                => kwadratische formule 

                button = time +/- sqrt(time * time - 4 * (button + 1))/2
             
             */

            var button1 = race.time - Math.Ceiling(Math.Sqrt(race.time * race.time - 4 * (race.distance + 1)) / 2);
            var button2 = race.time + Math.Floor(Math.Sqrt(race.time * race.time - 4 * (race.distance + 1)) / 2);

            var count2 = button2 - button1;


            return count;
        }
        
    }
}
