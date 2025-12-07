using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.Repository.Service;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "RequireUserRole")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly EcommerceContext db;

        private readonly IHttpClientFactory _httpClientFactory; // Fixed declaration

       


        public HomeController(ILogger<HomeController> logger, EcommerceContext context, IHttpClientFactory httpClientFactory) // Added IHttpClientFactory to constructor
        {
            _logger = logger;
            db = context;
            _httpClientFactory = httpClientFactory; // Assigned IHttpClientFactory
        }

        public async Task<IActionResult> Recommend(int? loai, int? page = 1)
        {
            var userId = HttpContext.User.Claims
                .SingleOrDefault(p => p.Type == MySetting.CART_CustomerID)?.Value;

            var history = db.Histories
                .Where(x => x.MaKh == userId)
                .OrderByDescending(x => x.Timestamp)
                .Take(5)
                .Select(x => x.Keyword)
                .ToList();

            var clickedProducts = db.UserInteractions
                .Where(x => x.MaKh == userId)
                .OrderByDescending(x => x.Timestamp)
                .Take(5)
                .Select(x => x.MaHh)
                .ToList();

            int pageSize = 12;
            int skip = ((page ?? 1) - 1) * pageSize;

            ViewBag.Loais = loai;
            ViewBag.CurrentPage = page;

            var payload = new
            {
                user_id = userId,
                history_keywords = history,
                clicked_products = clickedProducts
            };

            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                var response = await httpClient.PostAsJsonAsync("http://localhost:8000/recommend", payload);
                var fullList = await response.Content.ReadFromJsonAsync<List<RecommendProduct>>() ?? new List<RecommendProduct>();

                int totalItems = fullList.Count;
                ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

                var pagedProducts = fullList.Skip(skip).Take(pageSize).ToList();
                return View(pagedProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating HTTP client for recommendation service.");
                return View(new List<RecommendProduct>());
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> AskQuestion(string question)
        //{
        //    if (string.IsNullOrWhiteSpace(question))
        //        return Json(new { error = "Vui lòng nhập câu hỏi." });

        //    var payload = new { question };
        //    var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        //    var httpClient = _httpClientFactory.CreateClient();

        //    // Gọi FastAPI backend (port 8000 chẳng hạn)
        //    var response = await httpClient.PostAsync("http://localhost:8000/receive_question_ask", content);

        //    if (!response.IsSuccessStatusCode)
        //        return Json(new { error = "Không thể kết nối với chatbot." });

        //    var jsonResponse = await response.Content.ReadAsStringAsync();
        //    var chatbotResponse = JsonConvert.DeserializeObject<ChatbotResponseVM>(jsonResponse);

        //    return Json(chatbotResponse);
        //}


        [Route("/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
