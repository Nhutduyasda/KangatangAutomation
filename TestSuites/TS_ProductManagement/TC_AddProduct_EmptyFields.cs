using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;
using KangatangAutomation.PageObjects;

namespace KangatangAutomation.TestSuites.TS_ProductManagement;

/// <summary>
/// TEST CASE ID : TC_PRODUCTMANAGEMENT_ADDPRODUCTS_02
/// TEST SCENARIO: TS_PRODUCTMANAGEMENT
/// DESCRIPTION  : Verify system validation for empty mandatory fields (Negative Test)
/// PRE-CONDITION: Have an active admin account
/// PRIORITY     : Cao
/// </summary>
[TestFixture]
[Category("ProductManagement")]
public class TC_AddProduct_EmptyFields
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
            "TC_PRODUCTMANAGEMENT_ADDPRODUCTS_02",
            "Verify system validation for empty mandatory fields (Negative Test)");

        GenReport.LogInfo("[SETUP] Initializing ChromeDriver...");
        _driver = DriverManager.InitDriver(maximize: true);
        _loginPage  = new LoginPage(_driver);
        _productPage = new ProductPage(_driver);
    }

    [Test, Order(3)]
    public void AddProduct_WithEmptyFields_ShouldShowValidationErrors()
    {
        GenReport.LogInfo("[STEP 1-8] Logging in");
        _loginPage!.LoginAs(TestSettings.Username, TestSettings.Password);

        GenReport.LogInfo("[STEP 9-10] Navigating to Add Product page");
        _productPage!.NavigateToProductPage();

        GenReport.LogInfo("[STEP 11] Leaving all fields empty and clicking Submit");
        _productPage.ClickSubmit();

        GenReport.LogInfo("[STEP 12] Verifying system prevents submission");
        bool staysOnPage = _productPage.IsStillOnCreatePage();
        Assert.That(staysOnPage, Is.True, "System should remain on Add Product page when fields are empty");
        GenReport.LogPass("[ASSERT] System successfully blocked submission of empty form");
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed && _driver != null)
            GenReport.LogScreenshot(_driver, "Failure Screenshot - TC_PROD_02");

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
