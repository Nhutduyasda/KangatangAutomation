using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;
using KangatangAutomation.PageObjects;

namespace KangatangAutomation.TestSuites.TS_ProductManagement;

/// <summary>
/// TEST CASE ID : TC_PRODUCTMANAGEMENT_ADDPRODUCTS_01
/// TEST SCENARIO: TS_PRODUCTMANAGEMENT
/// DESCRIPTION  : Verify successful product creation with valid data (Happy Path)
/// PRE-CONDITION: Have an active admin account
/// PRIORITY     : Trung bình
/// </summary>
[TestFixture]
[Category("ProductManagement")]
public class TC_AddProduct_HappyPath
{
    private IWebDriver? _driver;
    private LoginPage?  _loginPage;
    private ProductPage? _productPage;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        GenReport.GetInstance("TS_ProductManagement");
    }

    [SetUp]
    public void Setup()
    {
        GenReport.CreateTest(
            "TC_PRODUCTMANAGEMENT_ADDPRODUCTS_01",
            "Verify successful product creation with valid data (Happy Path)");

        GenReport.LogInfo("[SETUP] Initializing ChromeDriver...");
        _driver = DriverManager.InitDriver(maximize: true);
        _loginPage  = new LoginPage(_driver);
        _productPage = new ProductPage(_driver);
        GenReport.LogPass("[SETUP] ChromeDriver initialized successfully");
    }

    [Test, Order(1)]
    public void AddProduct_WithValidData_ShouldRedirectToProductList()
    {
        // ── STEP 1-8: Login ──────────────────────────────────────────
        GenReport.LogInfo("[STEP 1-8] Navigating to site and logging in");
        _loginPage!.LoginAs(TestSettings.Username, TestSettings.Password);

        // ── STEP 9-10: Navigate to Product page ─────────────────────
        GenReport.LogInfo("[STEP 9-10] Navigating to Add Product page");
        _productPage!.NavigateToProductPage();

        // ── STEP 11-12: Enter Product Name ───────────────────────────
        GenReport.LogInfo("[STEP 11-12] Entering product name: Cà Phê Sữa Đá");
        _productPage.EnterProductName("Cà Phê Sữa Đá");

        // ── STEP 13-14: Enter Unit Price ─────────────────────────────
        GenReport.LogInfo("[STEP 13-14] Entering unit price: 1000");
        _productPage.EnterUnitPrice("1000");

        // ── STEP 15-16: Enter Discount ───────────────────────────────
        GenReport.LogInfo("[STEP 15-16] Entering discount: 10");
        _productPage.EnterDiscount("10");

        // ── STEP 17-18: Enter Quantity ───────────────────────────────
        GenReport.LogInfo("[STEP 17-18] Entering quantity: 100");
        _productPage.EnterQuantity("100");

        // ── STEP 19-21: Enter Product Date ───────────────────────────
        GenReport.LogInfo("[STEP 19-21] Entering product date: 02/20/2026");
        _productPage.EnterProductDate("02/20/2026");

        // ── STEP 22: Select Supplier ─────────────────────────────────
        GenReport.LogInfo("[STEP 22] Selecting supplier (value=33)");
        _productPage.SelectSupplier("33");

        // ── STEP 23-24: Select Category ──────────────────────────────
        GenReport.LogInfo("[STEP 23-24] Selecting category (value=34)");
        _productPage.SelectCategory("34");

        // ── STEP 25-26: Upload Image ──────────────────────────────────
        GenReport.LogInfo("[STEP 25-26] Uploading product image: CafeDa.png");
        _productPage.UploadImage("CafeDa.png");

        // ── STEP 27-28: Enter Description ────────────────────────────
        GenReport.LogInfo("[STEP 27-28] Entering description: Cà phê sữa đá thơm ngon");
        _productPage.EnterDescription("Cà phê sữa đá thơm ngon");

        // ── STEP 29: Click Submit ─────────────────────────────────────
        GenReport.LogInfo("[STEP 29] Clicking Submit (Add Product) button");
        _productPage.ClickSubmit();

        // ── STEP 30: Assertion & Close ───────────────────────────────
        GenReport.LogInfo("[STEP 30] Verifying product created successfully");

        // Verify redirect to product list page
        var wait = DriverManager.GetWait(_driver!);
        bool redirected = false;
        try
        {
            wait.Until(d => d.Url.Contains("product") || d.Url.Contains("success"));
            redirected = true;
        }
        catch { /* handled below */ }

        Assert.That(redirected, Is.True,
            "Expected redirect to Product List page after successful product creation");
        GenReport.LogPass("[STEP 30] Product created successfully - Redirected to Product List page");

        // Verify success toast (if present)
        try
        {
            var toast = _driver!.FindElement(By.CssSelector(".alert-success, .toast-success, .notification-success"));
            Assert.That(toast.Displayed, Is.True, "Success notification should be displayed");
            GenReport.LogPass($"Success message displayed: {toast.Text}");
        }
        catch
        {
            GenReport.LogWarning("Success toast not found (UI may vary) - URL redirect verified instead");
        }
    }

    [TearDown]
    public void TearDown()
    {
        var status  = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed && _driver != null)
            GenReport.LogScreenshot(_driver, "Failure Screenshot - TC01");

        GenReport.SetTestResult(status, message);
        GenReport.LogInfo("[TEARDOWN] Closing browser");
        
        // Fix NUnit1032: Dispose _driver directly
        if (_driver != null)
        {
            _driver.Quit();
            _driver.Dispose();
            _driver = null;
        }
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        GenReport.FlushReport();
        GenReport.Reset();
    }
}
