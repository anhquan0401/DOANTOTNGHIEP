using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Net;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.Hubs;
using WebApplication1.Repository.Interface;
using WebApplication1.Repository.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// connection with database
builder.Services.AddDbContext<EcommerceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HShop"));
});

// cycle life DJ:
builder.Services.AddTransient<IMyEmailSender, MyEmailSender>();

// session để lưu đơn hàng trên server
builder.Services.AddDistributedMemoryCache();

// coockie
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie?view=aspnetcore-8.0
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
//        options =>
//        {
//            options.LoginPath = "/KhachHang/DangNhap";
//            //options.LoginPath = "/Admin/AccountAdmin/LoginAdmin";
//            options.AccessDeniedPath = "/AccessDenied"; // nếu user đó ko có quyền truy cập thì sẽ chuyển tới trang AccessDenied vd như thông báo lỗi
//        });
builder.Services.AddAuthentication()
    .AddCookie("UserCookies", options =>
    {
        options.LoginPath = "/KhachHang/DangNhap";
        options.AccessDeniedPath = "/AccessDenied";
        options.Cookie.Name = "UserAuthCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    })
    .AddCookie("AdminCookies", options =>
    {
        options.LoginPath = "/Admin/AccountAdmin/LoginAdmin";
        options.AccessDeniedPath = "/Admin/AccessDenied";
        options.Cookie.Name = "AdminAuthCookie";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireClaim(ClaimTypes.Role, "QuanTriVien"));
    options.AddPolicy("RequireStaffAccess", policy => policy.RequireClaim(ClaimTypes.Role, "QuanTriVien", "NhanVien"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireClaim(ClaimTypes.Role, "Customer"));
});


// session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

////// Cập nhật chuỗi kết nối Redis trong Program.cs
//builder.Services.AddSingleton<IConnectionMultiplexer>(
//    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))
//);


// signalR
// signalR - ĐẶT IUserIdProvider TRƯỚC AddSignalR()
builder.Services.AddSignalR();


// đăng kí AutoMapper
// https://docs.automapper.org/en/stable/Dependency-injection.html
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


// đăng kí PaypalClient dạng Singleton() --> chỉ có 1 instance duy nhất trong toàn ứng dụng
builder.Services.AddSingleton(x => new PaypalClient(
    builder.Configuration["PaypalOpyions:AppId"],
    builder.Configuration["PaypalOpyions:AppSecret"],
    builder.Configuration["PaypalOpyions:Mode"]

));



builder.Services.AddHttpClient(); // Đăng ký dịch vụ HttpClient


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");

app.UseSession();


app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=HomeAdmin}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Recommend}/{id?}");

app.Run();
