using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.Extensions;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;

namespace KangatangAutomation.PageObjects;

public class ProductPage
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    // ===== Locators =====
    private static readonly By DashboardMenu    = By.CssSelector("li:nth-child(1) p:nth-child(2)");
    private static readonly By ProductsMenu     = By.CssSelector("li:nth-child(5) p");
    private static readonly By NameInput        = By.Id("name");
    private static readonly By UnitPriceInput   = By.Id("unitPrice");
    private static readonly By DiscountInput    = By.Id("discount");
    private static readonly By QuantityInput    = By.Id("quantity");
    private static readonly By ProductDateInput = By.Id("productDate");
    private static readonly By SupplierDropdown = By.Id("supplier.id");
    private static readonly By CategoryDropdown = By.Id("category.id");
    private static readonly By FileUpload       = By.Id("files");
    private static readonly By DescriptionInput = By.Id("description");
    private static readonly By SubmitButton     = By.CssSelector(".btn-info");

    public ProductPage(IWebDriver driver)
    {
        _driver = driver;
        _wait   = DriverManager.GetWait(driver);
    }

    public ProductPage ClickDashboardMenu()
    {
        _wait.Until(d => d.FindElement(DashboardMenu).Displayed);
        _driver.FindElement(DashboardMenu).Click();
        GenReport.LogPass("Dashboard menu clicked");
        return this;
    }

    public ProductPage ClickProductsMenu()
    {
        _wait.Until(d => d.FindElement(ProductsMenu).Displayed);
        _driver.FindElement(ProductsMenu).Click();
        GenReport.LogPass("Products menu opened");
        return this;
    }

    public ProductPage NavigateToProductPage()
    {
        ClickDashboardMenu();
        ClickProductsMenu();
        _wait.Until(d => d.FindElement(NameInput).Displayed);
        GenReport.LogPass("Navigated to Add Product page");
        return this;
    }

    public ProductPage EnterProductName(string name)
    {
        var input = _driver.FindElement(NameInput);
        input.Click();
        input.Clear();
        input.SendKeys(name);
        GenReport.LogPass($"Product name entered: {name}");
        return this;
    }

    public ProductPage EnterUnitPrice(string price)
    {
        var input = _driver.FindElement(UnitPriceInput);
        input.Click();
        input.Clear();
        input.SendKeys(price);
        GenReport.LogPass($"Unit price entered: {price}");
        return this;
    }

    public ProductPage EnterDiscount(string discount)
    {
        var input = _driver.FindElement(DiscountInput);
        input.Click();
        input.Clear();
        input.SendKeys(discount);
        GenReport.LogPass($"Discount entered: {discount}");
        return this;
    }

    public ProductPage EnterQuantity(string quantity)
    {
        var input = _driver.FindElement(QuantityInput);
        input.Click();
        input.Clear();
        input.SendKeys(quantity);
        GenReport.LogPass($"Quantity entered: {quantity}");
        return this;
    }

    public ProductPage EnterProductDate(string date)
    {
        var input = _driver.FindElement(ProductDateInput);
        input.Click();
        input.Clear();
        input.Click();
        input.SendKeys(date);
        GenReport.LogPass($"Product date entered: {date}");
        return this;
    }

    public ProductPage SelectSupplier(string value)
    {
        // Chờ dropdown Supplier có ít nhất 2 option (tức là đã load xong dữ liệu)
        _wait.Until(d =>
        {
            var el = d.FindElement(SupplierDropdown);
            return el.FindElements(By.TagName("option")).Count > 1;
        });

        var select = new SelectElement(_driver.FindElement(SupplierDropdown));
        select.SelectByValue(value);
        GenReport.LogPass($"Supplier selected: value={value}");
        return this;
    }

    public ProductPage SelectCategory(string value)
    {
        // Chờ dropdown Category có ít nhất 2 option
        // (vì Category thường load động sau khi Supplier được chọn)
        _wait.Until(d =>
        {
            var el = d.FindElement(CategoryDropdown);
            return el.FindElements(By.TagName("option")).Count > 1;
        });

        // Chờ thêm cho option cụ thể xuất hiện
        _wait.Until(d =>
            d.FindElement(CategoryDropdown)
             .FindElements(By.CssSelector($"option[value='{value}']")).Count > 0
        );

        var select = new SelectElement(_driver.FindElement(CategoryDropdown));
        select.SelectByValue(value);
        GenReport.LogPass($"Category selected: value={value}");
        return this;
    }

    public ProductPage UploadImage(string fileName)
    {
        var filePath = TestSettings.GetTestDataFile(fileName);
        if (File.Exists(filePath))
        {
            _driver.FindElement(FileUpload).SendKeys(filePath);
            GenReport.LogPass($"Image uploaded: {fileName}");
        }
        else
        {
            GenReport.LogWarning($"Image file not found: {filePath}");
        }
        return this;
    }

    public ProductPage EnterDescription(string description)
    {
        var input = _driver.FindElement(DescriptionInput);
        input.Click();
        input.Clear();
        input.SendKeys(description);
        GenReport.LogPass($"Description entered: {description}");
        return this;
    }

    public ProductPage ClickSubmit()
    {
        _driver.FindElement(SubmitButton).Click();
        GenReport.LogPass("Submit button clicked");
        return this;
    }

    /// <summary>
    /// Fill all product fields and submit
    /// </summary>
    public ProductPage FillAndSubmitProduct(
        string name, string unitPrice, string discount,
        string quantity, string date, string supplierValue,
        string categoryValue, string imageFile, string description)
    {
        EnterProductName(name);
        EnterUnitPrice(unitPrice);
        EnterDiscount(discount);
        EnterQuantity(quantity);
        EnterProductDate(date);
        SelectSupplier(supplierValue);
        SelectCategory(categoryValue);
        UploadImage(imageFile);
        EnterDescription(description);
        ClickSubmit();
        return this;
    }

    // ===== Assertion Helpers =====
    public bool IsOnProductListPage()
    {
        try
        {
            _wait.Until(d => d.Url.Contains("product"));
            return true;
        }
        catch { return false; }
    }

    public string GetValidationError(string fieldId)
    {
        try
        {
            var errorElement = _driver.FindElement(
                By.CssSelector($"#{fieldId} ~ .text-danger, #{fieldId} + .text-danger, .field-validation-error"));
            return errorElement.Text;
        }
        catch { return string.Empty; }
    }

    public bool IsStillOnCreatePage()
    {
        try
        {
            return _driver.FindElement(NameInput).Displayed;
        }
        catch { return false; }
    }
}
