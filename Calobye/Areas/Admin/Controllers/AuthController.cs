using Calobye.Services;
using Calobye.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Areas.Admin.Controllers
{
  public class AuthController : Controller
  {

		public ActionResult login()
		{
			if (AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("Index", "Home", new { area = "Admin" });
			}

			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				ViewBag.themeToast = result.data;
				Toast.SetToast<string>(null);
			}

			return View();
		}

		[HttpPost]
		public ActionResult login(FormCollection form)
		{
			var email = form["email"];
			var password = form["password"];

			var isError = false;
			Regex regexGmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

			if (String.IsNullOrEmpty(email))
			{
				ViewBag.validateEmail = "Please enter your email.";
				isError = true;
			}
			else if(!regexGmail.IsMatch(email))
			{
				ViewBag.validateEmail = "The email is not in the correct format.";
				isError = true;
			}

			if (String.IsNullOrEmpty(password))
			{
				ViewBag.validatePassword = "Please enter your password.";
				isError = true;
			}

			if(!isError)
			{
				var serviceResult = AdminAuthService.login(email, password);
				
				if(serviceResult.data == null)
				{
					Toast.SetToast(new Models.ServiceResult<string>
					{
						isError = serviceResult.isError,
						message = serviceResult.message
					});
					return this.login();
				}
				else if(serviceResult.data != null && AdminAuthService.isAdminLogin())
				{
					Toast.SetToast(new Models.ServiceResult<string>
					{
						isError = false,
						message = serviceResult.message,
						data = "bg-primary"
					});
					return RedirectToAction("Index", "Home");
				}
			}

			ViewBag.isError = isError;
			Toast.SetToast(new Models.ServiceResult<string>
			{
				isError = true,
				message = "Email or password is incorrect."
			});
			return this.login();
		}

		public ActionResult signOut()
		{
			var auth = AdminAuthService.getAdmin();	
			Toast.SetToast(new Models.ServiceResult<string>
			{
				isError = false,
				message = $"Good bye {auth.lastname} 😉!",
				data = "bg-primary"
			});
			AdminAuthService.setAdmin(null);
			return RedirectToAction("login", "Auth");
		}
		
		public ActionResult profile()
		{
			return View();
		}

		public ActionResult _HeaderAvatarAdmin()
		{

			var admin = AdminAuthService.getAdmin();

			return PartialView(admin);
		}

	}
}