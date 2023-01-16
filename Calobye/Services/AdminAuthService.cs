using Calobye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Services
{
	public class AdminAuthService
	{
		private static CalobyeDB calobeDB = new CalobyeDB();

		public static admin getAdmin()
		{
			return HttpContext.Current.Session["Admin"] as admin;
		}

		public static void setAdmin(admin admin)
		{
			HttpContext.Current.Session["Admin"]  = admin;
		}

		public static bool isAdminLogin()
		{
			return getAdmin() != null;
		}

		public static ServiceResult<admin> login(string email, string password)
		{
			var hashPass = AuthService.hashPassword(password);

			var admin = calobeDB.admins.FirstOrDefault(m => m.email.Equals(email) && m.password.Equals(hashPass));
			
			if(admin == null)
			{
				return new ServiceResult<admin>
				{
					isError = true,
					data = null,
					message = "Email or password is incorrect."
				};
			}

			setAdmin(admin);
			return new ServiceResult<admin>
			{
				isError = false,
				data = admin,
				message = $"Hi {admin.lastname} 😁!"
			};
		}
	}
}