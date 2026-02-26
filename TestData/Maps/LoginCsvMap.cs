using CsvHelper.Configuration;
using KangatangAutomation.TestData.Models;

namespace KangatangAutomation.TestData.Maps;

public sealed class LoginCsvMap : ClassMap<LoginCsv>
{
    public LoginCsvMap()
    {
        Map(m => m.Username).Name("username");
        Map(m => m.Password).Name("password");
    }
}
