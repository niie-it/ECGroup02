using Microsoft.AspNetCore.Mvc;
using MyCommerce.Data;
using MyCommerce.Models;

namespace MyCommerce.Controllers
{
	public class PaymentController : Controller
	{
		private readonly PaypalClient _paypalClient;
		private readonly MyeStoreContext _ctx;

		public PaymentController(PaypalClient paypalClient, MyeStoreContext ctx)
		{
			_paypalClient = paypalClient;
			_ctx = ctx;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult PaypalDemo()
		{
			ViewBag.PaypalClientId = _paypalClient.ClientId;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> PaypalOrder(CancellationToken cancellationToken)
		{
			// Tạo đơn hàng (thông tin lấy từ Session???)
			var tongTien = new Random().Next(100, 1000).ToString();
			var donViTienTe = "USD";
			// OrderId - mã tham chiếu (duy nhất)
			var orderIdref = "DH" + DateTime.Now.Ticks.ToString();

			try
			{
				// a. Create paypal order
				var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, orderIdref);

				return Ok(response);
			}
			catch (Exception e)
			{
				var error = new
				{
					e.GetBaseException().Message
				};

				return BadRequest(error);
			}
		}

		public async Task<IActionResult> PaypalCapture(string orderId, CancellationToken cancellationToken)
		{
			try
			{
				var response = await _paypalClient.CaptureOrder(orderId);

				var reference = response.purchase_units[0].reference_id;

				// Put your logic to save the transaction here
				// You can use the "reference" variable as a transaction key
				// Lưu đơn hàng vô database

				return Ok(response);
			}
			catch (Exception e)
			{
				var error = new
				{
					e.GetBaseException().Message
				};

				return BadRequest(error);
			}
		}

		public IActionResult Success()
		{
			return View();
		}
	}
}
