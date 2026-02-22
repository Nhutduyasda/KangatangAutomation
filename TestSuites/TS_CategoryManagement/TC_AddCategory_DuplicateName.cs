using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;
using KangatangAutomation.PageObjects;

namespace KangatangAutomation.TestSuites.TS_CategoryManagement;

/// <summary>
/// TEST CASE ID : TC_CATEGORYMANAGEMENT_ADDCATEGORY_04
/// TEST SCENARIO: TS_CATEGORYMANAGEMENT
/// DESCRIPTION  : Verify validation for duplicate category name (Negative Test)
/// PRE-CONDITION: Have an active admin account
/// PRIORITY     : Trung bình
/// </summary>
[TestFixture]
[Category("CategoryManagement")]
public class TC_AddCategory_DuplicateName
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
            "TC_CATEGORYMANAGEMENT_ADDCATEGORY_04",
            "Verify validation for duplicate category name (Negative Test)");

        _driver       = DriverManager.InitDriverWithSize(1256, 748);
        _loginPage    = new LoginPage(_driver);
        _categoryPage = new CategoryPage(_driver);
    }

    [Test, Order(3)]
    public void AddCategory_WithDuplicateName_ShouldShowError()
    {
        GenReport.LogInfo("[STEP 1] Login to system");
        _loginPage!.LoginAs(TestSettings.Username, TestSettings.Password);

        // Tạo tên danh mục chung cho cả 2 lần
        string duplicateName = "Duplicate_" + DateTime.Now.ToString("HHmmss");

        GenReport.LogInfo($"[STEP 2] Creating first category: {duplicateName}");
        _categoryPage!.ClickDashboardMenu().ClickCategoriesMenu();
        _categoryPage.EnterCategoryName(duplicateName);
        _categoryPage.ClickSave();

        // Chờ để hệ thống xử lý xong lần 1
        Thread.Sleep(2000);

        GenReport.LogInfo("[STEP 3] Attempting to create the SECOND category with the exact same name");
        _categoryPage.ClickDashboardMenu().ClickCategoriesMenu();
        _categoryPage.EnterCategoryName(duplicateName);
        _categoryPage.ClickSave();

        GenReport.LogInfo("[STEP 4] Verifying system validation blocks duplicate name");
        bool staysOnPage = _categoryPage.IsStillOnAddPage();
        
        Assert.That(staysOnPage, Is.True, "System should block creating a category with an existing name");
        GenReport.LogPass("[ASSERT] System successfully blocked duplicate category creation");
    }

    [TearDown]
    public void TearDown()
    {
        var status  = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed && _driver != null)
            GenReport.LogScreenshot(_driver, "Failure Screenshot - TC_CAT_04");

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
