// using OpenQA.Selenium;
// using OpenQA.Selenium.Support.UI;
// using KangatangAutomation.Config;
// using KangatangAutomation.Helpers;
// using KangatangAutomation.PageObjects;

// namespace KangatangAutomation.TestSuites.TS_CategoryManagement;

// /// <summary>
// /// TEST CASE ID : TC_CATEGORYMANAGEMENT_ADDCATEGORY_06
// /// TEST SCENARIO: TS_CATEGORYMANAGEMENT
// /// DESCRIPTION  : Verify system validation when entering HTML tags / XSS payload (Security Negative Test)
// /// PRE-CONDITION: Have an active admin account
// /// PRIORITY     : Cao
// /// </summary>
// [TestFixture]
// [Category("CategoryManagement")]
// public class TC_AddCategory_HtmlInjection
// {
//     private IWebDriver?   _driver;
//     private LoginPage?    _loginPage;
//     private CategoryPage? _categoryPage;

//     [OneTimeSetUp]
//     public void OneTimeSetup()
//     {
//         GenReport.GetInstance("TS_CategoryManagement");
//     }

//     [SetUp]
//     public void Setup()
//     {
//         GenReport.CreateTest(
//             "TC_CATEGORYMANAGEMENT_ADDCATEGORY_06",
//             "Verify system validation when entering HTML tags / XSS payload (Security)");

//         _driver       = DriverManager.InitDriverWithSize(1256, 748);
//         _loginPage    = new LoginPage(_driver);
//         _categoryPage = new CategoryPage(_driver);
//     }

//     [Test, Order(5)]
//     public void AddCategory_WithHtmlInjection_ShouldBlockRequest()
//     {
//         GenReport.LogInfo("[STEP 1] Login to system");
//         _loginPage!.LoginAs(TestSettings.Username, TestSettings.Password);

//         GenReport.LogInfo("[STEP 2] Navigate to Add Category");
//         _categoryPage!.ClickDashboardMenu().ClickCategoriesMenu();

//         GenReport.LogInfo("[STEP 3] Entering HTML/XSS payload in Name");
//         _categoryPage.EnterCategoryName("<script>alert('hack')</script>");
        
//         GenReport.LogInfo("[STEP 4] Clicking Save");
//         _categoryPage.ClickSave();

//         GenReport.LogInfo("[STEP 5] Verifying system handles dangerous input safely");
//         bool staysOnPage = _categoryPage.IsStillOnAddPage();
        
//         Assert.That(staysOnPage, Is.True, "System should block HTML/XSS injection tags");
//         GenReport.LogPass("[ASSERT] System successfully handled and blocked HTML injection");
//     }

//     [TearDown]
//     public void TearDown()
//     {
//         var status  = TestContext.CurrentContext.Result.Outcome.Status;
//         var message = TestContext.CurrentContext.Result.Message;

//         if (status == NUnit.Framework.Interfaces.TestStatus.Failed && _driver != null)
//             GenReport.LogScreenshot(_driver, "Failure Screenshot - TC_CAT_06");

//         GenReport.SetTestResult(status, message);
        
//         if (_driver != null)
//         {
//             _driver.Quit();
//             _driver.Dispose();
//             _driver = null;
//         }
//     }

//     [OneTimeTearDown]
//     public void OneTimeTearDown()
//     {
//         GenReport.FlushReport();
//         GenReport.Reset();
//     }
// }
