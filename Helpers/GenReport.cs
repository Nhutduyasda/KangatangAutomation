using System.Threading;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using KangatangAutomation.Config;

namespace KangatangAutomation.Helpers;

public static class GenReport
{
    private static ExtentReports? _extent;

    // Per-thread test context to prevent overwriting _currentTest when tests run in parallel.
    private static readonly AsyncLocal<ExtentTest?> _currentTest = new();

    private static readonly object _lock = new object();
    private static string? _currentSuiteName;

    public static ExtentReports GetInstance(string suiteName = "TestSuite")
    {
        lock (_lock)
        {
            if (_extent == null || _currentSuiteName != suiteName)
            {
                _currentSuiteName = suiteName;
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var fileName = $"{suiteName}_{timestamp}.html";

                var reportDir = TestSettings.ReportFolder;
                if (!Directory.Exists(reportDir))
                    Directory.CreateDirectory(reportDir);

                var reportPath = Path.Combine(reportDir, fileName);

                var reporter = new ExtentSparkReporter(reportPath);
                reporter.Config.DocumentTitle = $"{suiteName} - Test Report";
                reporter.Config.ReportName = $"{suiteName} Report - {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
                reporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Standard;
                reporter.Config.TimelineEnabled = true;

                _extent = new ExtentReports();
                _extent.AttachReporter(reporter);
                _extent.AddSystemInfo("OS", Environment.OSVersion.ToString());
                _extent.AddSystemInfo("Machine", Environment.MachineName);
                _extent.AddSystemInfo("User", Environment.UserName);
                _extent.AddSystemInfo("Test Suite", suiteName);
                _extent.AddSystemInfo("Base URL", TestSettings.BaseUrl);
                _extent.AddSystemInfo("Execution Time", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            }

            return _extent;
        }
    }

    public static ExtentTest CreateTest(string testName, string description = "")
    {
        // Safety: ensure extent exists even if GetInstance wasn't called.
        if (_extent == null)
            GetInstance(_currentSuiteName ?? "TestSuite");

        var t = _extent!.CreateTest(testName, description);
        t.Info($"Test started at: {DateTime.Now:HH:mm:ss}");
        _currentTest.Value = t;
        return t;
    }

    private static ExtentTest? CurrentTest => _currentTest.Value;

    public static void LogStep(string stepDescription, Status status = Status.Info)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
        CurrentTest?.Log(status, $"[{timestamp}] {stepDescription}");
    }

    public static void LogPass(string message) => LogStep(message, Status.Pass);
    public static void LogFail(string message) => LogStep(message, Status.Fail);
    public static void LogInfo(string message) => LogStep(message, Status.Info);
    public static void LogWarning(string message) => LogStep(message, Status.Warning);

    public static void LogScreenshot(OpenQA.Selenium.IWebDriver driver, string title = "Screenshot")
    {
        try
        {
            var screenshot = ((OpenQA.Selenium.ITakesScreenshot)driver).GetScreenshot();
            var base64 = screenshot.AsBase64EncodedString;
            CurrentTest?.Info(title, MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64).Build());
        }
        catch (Exception ex)
        {
            CurrentTest?.Warning($"Could not capture screenshot: {ex.Message}");
        }
    }

    public static void SetTestResult(NUnit.Framework.Interfaces.TestStatus status, string? message = null)
    {
        var endTime = DateTime.Now.ToString("HH:mm:ss");
        switch (status)
        {
            case NUnit.Framework.Interfaces.TestStatus.Passed:
                CurrentTest?.Pass($"Test completed successfully at: {endTime}");
                break;
            case NUnit.Framework.Interfaces.TestStatus.Failed:
                CurrentTest?.Fail($"Test failed at: {endTime}. Error: {message}");
                break;
            case NUnit.Framework.Interfaces.TestStatus.Skipped:
                CurrentTest?.Skip($"Test skipped at: {endTime}. Reason: {message}");
                break;
            default:
                CurrentTest?.Warning($"Test ended with unknown status at: {endTime}");
                break;
        }
    }

    public static void FlushReport() => _extent?.Flush();

    public static void Reset()
    {
        _extent = null;
        _currentTest.Value = null;
        _currentSuiteName = null;
    }
}
