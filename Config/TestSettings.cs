namespace KangatangAutomation.Config;

public static class TestSettings
{
    // ===== URL =====
    public const string BaseUrl = "https://kt1.hksolution.io.vn/";

    // ===== Credentials =====
    public const string Username = "nhutduy051";
    public const string Password = "Nhutduy0501@";

    // ===== Browser =====
    public const string BrowserVersion = "131";

    // ===== Timeouts =====
    public const int DefaultTimeoutSeconds = 10;
    public const int ShortWaitMs = 500;
    public const int MediumWaitMs = 1000;
    public const int LongWaitMs = 2000;

    // ===== Test Data Paths =====
    public static string TestDataFolder => Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "TestData");

    public static string GetTestDataFile(string fileName) =>
        Path.Combine(TestDataFolder, fileName);

    // ===== Report =====
    public static string ReportFolder => Path.GetFullPath(
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "report"));
}
