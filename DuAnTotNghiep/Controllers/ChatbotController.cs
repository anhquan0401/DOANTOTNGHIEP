using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Route("chatbot")]
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "RequireUserRole")]
    public class ChatbotController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<ChatbotController> _logger;

        public ChatbotController(IHttpClientFactory httpClientFactory, ILogger<ChatbotController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask(string question)
        {
            try
            {
                // Gửi request sang FastAPI hoặc model xử lý AI
                var httpClient = new HttpClient();
                var response = await httpClient.PostAsJsonAsync("http://localhost:8000/receive_question_ask", new { question });

                var jsonString = await response.Content.ReadAsStringAsync();
                Console.WriteLine("👉 JSON từ FastAPI:");
                Console.WriteLine(jsonString);

                if (!response.IsSuccessStatusCode)
                    return Json(new ChatbotResponseVM { Error = "Không thể kết nối tới chatbot." });

                var data = JsonConvert.DeserializeObject<ChatbotResponseVM>(jsonString); // Dùng Newtonsoft cho chắc
                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new ChatbotResponseVM { Error = ex.Message });
            }
        }
    }
}
