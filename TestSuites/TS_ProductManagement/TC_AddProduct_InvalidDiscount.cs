using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;
using KangatangAutomation.PageObjects;

namespace KangatangAutomation.TestSuites.TS_ProductManagement;

/// <summary>
/// TEST CASE ID : TC_PRODUCTMANAGEMENT_ADDPRODUCTS_04
/// TEST SCENARIO: TS_PRODUCTMANAGEMENT
/// DESCRIPTION  : Verify system validation for Discount greater than 100 (Negative Test)
/// PRE-CONDITION: Have an active admin account
/// PRIORITY     : Trung bình
/// </summary>
[TestFixture]
[Category("ProductManagement")]
public class TC_AddProduct_InvalidDiscount
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
            "TC_PRODUCTMANAGEMENT_ADDPRODUCTS_04",
            "Verify system validation for Discount greater than 100 (Negative Test)");

        GenReport.LogInfo("[SETUP] Initializing ChromeDriver...");
        _driver = DriverManager.InitDriver(maximize: true);
        _loginPage  = new LoginPage(_driver);
        _productPage = new ProductPage(_driver);
    }

    [Test, Order(4)]
    public void AddProduct_WithDiscountGreaterThan100_ShouldShowError()
    {
        GenReport.LogInfo("[STEP 1] Logging in and navigating");
        _loginPage!.LoginAs(TestSettings.Username, TestSettings.Password);
        _productPage!.NavigateToProductPage();

        GenReport.LogInfo("[STEP 2] Entering valid basic info");
        _productPage.EnterProductName("Test Invalid Discount");
        _productPage.EnterUnitPrice("5000");
        _productPage.EnterQuantity("50");

        GenReport.LogInfo("[STEP 3] Entering INVALID discount: 101");
        _productPage.EnterDiscount("101");
        
        GenReport.LogInfo("[STEP 4] Clicking Submit");
        _productPage.ClickSubmit();

        GenReport.LogInfo("[STEP 5] Verifying validation");
        bool staysOnPage = _productPage.IsStillOnCreatePage();
        Assert.That(staysOnPage, Is.True, "System should block discount > 100");
        GenReport.LogPass("[ASSERT] System successfully blocked invalid discount > 100");
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed && _driver != null)
            GenReport.LogScreenshot(_driver, "Failure Screenshot - TC_PROD_04");

        GenReport.SetTestResult(status, message);
        
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
