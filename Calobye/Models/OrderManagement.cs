using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Models
{
	public class OrderManagement
	{
		public string ID { set; get; }
		public CUSTOMER customer { set; get; }
		public bool isPaid { get; set; }
		public bool deliveryStatus { get; set; }
		public double totalOrder { set; get; }
		public DateTime dateOrder { get; set; }
		public List<ORDER_DETAILS> listOrderDetail { set; get; }
	}
}