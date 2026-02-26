using CsvHelper.Configuration;
using KangatangAutomation.TestData.Models;

namespace KangatangAutomation.TestData.Maps;

public sealed class CategoryTestDataMap : ClassMap<CategoryTestData>
{
    public CategoryTestDataMap()
    {
        Map(m => m.CategoryName).Name("categoryName");
    }
}
