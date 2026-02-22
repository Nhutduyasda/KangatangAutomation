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
    // Dựa vào ảnh, menu nằm bên trái có chữ "THÊM DANH MỤC" (có icon hình người/avatar).
    // Ta lấy theo text hoặc href vì cấu trúc menu có vẻ khác so với file xls ban đầu.
    private static readonly By DashboardMenu = By.XPath("//p[contains(text(), 'SẢN PHẨM & CUNG ỨNG') or contains(text(), 'Dashboard')]");
    
    // Nút "THÊM DANH MỤC" trên menu bên trái
    private static readonly By CategoriesMenu = By.XPath("//p[contains(text(), 'Thêm Danh Mục')] | //a[contains(@href, 'addcategory')]");
    
    // Trên form thêm danh mục:
    private static readonly By SupplierDropdown = By.Id("supplier.id"); 
    // Hoặc theo name="supplierId" / By.XPath("//select[contains(@name, 'supplier')]") nếu ID đổi
    
    private static readonly By CategoryNameInput = By.Id("name"); 
    // Hoặc By.Name("categoryName")

    // Nút "Add Category" màu xanh dương nhạt
    private static readonly By SaveButton = By.XPath("//button[@type='submit']");

    public CategoryPage(IWebDriver driver)
    {
        _driver = driver;
        _wait = DriverManager.GetWait(driver);
    }

    public CategoryPage ClickDashboardMenu()
    {
        try
        {
            _wait.Until(d => d.FindElement(DashboardMenu).Displayed);
            _driver.FindElement(DashboardMenu).Click();
            GenReport.LogPass("Dashboard menu clicked");
            Thread.Sleep(500); // Wait for transition
        }
        catch 
        {
            GenReport.LogWarning("Dashboard menu not found or not clickable, proceeding...");
        }
        return this;
    }

    public CategoryPage ClickCategoriesMenu()
    {
        _wait.Until(d => d.FindElement(CategoriesMenu).Displayed);
        _driver.FindElement(CategoriesMenu).Click();
        GenReport.LogPass("Menu 'THÊM DANH MỤC' clicked");
        Thread.Sleep(1000); // Wait for page to load completely
        return this;
    }

    // Không còn nút "Add New" nữa vì menu "THÊM DANH MỤC" chuyển trực tiếp vào form
    public CategoryPage ClickAddNew()
    {
        // Bỏ qua hành động này vì flow mới vào thẳng form
        return this;
    }

    public CategoryPage NavigateToCategoryPage()
    {
        // Có thể cần click menu cha nếu nó bị ẩn, nhưng tạm thời cứ click trực tiếp
        ClickCategoriesMenu();
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
        GenReport.LogPass("Add Category button clicked");
        Thread.Sleep(2000); // Wait for response
        return this;
    }

    // ===== Assertion Helpers =====
    public bool IsOnCategoryListPage()
    {
        try
        {
            _wait.Until(d => d.Url.Contains("categor")); // Adjust if URL differs upon success
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
