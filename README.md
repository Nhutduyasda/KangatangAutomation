# 🦘 KangatangAutomation

Selenium Automation Test Solution cho hệ thống **Kangatang** — được xây dựng với **C# + NUnit + Selenium WebDriver + ExtentReports**.

---

## 📁 Cấu trúc dự án

```
KangatangAutomation/
├── Config/
│   └── TestSettings.cs              # URL, credentials, timeout config
├── Helpers/
│   ├── GenReport.cs                 # ExtentReports: logging & HTML report
│   └── DriverManager.cs             # ChromeDriver init/quit, WebDriverWait
├── PageObjects/
│   ├── LoginPage.cs                 # Page Object: Login
│   ├── ProductPage.cs               # Page Object: Add Product
│   └── CategoryPage.cs              # Page Object: Add Category
├── TestSuites/
│   ├── TS_ProductManagement/
│   │   ├── TC_AddProduct_HappyPath.cs      # TC01 - Valid data → Success
│   │   └── TC_AddProduct_NegativePrice.cs  # TC03 - Negative price → Validation
│   └── TS_CategoryManagement/
│       ├── TC_AddCategory_HappyPath.cs     # TC01 - Valid data → Success
│       └── TC_AddCategory_EmptyName.cs     # TC02 - Empty name → Validation
├── TestData/
│   └── CafeDa.png                   # Ảnh upload cho test sản phẩm
├── report/                           # HTML reports (auto-generated)
├── KangatangAutomation.csproj
├── KangatangAutomation.sln
└── README.md
```

---

## 🧪 Test Suites & Test Cases

### Suite 1: TS_ProductManagement
| Test Case ID | Mô tả | Loại | Priority |
|---|---|---|---|
| TC_PRODUCTMANAGEMENT_ADDPRODUCTS_01 | Tạo sản phẩm với dữ liệu hợp lệ | Happy Path | Trung bình |
| TC_PRODUCTMANAGEMENT_ADDPRODUCTS_03 | Unit Price âm → hệ thống báo lỗi | Negative | Trung bình |

### Suite 2: TS_CategoryManagement
| Test Case ID | Mô tả | Loại | Priority |
|---|---|---|---|
| TC_CATEGORYMANAGEMENT_ADDCATEGORY_01 | Tạo category với dữ liệu hợp lệ | Happy Path | Cao |
| TC_CATEGORYMANAGEMENT_ADDCATEGORY_02 | Để trống Category Name → hệ thống báo lỗi | Negative | Cao |

---

## ⚙️ Cài đặt & Chạy

### Yêu cầu
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- Google Chrome (phiên bản **131**)
- ChromeDriver tương ứng (tự động qua NuGet package)

### 1. Clone repo
```bash
git clone https://github.com/Nhutduyasda/KangatangAutomation.git
cd KangatangAutomation
```

### 2. Thêm ảnh test vào TestData
> Copy file ảnh tên **`CafeDa.png`** vào thư mục `TestData/`

### 3. Restore packages
```bash
dotnet restore
```

### 4. Build project
```bash
dotnet build
```

### 5. Chạy tests

#### Chạy tất cả test cases
```bash
dotnet test
```

#### Chỉ chạy Suite 1 (Product Management)
```bash
dotnet test --filter Category=ProductManagement
```

#### Chỉ chạy Suite 2 (Category Management)
```bash
dotnet test --filter Category=CategoryManagement
```

#### Chạy 1 test case cụ thể
```bash
dotnet test --filter "FullyQualifiedName~TC_AddProduct_HappyPath"
```

---

## 📊 Báo cáo kết quả

Sau khi chạy, HTML report được sinh tự động tại:
```
report/
├── TS_ProductManagement_20260220_182500.html
└── TS_CategoryManagement_20260220_182501.html
```
Mở file `.html` bằng trình duyệt để xem kết quả chi tiết.

---

## 🔧 Cấu hình

Chỉnh sửa trong `Config/TestSettings.cs`:
```csharp
public const string BaseUrl  = "https://kt1.hksolution.io.vn/";
public const string Username = "nhutduy051";
public const string Password = "Nhutduy0501@";
public const string BrowserVersion = "131";
```

---

## 🛠 Công nghệ sử dụng

| Thư viện | Phiên bản | Mục đích |
|---|---|---|
| NUnit | 3.14.0 | Test framework |
| Selenium.WebDriver | 4.27.0 | Browser automation |
| Selenium.WebDriver.ChromeDriver | 131.x | Chrome driver |
| ExtentReports | 5.0.4 | HTML test reporting |
| .NET | 8.0 | Runtime |

---

## 📝 Ghi chú

- Dự án áp dụng **Page Object Model (POM)** để dễ bảo trì
- Dùng **WebDriverWait (Explicit Wait)** thay cho `Thread.Sleep`
- Mỗi Test Suite tạo **báo cáo HTML riêng**
- Khi test **FAIL**, tự động **chụp screenshot** đính kèm vào report
