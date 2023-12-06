Console.WriteLine("*** AdventOfCode 2023 ***");
Console.WriteLine("-------------------------");
Console.WriteLine("* December 5th: *");
Console.WriteLine("   Part #1");
Console.WriteLine($"     Lowest location: {RunPart1()}");
Console.WriteLine("   Part #2");
Console.WriteLine($"     Lowest location: {RunPart2()}");
Console.WriteLine("-------------------------");

static long RunPart1()
{
    using (var reader = new StreamReader("input.txt"))
    {
        var seeds = GetSeeds(reader);
        var seedToSoilMap = GetMap(reader, "seed-to-soil map:");
        var soilToFertilizerMap = GetMap(reader, "soil-to-fertilizer map:");
        var fertilizerToWaterMap = GetMap(reader, "fertilizer-to-water map:");
        var waterToLightMap = GetMap(reader, "water-to-light map:");
        var lightToTemperatureMap = GetMap(reader, "light-to-temperature map:");
        var temperatureToHumidityMap = GetMap(reader, "temperature-to-humidity map:");
        var humidityToLocationMap = GetMap(reader, "humidity-to-location map:");

        long lowestLocation = long.MaxValue;

        foreach (var seed in seeds)
        {
            long currentSeed = seed;
            currentSeed = ProcessMap(currentSeed, seedToSoilMap);
            currentSeed = ProcessMap(currentSeed, soilToFertilizerMap);
            currentSeed = ProcessMap(currentSeed, fertilizerToWaterMap);
            currentSeed = ProcessMap(currentSeed, waterToLightMap);
            currentSeed = ProcessMap(currentSeed, lightToTemperatureMap);
            currentSeed = ProcessMap(currentSeed, temperatureToHumidityMap);
            currentSeed = ProcessMap(currentSeed, humidityToLocationMap);
            lowestLocation = Math.Min(lowestLocation, currentSeed);
        }
        
        return lowestLocation;
    }
}

static long RunPart2()
{
    using (var reader = new StreamReader("input.txt"))
    {
        var seedRanges = GetSeedRanges(reader);

        var seedToSoilMap = GetMap(reader, "seed-to-soil map:");
        var soilToFertilizerMap = GetMap(reader, "soil-to-fertilizer map:");
        var fertilizerToWaterMap = GetMap(reader, "fertilizer-to-water map:");
        var waterToLightMap = GetMap(reader, "water-to-light map:");
        var lightToTemperatureMap = GetMap(reader, "light-to-temperature map:");
        var temperatureToHumidityMap = GetMap(reader, "temperature-to-humidity map:");
        var humidityToLocationMap = GetMap(reader, "humidity-to-location map:");

        long lowestLocation = long.MaxValue;

        foreach (var seedRange in seedRanges)
        {
            for (long i = 0; i < (seedRange.Item1 + seedRange.Item2); i++)
            {
                long currentSeed = seedRange.Item1 + i;

                // Process seed-to-soil map
                currentSeed = ProcessMap(currentSeed, seedToSoilMap);

                // Process soil-to-fertilizer map
                currentSeed = ProcessMap(currentSeed, soilToFertilizerMap);

                // Process fertilizer-to-water map
                currentSeed = ProcessMap(currentSeed, fertilizerToWaterMap);

                // Process water-to-light map
                currentSeed = ProcessMap(currentSeed, waterToLightMap);

                // Process light-to-temperature map
                currentSeed = ProcessMap(currentSeed, lightToTemperatureMap);

                // Process temperature-to-humidity map
                currentSeed = ProcessMap(currentSeed, temperatureToHumidityMap);

                // Process humidity-to-location map
                currentSeed = ProcessMap(currentSeed, humidityToLocationMap);

                // Update lowest location
                lowestLocation = Math.Min(lowestLocation, currentSeed);
            }
        }

        return lowestLocation;
    }
}

static long ProcessMap(long seed, List<Tuple<long, long, long>> map)
{
    foreach (var mapEntry in map)
    {
        if (seed >= mapEntry.Item2 && seed < mapEntry.Item2 + mapEntry.Item3)
        {
            return mapEntry.Item1 + (seed - mapEntry.Item2);
        }
    }
    return seed;
}

static List<long> GetSeeds(StreamReader reader)
{
    var seedsLine = reader.ReadLine();
    if (seedsLine != null)
    {
        List<long> seeds = seedsLine
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s =>
            {
                bool parsed = long.TryParse(s, out long value);
                return parsed ? value : -1;
            })
            .Where(value => value >= 0)
            .ToList();

        return seeds;
    }
    else
    {
        return new List<long>();
    }
}

static List<Tuple<long, long>> GetSeedRanges(StreamReader reader)
{
    var seedsLine = reader.ReadLine();
    if (seedsLine != null)
    {
        List<Tuple<long, long>> seedRanges = new List<Tuple<long, long>>();

        var seedValues = seedsLine
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s =>
            {
                bool parsed = long.TryParse(s, out long value);
                return parsed ? value : -1;
            })
            .Where(value => value >= 0)
            .ToList();

        // Iterate through pairs and create ranges
        for (int i = 0; i < seedValues.Count; i += 2)
        {
            if (i + 1 < seedValues.Count)
            {
                seedRanges.Add(Tuple.Create(seedValues[i], seedValues[i + 1]));
            }
            else
            {
                return new List<Tuple<long, long>>();
            }
        }

        return seedRanges;
    }
    else
    {
        return new List<Tuple<long, long>>();
    }
}

static List<Tuple<long, long, long>> GetMap(StreamReader reader, string mapIdentifier)
{
    List<Tuple<long, long, long>> map = new List<Tuple<long, long, long>>();

    string line;
    while ((line = reader.ReadLine()) != null)
    {
        if (line.StartsWith(mapIdentifier))
        {
            break;
        }
    }

    while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line))
    {
        var parts = line.Split(' ');
        if (parts.Length == 3)
        {
            map.Add(Tuple.Create(long.Parse(parts[0]), long.Parse(parts[1]), long.Parse(parts[2])));
        }
    }

    return map;
}
