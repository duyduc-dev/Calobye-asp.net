using Calobye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Services
{
	public class CartService
	{
		private static CalobyeDB calobyeDB = new();

		public static List<Cart> getCart()
		{
			List<Cart> listCart = HttpContext.Current.Session["Cart"] as List<Cart>;

			if(listCart == null)
			{
				listCart = new List<Cart>();
				HttpContext.Current.Session["Cart"] = listCart;
			}

			return listCart;
		}

		public static void setCart(List<Cart> cart)
		{
			HttpContext.Current.Session["Cart"] = cart;
		}

		public static void addProductToCart(string productID, int amount)
		{
			List<Cart> _cart = getCart();

			Cart product = _cart.Find(m => m.Product.ID.Equals(productID));

			if(product == null)
			{
				product = new Cart(productID, amount);
				_cart.Add(product);
				setCart(_cart);
			} else {
				product.amount += amount;
			}
		}

		public static double getTotalPriceAllProducts()
		{
			var cart = getCart();
			double result = 0;

			if(cart != null)
			{
				foreach(var prod in cart)
				{
					result += prod.total;
				}
			}

			return result;
		}
	}
}
