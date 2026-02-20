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
    private static readonly By AddNewButton = By.CssSelector(".btn-add-category");
    private static readonly By SupplierDropdown = By.Id("supplierId");
    private static readonly By CategoryNameInput = By.Id("categoryName");
    private static readonly By SaveButton = By.CssSelector(".btn-save");

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
        return this;
    }

    public CategoryPage ClickCategoriesMenu()
    {
        _wait.Until(d => d.FindElement(CategoriesMenu).Displayed);
        _driver.FindElement(CategoriesMenu).Click();
        GenReport.LogPass("Categories menu opened");
        return this;
    }

    public CategoryPage ClickAddNew()
    {
        _wait.Until(d => d.FindElement(AddNewButton).Displayed);
        _driver.FindElement(AddNewButton).Click();
        GenReport.LogPass("Add New button clicked");
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
        var option = dropdown.FindElement(By.CssSelector($"option[value='{value}']"));
        option.Click();
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
