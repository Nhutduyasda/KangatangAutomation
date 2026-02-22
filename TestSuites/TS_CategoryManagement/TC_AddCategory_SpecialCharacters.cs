using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;
using KangatangAutomation.PageObjects;

namespace KangatangAutomation.TestSuites.TS_CategoryManagement;

/// <summary>
/// TEST CASE ID : TC_CATEGORYMANAGEMENT_ADDCATEGORY_05
/// TEST SCENARIO: TS_CATEGORYMANAGEMENT
/// DESCRIPTION  : Verify system validation for special characters in Name (Negative Test)
/// PRE-CONDITION: Have an active admin account
/// PRIORITY     : Trung bình
/// </summary>
[TestFixture]
[Category("CategoryManagement")]
public class TC_AddCategory_SpecialCharacters
{
    private IWebDriver?   _driver;
    private LoginPage?    _loginPage;
    private CategoryPage? _categoryPage;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        GenReport.GetInstance("TS_CategoryManagement");
    }

    [SetUp]
    public void Setup()
    {
        GenReport.CreateTest(
            "TC_CATEGORYMANAGEMENT_ADDCATEGORY_05",
            "Verify system validation for special characters in Name (Negative Test)");

        _driver       = DriverManager.InitDriverWithSize(1256, 748);
        _loginPage    = new LoginPage(_driver);
        _categoryPage = new CategoryPage(_driver);
    }

    [Test, Order(4)]
    public void AddCategory_WithSpecialCharacters_ShouldShowError()
    {
        GenReport.LogInfo("[STEP 1] Login to system");
        _loginPage!.LoginAs(TestSettings.Username, TestSettings.Password);

        GenReport.LogInfo("[STEP 2] Navigate to Add Category");
        _categoryPage!.ClickDashboardMenu().ClickCategoriesMenu();

        GenReport.LogInfo("[STEP 3] Entering Name with Special Characters");
        _categoryPage.EnterCategoryName("@#$%^&*()_+!");
        
        GenReport.LogInfo("[STEP 4] Clicking Save");
        _categoryPage.ClickSave();

        GenReport.LogInfo("[STEP 5] Verifying system blocks special characters");
        bool staysOnPage = _categoryPage.IsStillOnAddPage();
        
        Assert.That(staysOnPage, Is.True, "System should block special characters in category name");
        GenReport.LogPass("[ASSERT] System successfully blocked special characters");
    }

    [TearDown]
    public void TearDown()
    {
        var status  = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed && _driver != null)
            GenReport.LogScreenshot(_driver, "Failure Screenshot - TC_CAT_05");

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
