using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;

namespace KangatangAutomation.PageObjects;

public class CategoryPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    // ===== Locators =====
    private static readonly By DashboardMenu = By.CssSelector("li:nth-child(1) p:nth-child(2)");
    private static readonly By CategoriesMenu = By.CssSelector("li:nth-child(4) p");
    
    // Updated locator for Add New button to be more flexible (checking class, href, text)
    private static readonly By AddNewButton = By.CssSelector("a[href*='category/create'], .btn-add-category, a.btn-primary, button.btn-primary");
    private static readonly By AddNewButtonXPath = By.XPath("//a[contains(text(), 'Add') or contains(text(), 'Thêm') or contains(@class, 'add-category')]");
    
    private static readonly By SupplierDropdown = By.Id("supplierId");
    private static readonly By CategoryNameInput = By.Id("categoryName");
    private static readonly By SaveButton = By.CssSelector(".btn-save, button[type='submit']");

    public CategoryPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = DriverManager.GetWait(driver);
    }

    public CategoryPage ClickDashboardMenu()
    {
        _wait.Until(d => d.FindElement(DashboardMenu).Displayed);
        _driver.FindElement(DashboardMenu).Click();
        GenReport.LogPass("Dashboard menu clicked");
        Thread.Sleep(500); // Wait for transition
        return this;
    }

    public CategoryPage ClickCategoriesMenu()
    {
        _wait.Until(d => d.FindElement(CategoriesMenu).Displayed);
        _driver.FindElement(CategoriesMenu).Click();
        GenReport.LogPass("Categories menu opened");
        Thread.Sleep(1000); // Wait for table/page to load completely
        return this;
    }

    public CategoryPage ClickAddNew()
    {
        // Try multiple locators just in case the UI is different from the excel file
        IWebElement? button = null;
        try
        {
            button = _wait.Until(d => d.FindElement(AddNewButton));
        }
        catch
        {
            try 
            {
                button = _driver.FindElement(AddNewButtonXPath);
            }
            catch (Exception ex)
            {
                GenReport.LogFail($"Could not find Add New button. Error: {ex.Message}");
                throw;
            }
        }

        if (button != null)
        {
            button.Click();
            GenReport.LogPass("Add New button clicked");
            Thread.Sleep(1000); // Wait for the form to appear
        }
        return this;
    }

    public CategoryPage NavigateToCategoryPage()
    {
        ClickDashboardMenu();
        ClickCategoriesMenu();
        ClickAddNew();
        _wait.Until(d => d.FindElement(CategoryNameInput).Displayed);
        GenReport.LogPass("Navigated to Add Category page");
        return this;
    }

    public CategoryPage SelectSupplier(string value)
    {
        var dropdown = _driver.FindElement(SupplierDropdown);
        dropdown.Click();
        Thread.Sleep(300);
        var option = dropdown.FindElement(By.CssSelector($"option[value='{value}']"));
        option.Click();
        Thread.Sleep(300);
        GenReport.LogPass($"Supplier selected: value={value}");
        return this;
    }

    public CategoryPage SelectDefaultSupplier()
    {
        var dropdown = _driver.FindElement(SupplierDropdown);
        dropdown.Click();
        var options = dropdown.FindElements(By.TagName("option"));
        if (options.Count > 0)
            options[0].Click();
        GenReport.LogPass("Default supplier option selected (no supplier)");
        return this;
    }

    public CategoryPage EnterCategoryName(string name)
    {
        var input = _driver.FindElement(CategoryNameInput);
        input.Click();
        input.Clear();
        input.SendKeys(name);
        GenReport.LogPass($"Category name entered: {name}");
        return this;
    }

    public CategoryPage ClearCategoryName()
    {
        var input = _driver.FindElement(CategoryNameInput);
        input.Click();
        input.Clear();
        input.SendKeys(Keys.Tab);
        GenReport.LogPass("Category name field cleared (empty)");
        return this;
    }

    public CategoryPage ClickSave()
    {
        _driver.FindElement(SaveButton).Click();
        GenReport.LogPass("Save button clicked");
        Thread.Sleep(2000); // Wait for response
        return this;
    }

    // ===== Assertion Helpers =====
    public bool IsOnCategoryListPage()
    {
        try
        {
            _wait.Until(d => d.Url.Contains("categor"));
            return true;
        }
        catch { return false; }
    }

    public string GetValidationError()
    {
        try
        {
            var errorElement = _driver.FindElement(
                By.CssSelector(".text-danger, .field-validation-error, .error-message"));
            return errorElement.Text;
        }
        catch { return string.Empty; }
    }

    public bool IsStillOnAddPage()
    {
        try
        {
            return _driver.FindElement(CategoryNameInput).Displayed;
        }
        catch { return false; }
    }
}