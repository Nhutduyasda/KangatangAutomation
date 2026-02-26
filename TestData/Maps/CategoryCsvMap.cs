using CsvHelper.Configuration;
using KangatangAutomation.TestData.Models;

namespace KangatangAutomation.TestData.Maps;

public sealed class CategoryCsvMap : ClassMap<CategoryCsv>
{
    public CategoryCsvMap()
    {
        Map(m => m.CategoryName).Name("categoryName");
    }
}
