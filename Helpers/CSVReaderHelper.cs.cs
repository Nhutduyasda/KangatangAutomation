using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KangatangAutomation.Helpers
{
    public static class CSVReaderHelper
    {
        // <summary>
        /// Đọc dữ liệu từ file CSV và trả về danh sách TestCaseData cho NUnit
        /// </summary>
        /// <typeparam name="T">Loại Model ánh xạ với file CSV</typeparam>
        /// <param name="csvFileName">Tên file CSV (ví dụ: "ProductData.csv") nằm trong thư mục TestData</param>
        /// <returns>Dữ liệu đã được định dạng để truyền vào [TestCaseSource]</returns>
        public static IEnumerable<TestCaseData> ReadCsvData<T>(string csvFileName)
        {
            // Đường dẫn tương đối đến file CSV trong thư mục TestData
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", csvFileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Không tìm thấy file CSV tại đường dẫn: {path}");
            }

            // Cấu hình CsvHelper (chuẩn InvariantCulture)
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null // Bỏ qua lỗi nếu CSV thiếu cột so với Model
            };

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, config);

            // Lấy tất cả dữ liệu từ file CSV chuyển thành danh sách các Object T
            var records = csv.GetRecords<T>().ToList();

            // Trả về từng dòng dữ liệu bằng yield return dưới định dạng TestCaseData của NUnit
            foreach (var record in records)
            {
                yield return new TestCaseData(record);
            }
        }
    }
}
