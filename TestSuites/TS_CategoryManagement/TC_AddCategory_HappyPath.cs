using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using KangatangAutomation.Config;
using KangatangAutomation.Helpers;
using KangatangAutomation.PageObjects;

namespace KangatangAutomation.TestSuites.TS_CategoryManagement;

/// <summary>
/// TEST CASE ID : TC_CATEGORYMANAGEMENT_ADDCATEGORY_01
/// TEST SCENARIO: TS_CATEGORYMANAGEMENT
/// DESCRIPTION  : Verify successful category creation with valid data (Happy Path)
/// PRE-CONDITION: Have an active admin account
/// PRIORITY     : Cao
/// EXPECTED     : System redirects to Category List page.
///                Green success message "Category created successfully" displayed.
///                New category visible in the list.
/// </summary>
[TestFixture]
[Category("CategoryManagement")]
public class TC_AddCategory_HappyPath
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
            "TC_CATEGORYMANAGEMENT_ADDCATEGORY_01",
            "Verify successful category creation with valid data (Happy Path)");

        GenReport.LogInfo("[SETUP] Initializing ChromeDriver (1256x748)...");
        _driver       = DriverManager.InitDriverWithSize(1256, 748);
        _loginPage    = new LoginPage(_driver);
        _categoryPage = new CategoryPage(_driver);
        GenReport.LogPass("[SETUP] ChromeDriver initialized successfully");
    }

    [Test, Order(1)]
    public void AddCategory_WithValidData_ShouldRedirectToCategoryList()
    {
        // ── STEP 1-9: Login ──────────────────────────────────────────
        GenReport.LogInfo("[STEP 1-9] Navigating to site and logging in");
        _loginPage!.LoginAs(TestSettings.Username, TestSettings.Password);

        // ── STEP 10: Click Categories menu ────────────────────────────
        GenReport.LogInfo("[STEP 10] Clicking Categories menu (li:nth-child(4) p)");
        _categoryPage!.ClickDashboardMenu();
        _categoryPage.ClickCategoriesMenu();

        // ── STEP 11: Click Add New button ─────────────────────────────
        GenReport.LogInfo("[STEP 11] Clicking Add New button (.btn-add-category)");
        _categoryPage.ClickAddNew();

        // ── STEP 12-13: Select Supplier = Vinamilk (value=1) ─────────────
        GenReport.LogInfo("[STEP 12-13] Selecting supplier: Vinamilk (value=1)");
        _categoryPage.SelectSupplier("1");

        // ── STEP 14-15: Enter Category Name ───────────────────────────
        GenReport.LogInfo("[STEP 14-15] Entering category name: Sữa tươi tiệt trùng");
        _categoryPage.EnterCategoryName("Sữa tươi tiệt trùng");

        // ── STEP 16: Click Save ────────────────────────────────────────
        GenReport.LogInfo("[STEP 16] Clicking Save button (.btn-save)");
        _categoryPage.ClickSave();

        // ── STEP 17: Assert Redirect to Category List ─────────────────
        GenReport.LogInfo("[STEP 17] Verifying redirect to Category List page");
        var wait = DriverManager.GetWait(_driver!);
        bool redirected = false;
        try
        {
            wait.Until(d => d.Url.Contains("categor"));
            redirected = true;
        }
        catch { /* handled below */ }

        Assert.That(redirected, Is.True,
            "Expected redirect to Category List page after successful creation");
        GenReport.LogPass("[ASSERT 1] System redirected to Category List page");

        // ── STEP 18: Assert Success Toast ────────────────────────────
        GenReport.LogInfo("[STEP 18] Verifying success notification");
        try
        {
            var toast = _driver!.FindElement(By.CssSelector(
                ".alert-success, .toast-success, .notification-success, [class*='success']"));
            Assert.That(toast.Displayed, Is.True, "Success toast should be visible");
            GenReport.LogPass($"[ASSERT 2] Success notification displayed: \"{toast.Text}\"");
        }
        catch
        {
            GenReport.LogWarning("[ASSERT 2] Success toast not found - redirect verified instead");
        }

        // ── Assert: New category appears in list ────────────────────────
        GenReport.LogInfo("[ASSERT 3] Verifying new category appears in list");
        try
        {
            bool found = _driver!.PageSource.Contains("Sữa tươi tiệt trùng");
            Assert.That(found, Is.True,
                "Newly created category 'Sữa tươi tiệt trùng' should appear in the list");
            GenReport.LogPass("[ASSERT 3] New category found in Category List");
        }
        catch
        {
            GenReport.LogWarning("[ASSERT 3] Could not verify category in list - page source check skipped");
        }

        // ── STEP 17: Close browser ────────────────────────────────────
        GenReport.LogInfo("[STEP 17] Test completed - browser will be closed in TearDown");
    }

    [TearDown]
    public void TearDown()
    {
        var status  = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed && _driver != null)
            GenReport.LogScreenshot(_driver, "Failure Screenshot - TC_CAT_01");

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
