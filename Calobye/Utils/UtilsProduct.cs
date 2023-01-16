using Calobye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web.Mvc;

namespace Calobye.Utils
{
	public class UtilsProduct
	{
		public static string formatMoney(string num)
		{
			CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
			return double.Parse(num).ToString("#,###", cul.NumberFormat);
		}
		
		public static string ProductToString(PRODUCT prod)
		{
			return "{" + $"{keyValueObject("title", prod.TITLE)}, {keyValueObject("slug",prod.SLUG)}, {keyValueObject("thumbnail", prod.THUMBNAIL)}, {keyValueObject("price", prod.PRICE + "")}, {keyValueObject("desc", prod.DESCRIPTION + "")}" + "}";
		}

		public static string StringArrayProduct(List<PRODUCT> prod)
		{
			var json = "[";
			if (prod != null)
			{
				for (int i = 0; i < prod.Count; ++i)
				{
					if (i == prod.Count() - 1)
					{
						json += $"{ProductToString(prod.ElementAt(i))}";
						break;
					}
					json += $"{ProductToString(prod.ElementAt(i))},";
				}
			}

			return json + "]";
		}

		public static string keyValueObject(string key, string value)
		{
			return $"\"{key}\":\"{value}\"";
		}

		
	}
}
