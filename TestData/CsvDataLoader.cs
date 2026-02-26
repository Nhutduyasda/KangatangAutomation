using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace KangatangAutomation.TestData;

public static class CsvDataLoader
{
    public static IEnumerable<T> Load<T, TMap>(string csvPath)
        where TMap : ClassMap<T>
    {
        if (!File.Exists(csvPath))
            throw new FileNotFoundException($"CSV test data not found: {csvPath}");

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            BadDataFound = null,
            MissingFieldFound = null,
            HeaderValidated = null,
            PrepareHeaderForMatch = args => args.Header.Trim(),
            TrimOptions = TrimOptions.Trim
        };

        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<TMap>();

        return csv.GetRecords<T>().ToList();
    }
}
