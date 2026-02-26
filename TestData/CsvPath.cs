using KangatangAutomation.Config;

namespace KangatangAutomation.TestData;

public static class CsvPath
{
    // Convention: TestData/<SuiteName>/<TestCaseId>.csv
    public static string ForTestCase(string suiteName, string testCaseId)
    {
        var fileName = testCaseId + ".csv";
        return Path.Combine(TestSettings.TestDataFolder, suiteName, fileName);
    }
}
