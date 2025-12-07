// myUtil để tạo ra random nằm sau mk
using System.Text;

namespace WebApplication1.Helpers
{
    public class MyUtil
    {
        public static string UpLoadHinh(IFormFile Hinh, string folder)
        {
            try
            {
                var fullPath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, Hinh.FileName
                    );
                using (var myFile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Hinh.CopyTo(myFile);
                }
                return Hinh.FileName;
            } 
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string GenerateRandomKey(int lenght = 5)
        {
            var pattern = @"qazwsxedcr";
            var random = new Random();
            var sb = new StringBuilder();

            for(int i = 0; i < lenght; i++)
            {
                sb.Append(pattern[random.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }
    }
}
