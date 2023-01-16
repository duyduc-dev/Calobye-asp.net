using Calobye.Models;
using Calobye.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Controllers
{
  public class UserController : Controller
  {
    // GET: User
    public ActionResult Index()
    {
		  if (!AuthService.isLogin())
			{
				return RedirectToAction("login", "Auth");
			}

			return View();
    }

    public ActionResult edit()
    {
			if (!AuthService.isLogin())
			{
				return RedirectToAction("login", "Auth");
			}
			var auth = AuthService.getAuth();
					
			return View(auth);
    }

		[HttpPost]
		public ActionResult edit(FormCollection form)
		{
			if (!AuthService.isLogin())
			{
				return RedirectToAction("login", "Auth");
			}

			var email = form["email"];
			var phone = form["phone_number"];
			var orderAddress = form["order_address"];

			bool isError = false;
			string messageError = " không được bỏ trống";

			if (String.IsNullOrEmpty(email))
			{
				ViewBag.validate = "Email" + messageError; ;
				isError = true;
			}
			else
			{
				Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
				if (!regexMail.IsMatch(email))
				{
					ViewBag.validate = "Email không đúng định dạng. VD: abc@hotname.com";
					isError = true;
				}
			}

			if (String.IsNullOrEmpty(phone))
			{
				ViewBag.validate = "Số điện thoại" + messageError;
				isError = true;
			}
			else
			{
				Regex regexPhone = new Regex("^[0-9]+$");
				if (phone.Length != 10)
				{
					ViewBag.validate = "Số điện thoại phải chứa 10 ký tự";
					isError = true;
				}
				else
				if (!regexPhone.IsMatch(phone))
				{
					ViewBag.validate = "Số điện thoại chỉ được chứa số";
					isError = true;
				}
			}

			if (String.IsNullOrEmpty(orderAddress))
			{
				ViewBag.validate = "Địa chỉ đặt hàng" + messageError;
				isError = true;
			}


			if(!isError)
			{
				var currentAuth = AuthService.getAuth();
				currentAuth.EMAIL = email;
				currentAuth.PHONE_NUMBER = phone;
				currentAuth.ORDER_ADDRESS = orderAddress;
				var result = AuthService.updateCurrentAuth(currentAuth);
				;

				if(result.isError)
				{
					ViewBag.success = result.message;
				}
				else
				{
					ViewBag.success = "Cập nhập thông tin thành công.";
				}
			}

			return this.edit();
		}

		public ActionResult changePassword()
		{
			if (!AuthService.isLogin())
			{
				return RedirectToAction("login", "Auth");
			}

			var auth = AuthService.getAuth();

			return View(auth);
		}

		[HttpPost]
		public ActionResult changePassword(FormCollection form)
		{
			if (!AuthService.isLogin())
			{
				return RedirectToAction("login", "Auth");
			}

			var passwordOld = form["password_old"];
			var passwordNew = form["password_new"];
			var passwordNewAgain = form["password_new_again"];

			bool isError = false;
			string messageError = " không được bỏ trống";

			if (String.IsNullOrEmpty(passwordOld))
			{
				ViewBag.validate = "Mật khẩu cũ" + messageError;
				isError = true;
			}
			else if (!AuthService.isCorrectPassword(passwordOld))
			{
				ViewBag.validate = "Mật khẩu cũ không đúng";
				isError = true;
			}
			
			else
			if (String.IsNullOrEmpty(passwordNew))
			{
				ViewBag.validate = "Mật khẩu mới" + messageError;
				isError = true;
			}
			else if (passwordNew.Trim().Length < 6)
			{
				ViewBag.validate = "Mật khẩu có tối thiểu 6 kí tự.";
				isError = true;
			}
			else
			if (String.IsNullOrEmpty(passwordNewAgain))
			{
				ViewBag.validate = "Nhập lại mật khẩu mới" + messageError;
				isError = true;
			}
			else
			if(!passwordNew.Equals(passwordNewAgain))
			{
				ViewBag.validate = "Mật khẩu nhập lại không trùng khớp.";
				isError = true;
			}
			else
			if (!isError)
			{
				var currentAuth = AuthService.getAuth();
				AuthService.changePassword(currentAuth, passwordOld, passwordNewAgain);
				ViewBag.changeSuccess = "Đổi mật khẩu thành công.";
			}

			return this.changePassword();
		}

		public ActionResult DestroyCustomer()
		{
			var cus = AuthService.getAuth();
			CustomerService.deleteCustomer(cus);
			AuthService.setAuth(null);
			return RedirectToAction("login", "Auth");
		}
	}
}