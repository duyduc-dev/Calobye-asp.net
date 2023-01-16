using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Models
{
	public class OrderHistory
	{

		public string CustomerID { get; set; }

		public PRODUCT Product { get; set; }

		public int Amount { get; set; }

		public DateTime DateCreate { set; get; }

		public bool isPaid { get; set; }

		public bool DeliveryStatus { get; set; }

		public double Total { get; set; }

	}
}