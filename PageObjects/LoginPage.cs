using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;

namespace KangatangAutomation.PageObjects;

public class LoginPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    // ===== Locators =====
    private static readonly By MenuList = By.CssSelector(".agile-login > ul");
    private static readonly By LoginLink = By.LinkText("Login");
    private static readonly By IdInput = By.Name("id");
    private static readonly By PasswordInput = By.Name("password");
    private static readonly By LoginButton = By.CssSelector(".login-form-grids input:nth-child(5)");
    private static readonly By DashboardElement = By.CssSelector("div[class='row'] div:nth-child(1) div:nth-child(1) div:nth-child(1)");

    public LoginPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = DriverManager.GetWait(driver);
    }

    public LoginPage NavigateToSite()
    {
        _driver.Navigate().GoToUrl(TestSettings.BaseUrl);
        GenReport.LogPass("Navigated to: " + TestSettings.BaseUrl);
        return this;
    }

    public LoginPage ClickMenuList()
    {
        _wait.Until(d => d.FindElement(MenuList).Displayed);
        _driver.FindElement(MenuList).Click();
        GenReport.LogPass("Menu list clicked");
        return this;
    }

    public LoginPage ClickLoginLink()
    {
        _wait.Until(d => d.FindElement(LoginLink).Displayed);
        _driver.FindElement(LoginLink).Click();
        GenReport.LogPass("Login link clicked");
        return this;
    }

    public LoginPage EnterUsername(string username)
    {
        var input = _wait.Until(d => d.FindElement(IdInput));
        input.Click();
        input.Clear();
        input.SendKeys(username);
        GenReport.LogPass($"Username entered: {username}");
        return this;
    }

    public LoginPage EnterPassword(string password)
    {
        var input = _driver.FindElement(PasswordInput);
        input.Click();
        input.Clear();
        input.SendKeys(password);
        GenReport.LogPass("Password entered");
        return this;
    }

    public LoginPage ClickLoginButton()
    {
        _driver.FindElement(LoginButton).Click();
        GenReport.LogPass("Login button clicked");
        return this;
    }

    public LoginPage VerifyDashboardDisplayed()
    {
        _wait.Until(d => d.FindElement(DashboardElement).Displayed);
        GenReport.LogPass("Dashboard is displayed - Login successful");
        return this;
    }

    /// <summary>
    /// Combo method: Full login flow
    /// </summary>
    public LoginPage LoginAs(string username, string password)
    {
        GenReport.LogInfo($"Performing login as: {username}");
        NavigateToSite();
        ClickMenuList();
        ClickLoginLink();
        EnterUsername(username);
        EnterPassword(password);
        ClickLoginButton();
        VerifyDashboardDisplayed();
        GenReport.LogPass($"Successfully logged in as: {username}");
        return this;
    }
}
