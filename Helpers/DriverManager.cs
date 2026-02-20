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
        return new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
    }

    public static void QuitDriver(IWebDriver? driver)
    {
        driver?.Quit();
        driver?.Dispose();
    }
}
