using Calobye.Constants;
using Calobye.Models;
using Calobye.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Controllers
{
  public class OrderController : Controller
  {
    // GET: Order
    public ActionResult Index()
    {

			if(!AuthService.isLogin())
			{
				return RedirectToAction("login", "Auth");
			}

			var cart = CartService.getCart();


			if (cart == null || cart.Count <= 0)
			{
				return RedirectToAction("Index", "Cart");
			}
			ViewBag.totalAllProduct = CartService.getTotalPriceAllProducts();
			return View(cart);
    }

		// GET: Order/success
	  public ActionResult orderSucsess()
	  {

			if (!AuthService.isLogin())
			{
				return RedirectToAction("login", "Auth");
			}

			var cart = CartService.getCart();
			var auth = AuthService.getAuth();
			CalobyeDB db = new CalobyeDB();

			if (cart == null || cart.Count <= 0)
			{
				return RedirectToAction("Index", "Cart");
			}

			THE_ORDER order = new THE_ORDER()
			{
				ID = Guid.NewGuid().ToString(),
				CUSTOMER_ID = auth.ID,
				DATE_ORDER = DateTime.Now,
				IS_PAID = false,
				DELIVERY_STATUS = false,
			};

			db.THE_ORDER.Add(order);
			db.SaveChanges();

			foreach(var prod in cart)
			{
				ORDER_DETAILS orderDetail = new ORDER_DETAILS()
				{
					ID = order.ID,
					PRODUCT_ID = prod.Product.ID,
					AMOUNT = prod.amount,
					TOTAL = (decimal)prod.total,
				};

				db.ORDER_DETAILS.Add(orderDetail);
			}

			db.SaveChanges();

			ViewBag.totalAllProduct = CartService.getTotalPriceAllProducts();
			CartService.setCart(null);
			return View(cart);
	  }

		public ActionResult OrderHistory()
		{
			var historyOrder = OrderService.getHistoryOrder();
			return View(historyOrder);
		}
  }
}