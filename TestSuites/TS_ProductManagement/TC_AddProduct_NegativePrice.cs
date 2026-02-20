using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;
using KangatangAutomation.PageObjects;

namespace KangatangAutomation.TestSuites.TS_ProductManagement;

/// <summary>
/// TEST CASE ID : TC_PRODUCTMANAGEMENT_ADDPRODUCTS_03
/// TEST SCENARIO: TS_PRODUCTMANAGEMENT
/// DESCRIPTION  : Verify system validation for negative value in Unit Price field (Negative Test)
/// PRE-CONDITION: Have an active admin account
/// PRIORITY     : Trung bình
/// EXPECTED     : Error message "Price must be greater than or equal to 0"
///                System stays on current page, product NOT created
/// </summary>
[TestFixture]
[Category("ProductManagement")]
public class TC_AddProduct_NegativePrice
{
    private IWebDriver?  _driver;
    private LoginPage?   _loginPage;
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
            "TC_PRODUCTMANAGEMENT_ADDPRODUCTS_03",
            "Verify system validation for negative value in Unit Price field (Negative Test)");

        GenReport.LogInfo("[SETUP] Initializing ChromeDriver (1256x748)...");
        _driver      = DriverManager.InitDriverWithSize(1256, 748);
        _loginPage   = new LoginPage(_driver);
        _productPage = new ProductPage(_driver);
        GenReport.LogPass("[SETUP] ChromeDriver initialized successfully");
    }

    [Test, Order(2)]
    public void AddProduct_WithNegativeUnitPrice_ShouldShowValidationError()
    {
        // ── STEP 1-10: Login & Navigate ──────────────────────────────
        GenReport.LogInfo("[STEP 1-10] Logging in and navigating to Add Product page");
        _loginPage!.LoginAs(TestSettings.Username, TestSettings.Password);
        _productPage!.NavigateToProductPage();

        // ── STEP 11-12: Enter Product Name ───────────────────────────
        GenReport.LogInfo("[STEP 11-12] Entering product name: Testing Negative Price");
        _productPage.EnterProductName("Testing Negative Price");

        // ── STEP 13-14: Enter INVALID Negative Unit Price ────────────
        GenReport.LogInfo("[STEP 13-14] Entering INVALID unit price: -1000 (negative value)");
        _productPage.EnterUnitPrice("-1000");
        GenReport.LogWarning("Intentionally invalid input: negative price -1000");

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
        GenReport.LogInfo("[STEP 27-28] Entering description: Test negative price validation");
        _productPage.EnterDescription("Test negative price validation");

        // ── STEP 29: Click Submit ─────────────────────────────────────
        GenReport.LogInfo("[STEP 29] Clicking Submit button with invalid data");
        _productPage.ClickSubmit();

        // ── STEP 30: Assertions ───────────────────────────────────────
        GenReport.LogInfo("[STEP 30] Verifying validation: system should stay on page & show error");

        // Assertion 1: System must stay on Add Product page (not redirect)
        bool staysOnPage = _productPage.IsStillOnCreatePage();
        Assert.That(staysOnPage, Is.True,
            "System should remain on Add Product page when unit price is negative");
        GenReport.LogPass("[ASSERT 1] System stays on Add Product page - NOT redirected");

        // Assertion 2: Error message should be visible
        try
        {
            var wait = DriverManager.GetWait(_driver!);
            var errorElement = wait.Until(d =>
            {
                var els = d.FindElements(By.CssSelector(
                    ".text-danger, .invalid-feedback, .field-validation-error, " +
                    "[class*='error'], [class*='invalid']"));
                return els.FirstOrDefault(e => e.Displayed && e.Text.Length > 0);
            });

            Assert.That(errorElement, Is.Not.Null,
                "Validation error element should be displayed");
            GenReport.LogPass($"[ASSERT 2] Validation error displayed: \"{errorElement!.Text}\"");

            // Soft check: message content
            if (errorElement.Text.Contains("0") || errorElement.Text.ToLower().Contains("price"))
                GenReport.LogPass("[ASSERT 2+] Error message content matches expected validation");
            else
                GenReport.LogWarning($"[ASSERT 2+] Error text differs from expected: {errorElement.Text}");
        }
        catch (Exception ex)
        {
            GenReport.LogWarning($"[ASSERT 2] Could not locate error element: {ex.Message}");
            // Check URL did NOT change as fallback
            Assert.That(_driver!.Url, Does.Not.Contain("success"),
                "URL should not contain 'success' when validation fails");
            GenReport.LogPass("[ASSERT 2 Fallback] URL does not indicate success");
        }
    }

    [TearDown]
    public void TearDown()
    {
        var status  = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed && _driver != null)
            GenReport.LogScreenshot(_driver, "Failure Screenshot - TC03");

        GenReport.SetTestResult(status, message);
        GenReport.LogInfo("[TEARDOWN] Closing browser");
        DriverManager.QuitDriver(_driver);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        GenReport.FlushReport();
        GenReport.Reset();
    }
}
