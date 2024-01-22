using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CoreLibrary.Extensions
{
    public static class ConvertBase64Generator
    {
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