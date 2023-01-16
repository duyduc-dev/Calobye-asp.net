using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Constants
{
	public class DeliveryStatus
	{
		public static string NOT_RECEIVED_VN
		{
			get
			{
				return "Chưa nhận hàng";
			} 
		}

		public static string RECEIVED_VN
		{
			get
			{
				return "Đã nhận hàng";
			}
		}

		public static string RECEIVED_EN
		{
			get 
			{
				return "RECEIVED";
			}
		}

		public static string NOT_RECEIVED_EN
		{
			get
			{
				return "NOT RECEIVED";
			}
		}
	}
}