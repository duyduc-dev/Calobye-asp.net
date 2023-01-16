using Calobye.Services;
using Calobye.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Areas.Admin.Controllers
{
  public class OrderController : Controller
  {
    // GET: Admin/Order
    public ActionResult Index()
    {
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

      var order = OrderService.ordersManagement().OrderByDescending(m => m.dateOrder).ToList();

			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				Toast.SetToast<string>(null);
			}

			return View(order);
    }

		public ActionResult EditOrder(string order_id)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			var orderDetail = OrderService.getOrderManagementByID(order_id);

			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				Toast.SetToast<string>(null);
			}

			return View(orderDetail);
		}

		[HttpPost]
		public ActionResult EditOrder(FormCollection form, string order_id)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}
			var deliveryStatus =  Convert.ToInt32(form["deliveryStatus"]) != 0;
			var isPaid =  Convert.ToInt32(form["isPaid"]) != 0;

			if(deliveryStatus && !isPaid)
			{
				ViewBag.isError = true;
				Toast.SetToast(new Models.ServiceResult<string>
				{
					isError = true,
					message = "Haven't paid, can't receive goods"
				});
				return this.EditOrder(order_id);
			}

			var result = OrderService.updateOrder(order_id, deliveryStatus, isPaid);
			ViewBag.isError = result.isError;
			Toast.SetToast(new Models.ServiceResult<string>
			{
				isError = result.isError,
				message = result.message
			});
			return this.EditOrder(order_id);
		}

		public ActionResult DeleteOrder(string order_id)
		{
			var result = OrderService.deleteOrderByID(order_id);
			Toast.SetToast(result);
			return RedirectToAction("Index", "Order");
		}
	}
}