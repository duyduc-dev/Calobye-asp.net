using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Calobye.Constants
{
	public class AppRegex
	{
		public static Regex regexNumber = new Regex("^[0-9]+$");

		public static Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

		public static Regex regexNumberHasDot = new Regex(@"^[0-9.-]+$");
	}
}