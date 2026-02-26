using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace KangatangAutomation.Helpers;

public static class CsvDataLoader
{
    public static IEnumerable<T> Load<T, TMap>(string relativePath)
        where TMap : ClassMap<T>
    {
        var fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException(
                $"CSV test data file not found: '{relativePath}'. Expected at: '{fullPath}'. " +
                "Ensure the file exists under TestData and is copied to output.");

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            IgnoreBlankLines = true,
            BadDataFound = null,
            MissingFieldFound = null,
            TrimOptions = TrimOptions.Trim,
        };

        using var reader = new StreamReader(fullPath);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<TMap>();

        return csv.GetRecords<T>().ToList();
    }
}
