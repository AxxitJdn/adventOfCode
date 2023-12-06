namespace AdventOfCode.Day5;

public record struct Seed ( long id, long range );

public record struct Mapper (long destination, long mapping, long range);

public class Solution
{
    public (List<Seed>, Dictionary<string, List<Mapper>>) ReadFromFile()
    {

        var seeds = new List<Seed>();

        var mappings = new Dictionary<string, List<Mapper>>();

        string line;
        try
        {
            StreamReader sr = new StreamReader("Day5/input1.txt");

            line = sr.ReadLine();

            if (line != null)
            {
                var delimiter = line.IndexOf(":");
                var parsedLine = line
                    .Substring(delimiter + 1)
                    .Trim()
                    .Split(" ")
                    .Select(long.Parse)
                    .ToList();

                for(int i = 0; i < parsedLine.Count; i = i+2) 
                {
                    seeds.Add(new Seed(parsedLine[i], parsedLine[i+1]));
                }

                line = sr.ReadLine();
            }

            while (line != null)
            {
                if (line == "")
                {
                    line = sr.ReadLine();
                    continue;
                }

                var index = line.IndexOf(":");
                var key = "";

                if (index != -1)
                {
                    var nameIndex = line.IndexOf(" ");
                    key = line.Substring(0, nameIndex);
                }

                var mappingLines = sr.ReadLine();

                var mappingForType = new List<Mapper>();

                while(mappingLines != "" && mappingLines != null) 
                {
                    var mapping = mappingLines
                        .Trim()
                        .Split(" ")
                        .Where(x => x != "")
                        .Select(long.Parse)
                        .ToList();

                    mappingForType.Add(new Mapper(mapping[0], mapping[1], mapping[2]));

                    mappingLines = sr.ReadLine();
                }

                mappings.Add(key, mappingForType);

                line = sr.ReadLine();
            }

            sr.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }

        return (seeds, mappings);
    }

    private long GetNextValue(bool found, List<Mapper> mapper, long index)
    {
        if (found)
        {
            var result = mapper
                .Where(x => x.mapping <= index && index < x.mapping + x.range)
                .Select(x => x.destination - x.mapping + index )
                .ToList();


            if (result.Count == 0)
            {
                return index;
            }
            else
            {
                return result[0];
            }
        }

        return 0;
    }

    private IEnumerable<long> GetSeedEnumerator(List<Seed> seeds)
    {
        foreach (var seed in seeds)
        {
            long seedIndex = -1;
            while(seedIndex < seed.range - 1)
            {
                seedIndex += 1;
                yield return seed.id + seedIndex;
            }
        }
    }

    public long Execute()
    {
        var (seeds, mappings) = ReadFromFile();

        var lowestLocation = long.MaxValue;

        foreach(var seed in GetSeedEnumerator(seeds))
        {
            var foundSeedToSoil = mappings.TryGetValue("seed-to-soil", out var map1);
            var soil = GetNextValue(foundSeedToSoil, map1, seed);

            var foundSoilToFertilizer = mappings.TryGetValue("soil-to-fertilizer", out var map2);
            var fertilizer = GetNextValue(foundSoilToFertilizer, map2, soil);

            var foundFertilizerToWater = mappings.TryGetValue("fertilizer-to-water", out var map3);
            var water = GetNextValue(foundFertilizerToWater, map3, fertilizer);

            var foundWaterToLight = mappings.TryGetValue("water-to-light", out var map4);
            var light = GetNextValue(foundWaterToLight, map4, water);

            var foundLightToTemperature = mappings.TryGetValue("light-to-temperature", out var map5);
            var temperature = GetNextValue(foundLightToTemperature, map5, light);

            var foundTemperatureToHumidity = mappings.TryGetValue("temperature-to-humidity", out var map6);
            var humidity = GetNextValue(foundTemperatureToHumidity, map6, temperature);

            var foundHunidityToLocation = mappings.TryGetValue("humidity-to-location", out var map7);
            var location = GetNextValue(foundHunidityToLocation, map7, humidity);

            if (location < lowestLocation)
            {
                lowestLocation = location;
            }

        }


        return lowestLocation;
    }
}
