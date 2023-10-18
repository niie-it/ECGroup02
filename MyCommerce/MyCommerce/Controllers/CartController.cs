using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCommerce.Data;
using MyCommerce.Models;

namespace MyCommerce.Controllers
{
	public class CartController : Controller
	{
		private readonly MyeStoreContext _context;

		public CartController(MyeStoreContext context)
		{
			_context = context;
		}

		public List<CartItem> CartItems
		{
			get
			{
				var carts = HttpContext.Session.Get<List<CartItem>>("CART");
				if (carts == null)
				{
					carts = new List<CartItem>();
				}
				return carts;
			}
		}

		public IActionResult Index()
		{
			return View(CartItems);
		}

		public IActionResult AddToCart(int id, int qty = 1)
		{
			// 1. Đưa vào giỏ hàng
			// Kiểm tra xem hàng đã có hau chưa dựa vào id (MaHH)
			var gioHang = CartItems;
			var item = gioHang.SingleOrDefault(p => p.MaHh == id);
			if (item != null)//đã có
			{
				item.SoLuong += qty;
			}
			else
			{
				var hangHoa = _context.HangHoas.SingleOrDefault(h => h.MaHh == id);
				if (hangHoa != null)
				{
					item = new CartItem {
						MaHh = id, SoLuong = qty,
						TenHh = hangHoa.TenHh,
						DonGia = hangHoa.DonGia.Value,
						Hinh = hangHoa.Hinh
					};
					gioHang.Add(item);
				}
			}
			// cập nhật session
			HttpContext.Session.Set("CART", gioHang);

			// 2. Chuyển tới trang hiển thị giỏ hàng
			return RedirectToAction("Index");
		}
	}
}
