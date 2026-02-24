using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;

namespace KangatangAutomation.Helpers;

public static class DriverManager
{
    public static IWebDriver InitDriver(bool maximize = true)
    {
        var options = new ChromeOptions();
        options.BrowserVersion = TestSettings.BrowserVersion;
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");

        var driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TestSettings.DefaultTimeoutSeconds);

        if (maximize)
            driver.Manage().Window.Maximize();

        return driver;
    }

    public static IWebDriver InitDriverWithSize(int width, int height)
    {
        var options = new ChromeOptions();
        options.BrowserVersion = TestSettings.BrowserVersion;
        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-popup-blocking");

        var driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TestSettings.DefaultTimeoutSeconds);
        driver.Manage().Window.Size = new System.Drawing.Size(width, height);

        return driver;
    }

    public static WebDriverWait GetWait(IWebDriver driver, int? timeoutSeconds = null)
    {
        var timeout = timeoutSeconds ?? TestSettings.DefaultTimeoutSeconds;
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));

        // Make waits more resilient in real-world UI tests
        wait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(UnhandledAlertException));

        return wait;
    }

    /// <summary>
    /// Accept any unexpected JS alert if present (ex: alert text 'XSS').
    /// Returns true if an alert was found and accepted.
    /// </summary>
    public static bool TryAcceptAnyAlert(IWebDriver driver, int timeoutMs = 800)
    {
        var end = DateTime.UtcNow.AddMilliseconds(timeoutMs);
        while (DateTime.UtcNow < end)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                alert.Accept();
                return true;
            }
            catch (NoAlertPresentException)
            {
                Thread.Sleep(80);
            }
            catch (UnhandledAlertException)
            {
                try
                {
                    driver.SwitchTo().Alert().Accept();
                    return true;
                }
                catch
                {
                    Thread.Sleep(80);
                }
            }
        }

        return false;
    }

    public static void QuitDriver(IWebDriver? driver)
    {
        driver?.Quit();
        driver?.Dispose();
    }
}
