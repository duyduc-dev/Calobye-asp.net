using Calobye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Utils
{
	public class Toast
	{
		public static ServiceResult<T> GetToast<T>()
		{
			return HttpContext.Current.Session["Toast"] as ServiceResult<T>;
		}

		public static void SetToast<T>(ServiceResult<T> serviceResult)
		{
			HttpContext.Current.Session["Toast"] = serviceResult;
		}

		public static bool isShowToast 
		{ 
			get 
			{
				return HttpContext.Current.Session["Toast"] != null;
			}
		}
	}
}