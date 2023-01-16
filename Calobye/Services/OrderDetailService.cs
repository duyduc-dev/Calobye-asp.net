using Calobye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Services
{
	public class OrderDetailService
	{
		private static CalobyeDB calobyeDB = new CalobyeDB();	

		public static List<ORDER_DETAILS> getOrderDetailByID(string ID)
		{
			return calobyeDB.ORDER_DETAILS.Where(m => m.ID.Equals(ID)).ToList();
		}

		public static double getTotalOrderDetailByID(string ID)
		{
			var orderDetail = getOrderDetailByID(ID);
			
			double total = 0;

			foreach (var item in orderDetail)
			{
				var product = ProductService.getProductByID(item.PRODUCT_ID);
				if(product != null)
					total += item.AMOUNT * (double)product.PRICE;
			}

			return total;

		}
	}
}