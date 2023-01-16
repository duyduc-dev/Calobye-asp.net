using Calobye.Models;
using Calobye.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Calobye.Controllers
{
  public class AuthController : Controller
  {
    // GET auth/login
    public ActionResult login()
    {
			if(AuthService.isLogin())
			{
				return RedirectToAction("Index", "Home");
			}
      return View();
    }
	
		[HttpPost]
		public ActionResult login(FormCollection form)
		{
			var username = form["username"];
			var password = form["password"];
			var isError = false;
			var mess = "Không được bỏ trống trường này";

			if (String.IsNullOrEmpty(username))
			{
				ViewBag.validateUsername = mess;
				isError = true;
			}

			if(String.IsNullOrEmpty(password))
			{
				ViewBag.validatePassword = mess;
				isError = true;
			}

			if(!isError)
			{
				var result = AuthService.login(username, password);
				ViewBag.message = result.message;
				if(AuthService.isLogin())
				{
					return RedirectToAction("Index", "Home");
				}
			}

			return View();

		}

		// GET auth/register
		public ActionResult register()
	  {
			if (AuthService.isLogin())
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
	  }

		[HttpPost]
		public ActionResult register(FormCollection form)
		{

			// Lấy dữ kiệu từ input
			var firstname = form["firstname"];
			var lastname = form["lastname"];
			var phone = form["phone"];
			var orderAddress = form["order_address"];
			var email = form["email"];
			var username = form["username"];
			var password = form["password"];
			var password_again = form["password_again"];

			string messError = "Trường này không được bỏ trống.";
			bool isError = false;

			// Regex kiểm tra dạng số và mail
			Regex regexPhone = new Regex("^[0-9]+$");
			Regex regexGmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

			// Validate các input
			if (String.IsNullOrEmpty(firstname))
			{
				ViewBag.validateFristname = messError;
				isError = true;
			}

			if (String.IsNullOrEmpty(lastname))
			{
				ViewBag.validateLastname = messError;
				isError = true;
			}

			if (String.IsNullOrEmpty(phone))
			{
				ViewBag.validatePhone = messError;
				isError = true;
			}
			else if(!regexPhone.IsMatch(phone))
			{
				ViewBag.validatePhone = "Số điện thoại không đúng dịnh dạng.";
				isError = true;
			}
			else if(phone.Length != 10)
			{
				ViewBag.validatePhone = "Số điện thoại phải có 10 ký tự.";
				isError = true;
			}

			if (String.IsNullOrEmpty(orderAddress))
			{
				ViewBag.validateOrderAddress = messError;
				isError = true;
			}
			
			if(String.IsNullOrEmpty(email))
			{
				ViewBag.validateEmail = messError;
				isError = true;
			}
			else if(!regexGmail.IsMatch(email))
			{
				ViewBag.validateEmail = "Gmail không đúng dịnh dạng.";
				isError = true;
			}

			if(String.IsNullOrEmpty(username))
			{
				ViewBag.validateUserName = messError;
				isError = true;
			}
			else if(char.IsDigit(username[0]))
			{
				ViewBag.validateUserName = "Tên đăng nhập không bắt đầu bằng số";
				isError = true;
			}
			else if(username.Length < 6)
			{
				ViewBag.validateUserName = "Tên đăng nhập phải có hơn 6 ký tự.";
				isError = true;
			}

			if(String.IsNullOrEmpty(password))
			{
				ViewBag.validatePassword = messError;
				isError = true;
			}
			else if(password.Length < 6)
			{
				ViewBag.validatePassword = "Mật khẩu phải có hơn 6 ký tự.";
				isError = true;
			}

			if (String.IsNullOrEmpty(password_again))
			{
				ViewBag.validatePasswordAgain = messError;
				isError = true;
			}else if(!password_again.Equals(password))
			{
				ViewBag.validatePasswordAgain = "Mật khẩu nhập lại không trùng khớp.";
				isError = true;
			}

			//Nếu không có lỗi thì bắt đầu đăng ký
			if(!isError)
			{
				CUSTOMER newCus = new CUSTOMER()
				{
					ID = Guid.NewGuid().ToString(), // tạo id cho người dùng
					FIRST_NAME = firstname,
					LAST_NAME = lastname,
					EMAIL = email,
					ORDER_ADDRESS = orderAddress,
					PASSWORD = AuthService.hashPassword(password_again), // method mã hóa mật khẩu
					PHONE_NUMBER = phone,
					USER_NAME = username,
					time_create = DateTime.Now
					
				};
				// Đăng ký người dùng
				CustomerService.createCustomer(newCus);
				ViewBag.result = "Đã thêm thành viên mới thành công";
			}

			return this.register();

		}

		public ActionResult signout()
		{
			AuthService.setAuth(null);
			return RedirectToAction("login", "Auth");
		}

		public ActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public ActionResult ForgotPassword(FormCollection form)
		{
			var _email = form["email"];

			Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

			if (String.IsNullOrEmpty(_email))
				ViewBag.validate = "Email không được bỏ trống.";
			else if(!regexMail.IsMatch(_email))
				ViewBag.validate = "Email không đúng định dạng.";
			else if(!CustomerService.isExistEmail(_email))
				ViewBag.validate = "Email này không tồn tại.";
			else 
			{
				var pass = AuthService.PasswordRecovery(_email);
				var auth = CustomerService.getCustomerByGmail(_email);

				string subject = "[CALOBYE] Đổi mật khẩu Calobye.";
				string body = $"Kính gửi {auth.LAST_NAME}." +
											$"\nMật khẩu mới của bạn là {pass.data}" +
											$"\nVui lòng không chia sẻ với bất kì ai.";

				Gmail gmail = new(){
					to = _email,
					subject = subject,
					body = body
				};

				gmail.send();
				ViewBag.password = "Mật khẩu mới của bạn đã được gửi tới " + _email +
														".Vui lòng kiểm tra Gmail, có thể nằm trong Thư rác.";
			}

			return View();
		}

	}
}
