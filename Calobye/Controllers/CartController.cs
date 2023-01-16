using Calobye.Models;
using Calobye.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Controllers
{
  public class CartController : Controller
  {
    // GET: Cart
    public ActionResult Index()
    {
			var cart = CartService.getCart();
      ViewBag.totalAllProducts = CartService.getTotalPriceAllProducts();
      return View(cart);
    }

    public ActionResult DeleteProductInCartByID(string productID)
    {
      var cart = CartService.getCart();
      Cart prod = cart.SingleOrDefault(m => m.Product.ID.Equals(productID));

      if(prod != null)
      {
        cart.Remove(prod);
        CartService.setCart(cart);
			}

      return RedirectToAction("Index", "Cart");
    }

		[HttpPost]
    public ActionResult AddProductToCart(string idProduct, int amount)
    {
      CartService.addProductToCart(idProduct, amount);
      var product = ProductService.getProductByID(idProduct);

      return RedirectToAction("Index", "Product", new { slug = product.SLUG });
    }

    public ActionResult BuyProductNow(string idProduct, int amount)
    {
			CartService.addProductToCart(idProduct, amount);
			var product = ProductService.getProductByID(idProduct);

      return RedirectToAction("Index", "Cart");
		}

		public ActionResult _CartAmountDesktop()
		{
			var cart = CartService.getCart();

			ViewBag.amount = cart.Count();
			return PartialView();
		}

		public ActionResult _CartAmountMobile()
		{
			var cart = CartService.getCart();

			ViewBag.amount = cart.Count();
			return PartialView();
		}

		public ActionResult _CartAmountSidebar()
		{
			var cart = CartService.getCart();

			ViewBag.amount = cart.Count();
			return PartialView();
		}
	}
}
