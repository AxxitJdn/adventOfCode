using AdventOfCode.Day2;

namespace AdventOfCode.Day4;

public record Card (int id, List<int> winning, List<int> numbers);

public class Solution
{
    public List<Card> ReadFromFile()
    {
        var result = new List<Card>();

        string line;
        try
        {
            StreamReader sr = new StreamReader("Day4/input1.txt");

            line = sr.ReadLine();

            while (line != null)
            {
                var gameDelimiter = line.IndexOf(":");
                var header = line.Substring(0, gameDelimiter);
                var body = line.Substring(gameDelimiter + 1);

                var gameId = int.Parse(new string(header.Where(char.IsDigit).ToArray()));


                var numberDelimiter = body.IndexOf("|");

                var winningNumbers = body.Substring(0, numberDelimiter);
                var ownNumbers = body.Substring(numberDelimiter + 1);

                var winningNumbersAsList = winningNumbers
                    .Trim()
                    .Split(" ")
                    .Select(x => x.Trim())
                    .Where(x => x != "")
                    .ToList();

                var ownNumbersAsList = ownNumbers
                    .Trim()
                    .Split(" ")
                    .Select(x => x.Trim())
                    .Where(x => x != "")
                    .ToList();

                var card = new Card(
                    gameId, 
                    winningNumbersAsList.Select(int.Parse).ToList(), 
                    ownNumbersAsList.Select(int.Parse).ToList()
                );
                
                result.Add(card);

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

    private int Calculate(int total)
    {
        if (total == 0)
        {
            return 0;
        }

        int result = 1;
        int idx = total - 1;

        while(idx > 0)
        {
            result = result * 2;
            idx--;
        }

        return result;
    }

    public int Execute()
    {
        var cards = ReadFromFile();

        var resultOne = cards.Aggregate(0, (current, card) =>
        {
            var total = card.numbers
                .Count(x => card.winning.Contains(x));

            return current + Calculate(total);
        });

        var resultTwo = Enumerable.Repeat(1, cards.Count).ToArray();

        foreach (var card in cards.Select((x,i) => (x, i)))
        {
            var total = card.x.numbers
                .Count(card.x.winning.Contains);

            var current = resultTwo[card.i];

            var startIdx = card.i + 1;
            var end = card.i + total;
            var endIdx = end < cards.Count ? end : cards.Count - 1;
            var indexes = Enumerable.Range(startIdx, endIdx-startIdx+1).ToArray();


            foreach (var idx in indexes)
            {
                resultTwo[idx] = resultTwo[idx] + current;
            }
        }

        return resultTwo.Sum();
    }
}
