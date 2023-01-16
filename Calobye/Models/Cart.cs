using Calobye.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Models
{
	public class Cart
	{
		public Cart(string ProductID, int amount)
		{
			var product = ProductService.getProductByID(ProductID);
			this.Product = product;
			this.amount = amount;
		}

		public PRODUCT Product { get; set; }
		
		public int amount { get; set; }

		public double total {
			get {
				return (double)this.Product.PRICE * amount;
			}
		}
	}
}