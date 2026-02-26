using CsvHelper.Configuration;
using KangatangAutomation.TestData.Models;

namespace KangatangAutomation.TestData.Maps;

public sealed class ProductTestDataMap : ClassMap<ProductTestData>
{
    public ProductTestDataMap()
    {
        Map(m => m.ProductName).Name("productName");
        Map(m => m.UnitPrice).Name("unitPrice");
        Map(m => m.Discount).Name("discount");
        Map(m => m.Quantity).Name("quantity");
        Map(m => m.ProductDate).Name("productDate");
        Map(m => m.SupplierValue).Name("supplierValue");
        Map(m => m.CategoryValue).Name("categoryValue");
        Map(m => m.ImageFile).Name("imageFile");
        Map(m => m.Description).Name("description");
    }
}
