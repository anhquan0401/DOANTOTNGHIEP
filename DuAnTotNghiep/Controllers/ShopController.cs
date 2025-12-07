using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "RequireUserRole")]
    public class ShopController : Controller
    {
        private readonly EcommerceContext db;

        public ShopController(EcommerceContext context)
        {
            db = context;
        }

        [HttpGet]

        // Dùng offset pagination
        public IActionResult Index(int? loai, int page = 1)
        {
            var hangHoas = db.HangHoas.AsQueryable(); // lấy hết danh sách có cùng mã loại


            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(h => h.MaLoai == loai.Value);
            }

            int pageSize = 9;
            int skip = (page - 1) * pageSize;

            int totalItems = hangHoas.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.Loais = loai;

            var result = hangHoas.Skip(skip).Take(pageSize).Select(r => new HangHoaVM
            {
                MaHH = r.MaHh,
                TenHH = r.TenHh,
                Soluong = r.SoLuong,
                Img = r.Hinh ?? "",
                DonGia = r.DonGia ?? 0,
                MoTa = r.MoTa ?? "",
                DaBan = db.ChiTietHds.Where(c => c.MaHh == r.MaHh).Sum(c => c.SoLuong) // Tính tổng số lượng đã bán
            }).ToList();

            return View(result); // Trả về danh sách các đối tượng HangHoaVM
        }

        // Dùng cursor pagination
        //public IActionResult Index(int? loai, int? cursor = null)
        //{
        //    var hangHoas = db.HangHoas.AsQueryable();

        //    if (loai.HasValue)
        //        hangHoas = hangHoas.Where(h => h.MaLoai == loai.Value);

        //    hangHoas = hangHoas.OrderBy(h => h.MaHh);

        //    if (cursor.HasValue)
        //        hangHoas = hangHoas.Where(h => h.MaHh > cursor.Value);

        //    int pageSize = 9;
        //    var result = hangHoas.Take(pageSize)
        //        .Select(r => new HangHoaVM
        //        {
        //            MaHH = r.MaHh,
        //            TenHH = r.TenHh,
        //            Soluong = r.SoLuong,
        //            Img = r.Hinh ?? "",
        //            DonGia = r.DonGia ?? 0,
        //            MoTa = r.MoTa ?? "",
        //            DaBan = db.ChiTietHds.Where(c => c.MaHh == r.MaHh).Sum(c => c.SoLuong)
        //        })
        //        .ToList();
        //    int? nextCursor = result.LastOrDefault()?.MaHH;
        //    ViewBag.NextCursor = nextCursor;

        //    return View(result); // Trả về danh sách các đối tượng HangHoaVM
        //}


        [HttpPost]
        public async Task<IActionResult> Search(string? query)
        {
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID)?.Value;
            if (string.IsNullOrEmpty(UserID)) return Unauthorized();

            var normalized = query?.ToLower().Trim() ?? string.Empty;

            // Cập nhật lịch sử tìm kiếm
            var existingHistory = db.Histories.FirstOrDefault(h => h.MaKh == UserID && h.Keyword == normalized);
            if (existingHistory == null)
            {
                db.Histories.Add(new History
                {
                    MaKh = UserID,
                    Keyword = normalized,
                    Timestamp = DateTime.Now,
                });
            }
            else
            {
                existingHistory.Timestamp = DateTime.Now;
            }
            await db.SaveChangesAsync();



            IQueryable<HangHoa> hangHoas = db.HangHoas.Include(h => h.MaLoaiNavigation);

            // Nếu có query
            if (!string.IsNullOrWhiteSpace(query))
            {
                // Ưu tiên tìm sản phẩm có tên chứa đầy đủ chuỗi query
                var exactMatches = hangHoas
                    .Where(h => h.TenHh.ToLower().Contains(normalized));

                if (exactMatches.Any())
                {
                    hangHoas = exactMatches;
                }
                else
                {
                    // Nếu không có, tìm theo từng từ như trước
                    var keywords = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    hangHoas = hangHoas
                        .AsEnumerable()
                        .Select(h =>
                        {
                            var name = h.TenHh?.ToLower() ?? "";
                            var category = h.MaLoaiNavigation?.TenLoai?.ToLower() ?? "";
                            var matchCount = keywords.Count(k => name.Contains(k) || category.Contains(k));

                            return new { HangHoa = h, Score = matchCount };
                        })
                        .Where(x => x.Score > 0)
                        .OrderByDescending(x => x.Score)
                        .Select(x => x.HangHoa)
                        .AsQueryable();
                }
            }

            // Kết quả trả về
            var result = hangHoas.Select(r => new HangHoaVM
            {
                MaHH = r.MaHh,
                TenHH = r.TenHh,
                Soluong = r.SoLuong,
                Img = r.Hinh ?? "",
                DonGia = r.DonGia ?? 0,
                MoTa = r.MoTa ?? ""
            });

            return View(result);
        }



        [HttpGet]
        public IActionResult History()
        {
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var history = db.Histories
                .Where(h => h.MaKh == UserID && h.Keyword != null) // Lọc theo người dùng và từ khóa không null
                .OrderByDescending(h => h.Timestamp)
                .Select(h => new
                {
                    id = h.Id,
                    keyword = h.Keyword
                })
                .Take(5)
                .ToList(); 
            return Json(history);
        }

        [HttpPost]
        public IActionResult DeleteHistory(int id)
        {
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var history = db.Histories.FirstOrDefault(h => h.Id == id && h.MaKh == UserID);
            if (history == null) return NotFound();

            db.Histories.Remove(history);
            db.SaveChanges();

            return Ok();
        }



        [HttpGet]
        public IActionResult SuggestSearch(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<object>()); // Trả về danh sách rỗng nếu query không hợp lệ
            }

            var suggestions = db.HangHoas
                .Where(hh => hh.TenHh.Contains(query)) // Tìm kiếm
                .Select(hh => new
                {
                    maHh = hh.MaHh,
                    tenHh = hh.TenHh,
                    hinh = hh.Hinh,
                    donGia = hh.DonGia ?? 0
                })
                .Take(5)
                .ToList();

            return Json(suggestions);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            // averageTotal star
            var totalComments = db.Commentss.Where(t => t.MaHh == id).Count();
            //var totalRating = db.Commentss.Where(t => t.MaHh == id).Select(p => p.Rating).Sum();
            //string averageTotal;

            //if (totalComments > 0)
            //{
            //    averageTotal = Math.Round((totalRating / (double)totalComments), 1).ToString();
            //}
            //else
            //{
            //    averageTotal = "Chưa có đánh giá";
            //}
            //ViewBag.AverageTotal = averageTotal;

            // total sold
            var sold = db.ChiTietHds.Where(s => s.MaHh == id).Count();
            ViewBag.Sold = sold;

            // deltail
            var data = db.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .Include(p => p.Commentss)
                    .ThenInclude(r => r.MaKhNavigation)
                .SingleOrDefault(p => p.MaHh == id); // dùng SingleOrDefault là lấy hết sản phẩm r nên ko cần gọi data

            if (data == null)
            {
                TempData["Message"] = $"Không thấy sản phẩm {id}";
                return Redirect("/404");
            }

            data.SoLanXem += 1; // tăng số lần xem


            var userId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            // nếu id trùng với maHh thì cập nhật là cout và time thôi
            if (db.UserInteractions.Any(u => u.MaKh == userId && u.MaHh == id))
            {
                var interaction = db.UserInteractions.First(u => u.MaKh == userId && u.MaHh == id);
                interaction.Count += 1;
                interaction.Timestamp = DateTime.Now;
            }
            else
            {
                var interaction = new UserInteraction
                {
                    MaKh = userId,
                    MaHh = id,
                    Count = 1,
                    Timestamp = DateTime.Now
                };
                db.UserInteractions.Add(interaction);
            }
            db.SaveChanges();



            var result = new ChiTietHangHoaVM
            {
                MaHH = data.MaHh,
                TenHH = data.TenHh,
                Soluong = data.SoLuong, // nhớ cập nhật lại khi người dùng mua rồi
                TenLoai = data.MaLoaiNavigation.TenLoai,
                Img = data.Hinh,
                DonGia = data.DonGia ?? 0,
                MoTa = "San Pham",
                ChiTiet = data.MoTa ?? "",
                Commentss = data.Commentss.ToList(),
                Rating = db.Commentss.Where(c => c.MaHh == data.MaHh).Any() ? db.Commentss.Where(c => c.MaHh == data.MaHh).Average(c => (double)c.Rating) : 0 // Tính điểm đánh giá trung bình
            };
            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> Details(CommentVM model, int id, int Rating, string comment)
        {
            var userID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var results = new Comments
            {
                MaKh = userID,
                MaHh = id,
                CommentDescription = comment,
                Rating = Rating,
                CommentDate = DateTime.Now
            };
            db.Commentss.Add(results);
            await db.SaveChangesAsync();

            return RedirectToAction("Details");
        }



    }
}
