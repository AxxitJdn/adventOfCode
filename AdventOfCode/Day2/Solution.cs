namespace AdventOfCode.Day2
{
    public record Game (int id, List<Hand> hands);

    public record Hand (List<Set>  sets);

    public record Set (Color color, int occurances);

    public enum Color { Blue, Red, Green }

    public record struct Total (int red, int green, int blue);

    public class Solution
    {
        public List<Game> ReadFromFile()
        {
            var result = new List<Game>();

            string line;
            try
            {
                StreamReader sr = new StreamReader("Day2/input1.txt");

                line = sr.ReadLine();

                while (line != null)
                {
                    var gameDelimiter = line.IndexOf(":");
                    var header = line.Substring(0, gameDelimiter);
                    var body = line.Substring(gameDelimiter + 1);

                    var gameId = int.Parse(new string(header.Where(char.IsDigit).ToArray()));

                    var hands = body.Split(";").ToList();

                    var handsCollection = new List<Hand>();

                    foreach(var hand in hands)
                    {
                        var sets = hand.Split(",");

                        var setCollection = new List<Set>();

                        foreach(var set in sets)
                        {
                            var s = set.Split(" ").Where(x => x != "").ToList();
                            var number = int.Parse(s[0]);
                            Replacements.TryGetValue(s[1], out var color);

                            setCollection.Add(new Set(color, number));
                        }

                        handsCollection.Add(new Hand(setCollection));
                    }

                    result.Add(new Game(gameId, handsCollection));

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
            var total = new Total(12, 13, 14);
            var games = ReadFromFile();

            var compareSet = (Set x) => x.color switch
            {
                Color.Red => x.occurances <= total.red,
                Color.Green => x.occurances <= total.green,
                Color.Blue => x.occurances <= total.blue,
            };

            var possibleGames = games
                .Where(x => x.hands.All(y => y.sets.All(compareSet)))
                .Select(x => x.id)
                .ToList();



            var maxGames = games
                .Select(x => {
                    var currentMax = new Total(1, 1, 1);
                    var sets = x.hands.SelectMany(y => y.sets);
                    foreach(var set in sets)
                    {
                        switch(set.color)
                        {
                            case Color.Red: currentMax.red = set.occurances > currentMax.red ? set.occurances : currentMax.red; break;
                            case Color.Green: currentMax.green = set.occurances > currentMax.green ? set.occurances : currentMax.green; break;
                            case Color.Blue: currentMax.blue = set.occurances > currentMax.blue ? set.occurances : currentMax.blue; break;
                        }
                    }
                    return currentMax;
                })
                .Select(x => x.red * x.blue * x.green)
                .ToList();


            return maxGames.Sum();
        }

        private static readonly Dictionary<string, Color> Replacements = new()
        {
            { "blue", Color.Blue },
            { "red", Color.Red },
            { "green", Color.Green }
        };
    }

    
}
