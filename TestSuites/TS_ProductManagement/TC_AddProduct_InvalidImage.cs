using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;
using KangatangAutomation.PageObjects;

namespace KangatangAutomation.TestSuites.TS_ProductManagement;

/// <summary>
/// TEST CASE ID : TC_PRODUCTMANAGEMENT_ADDPRODUCTS_05
/// TEST SCENARIO: TS_PRODUCTMANAGEMENT
/// DESCRIPTION  : Verify system validation for invalid image file format (Negative Test)
/// PRE-CONDITION: Have an active admin account
/// PRIORITY     : Trung bình
/// </summary>
[TestFixture]
[Category("ProductManagement")]
public class TC_AddProduct_InvalidImage
{
    private IWebDriver? _driver;
    private LoginPage?  _loginPage;
    private ProductPage? _productPage;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        GenReport.GetInstance("TS_ProductManagement");
        
        // Generate a fake text file to act as an invalid image
        string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData");
        Directory.CreateDirectory(dir);
        File.WriteAllText(Path.Combine(dir, "invalid_format.txt"), "This is not an image file.");
    }

    [SetUp]
    public void Setup()
    {
        GenReport.CreateTest(
            "TC_PRODUCTMANAGEMENT_ADDPRODUCTS_05",
            "Verify system validation for invalid image file format (Negative Test)");

        _driver = DriverManager.InitDriver(maximize: true);
        _loginPage  = new LoginPage(_driver);
        _productPage = new ProductPage(_driver);
    }

    [Test, Order(5)]
    public void AddProduct_WithInvalidImageFormat_ShouldShowError()
    {
        GenReport.LogInfo("[STEP 1] Logging in and navigating");
        _loginPage!.LoginAs(TestSettings.Username, TestSettings.Password);
        _productPage!.NavigateToProductPage();

        GenReport.LogInfo("[STEP 2] Entering valid text inputs");
        _productPage.EnterProductName("Test Invalid Image");
        _productPage.EnterUnitPrice("2000");

        GenReport.LogInfo("[STEP 3] Uploading an invalid file format (.txt)");
        _productPage.UploadImage("invalid_format.txt");
        
        GenReport.LogInfo("[STEP 4] Clicking Submit");
        _productPage.ClickSubmit();

        GenReport.LogInfo("[STEP 5] Verifying system block");
        bool staysOnPage = _productPage.IsStillOnCreatePage();
        Assert.That(staysOnPage, Is.True, "System should block non-image file uploads");
        GenReport.LogPass("[ASSERT] System successfully blocked .txt file upload");
    }

    [TearDown]
    public void TearDown()
    {
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed && _driver != null)
            GenReport.LogScreenshot(_driver, "Failure Screenshot - TC_PROD_05");

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
