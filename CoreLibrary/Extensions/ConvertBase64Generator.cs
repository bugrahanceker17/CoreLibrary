using System.Reflection;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CoreLibrary.Extensions
{
    public static class ConvertBase64Generator
    {
        public static string DynamicExcelGenerator<T>(List<string> columnNames, List<T> data)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet? worksheet = package.Workbook.Worksheets.Add("Sheet1");

                for (int i = 0; i < columnNames.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = columnNames[i];
                
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Chocolate);

                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                for (int row = 0; row < data.Count; row++)
                {
                    PropertyInfo[] properties = typeof(T).GetProperties();
                    for (int col = 0; col < properties.Length; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(data[row]);
                    }
                }

                for (int col = 1; col <= columnNames.Count; col++)
                {
                    worksheet.Column(col).AutoFit();
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
        }
        
        public static async Task<IFormFile> Base64ToImage(string item)
        {
            byte[] bytes = Convert.FromBase64String(item);

            byte[] byteWebp = ImageCompressExtension.CompressAsync(bytes);

            string toBase64 = Convert.ToBase64String(byteWebp);

            string imageName = $"{Guid.NewGuid().ToString()}.{toBase64.GetBase64MimeType()}";

            MemoryStream stream = new MemoryStream(byteWebp);
            IFormFile file = new FormFile(stream, 0, byteWebp.Length, imageName, imageName);
            return file;
        }

        public static string GetBase64MimeType(this string base64)
        {
            Dictionary<string, string> imgContentTypes = new Dictionary<string, string>
            {
                { "IVBOR", "png" }, { "/9J/4", "jpg" }, { "AAABA", "ico" }, { "R0LGO", "gif" }, { "PHN2Z", "svg" }, { "UklGR", "webp" }, {"JVBER", "pdf"}
            };
            return imgContentTypes.ContainsKey(base64.Trim()[..5]) ? imgContentTypes[base64.Trim()[..5]] : "jpg";
        }
    }
}