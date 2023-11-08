using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyCommerce.Data;
using MyCommerce.Helpers;
using MyCommerce.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace MyCommerce.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly MyeStoreContext _ctx;

        public KhachHangController(MyeStoreContext ctx)
        {
            _ctx = ctx;
        }

        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile FileHinh)
        {
            try
            {
                var khachHang = new KhachHang
                {
                    MaKh = model.MaKh,
                    HoTen = model.HoTen,
                    NgaySinh = model.NgaySinh,
                    DiaChi = model.DiaChi,
                    GioiTinh = model.GioiTinh,
                    DienThoai = model.DienThoai,
                    Email = model.Email,
                    Hinh = MyTool.UploadFileToFolder(FileHinh, "KhachHang"),
                    HieuLuc = true,//tùy điều kiện
                    RandomKey = MyTool.GetRandom()
                };
                khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                _ctx.Add(khachHang);
                _ctx.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public IActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model)
        {
            var kh = _ctx.KhachHangs.SingleOrDefault(p => p.MaKh == model.UserName);
            if (kh == null)
            {
                ViewBag.Message = "Không tồn tại user này";
                return View();
            }
            if (kh.MatKhau != model.Password.ToMd5Hash(kh.RandomKey))
            {
                ViewBag.Message = "Sai thông tin đăng nhập";
                return View();
            }

            // khai báo claims
            var claims = new List<Claim> { 
                new Claim(ClaimTypes.Name, kh.HoTen),
                new Claim(ClaimTypes.Email, kh.Email),

                // làm động danh sách Roles
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "Accountant"),
            };

            var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var princial = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(princial);

            return Redirect("/");
        }

        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
			return Redirect("/");
		}
    }
}
