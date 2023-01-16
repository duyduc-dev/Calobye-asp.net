using Calobye.Constants;
using Calobye.Models;
using Calobye.Services;
using Calobye.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Calobye.Areas.Admin.Controllers
{
  public class AdminController : Controller
  {
      // GET: Admin/Admin
    public ActionResult Index()
    {
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

      var admins = AdminService.getAllAdmin().OrderByDescending(m => m.time_create).ToList();
			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				Toast.SetToast<string>(null);
			}
			return View(admins);
    }

		public ActionResult EditAdmin(string admin_id)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			if(admin_id == null)
			{
				return RedirectToAction("Index", "Admin");
			}

			var admin = AdminService.getAdminByID(admin_id.Trim());
			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				Toast.SetToast<string>(null);
			}
			return View(admin);
		}

		[HttpPost]
		public ActionResult EditAdmin(string admin_id, FormCollection form)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			if (admin_id == null)
			{
				return RedirectToAction("Index", "Admin");
			}
			var firstname = form["firstname"];
			var lastname = form["lastname"];
			var email = form["email"];
			var admin = AdminService.getAdminByID(admin_id.Trim());

			var messError = "This feild is required";
			var isError = false;

			if(String.IsNullOrEmpty(firstname))
			{
				ViewBag.validateFirstName = messError;
				isError = true;
			}

			if (String.IsNullOrEmpty(lastname))
			{
				ViewBag.validateLastName = messError;
				isError = true;
			}

			if (String.IsNullOrEmpty(email))
			{
				ViewBag.validateEmail = messError;
				isError = true;
			}
			else if(!AppRegex.regexMail.IsMatch(email))
			{
				ViewBag.validateEmail = "Unformatted email.";
				isError = true;
			}
			else if(AdminService.isExistAdminEmailButAdminID(email, admin.id))
			{
				ViewBag.validateEmail = "This email is existed. Try email another.";
				isError = true;
			}
			

			string bodyToast = "Save Admin failed.";

			if (!isError)
			{
				var newAdmin = AdminService.getAdminByID(admin_id);
				newAdmin.firstname = firstname;
				newAdmin.lastname = lastname;
				newAdmin.email = email;

				AdminService.updateAdmin(newAdmin);
				bodyToast = $"Save Admin \"{newAdmin.lastname}\" success.";
			}

			ViewBag.isError = isError;
			Toast.SetToast(new ServiceResult<string>
			{
				isError = isError,
				message = bodyToast
			});
			
			return this.EditAdmin(admin_id);
		}

		public ActionResult	CreateAdmin()
		{
			if(!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				Toast.SetToast<string>(null);
			}

			return View();
		}

		[HttpPost]
		public ActionResult CreateAdmin(HttpPostedFileBase avatar, FormCollection form)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			Image img = new Image(avatar, "~/Areas/Admin/Assets/img/avatar-admin");
			var firstname = form["firstname"];
			var lastname = form["lastname"];
			var email = form["email"];
			var password = form["password"];
			var passwordAgain = form["passwordAgain"];

			string messErr = "This feild is required";
			bool isError = false;

			if (String.IsNullOrEmpty(firstname))
			{
				ViewBag.validateFirstName = messErr;
				isError = true;
			}

			if (String.IsNullOrEmpty(lastname))
			{
				ViewBag.validateLastName = messErr;
				isError = true;
			}

			if (String.IsNullOrEmpty(email))
			{
				ViewBag.validateEmail = messErr;
				isError = true;
			}
			else if (!AppRegex.regexMail.IsMatch(email))
			{
				ViewBag.validateEmail = "Unformatted email.";
				isError = true;
			}
			else if (AdminService.isExistAdminEmail(email))
			{
				ViewBag.validateEmail = "This email is existed. Try email another.";
				isError = true;
			}

			if(String.IsNullOrEmpty(password))
			{
				isError = true;
				ViewBag.validatePassword = messErr;
			}
			else if(password.Length < 6)
			{
				isError = true;
				ViewBag.validatePassword = "Password must be more than 6 characters in length.";
			}

			if(String.IsNullOrEmpty(passwordAgain))
			{
				ViewBag.validatePassword = messErr;
				isError = true;
			}
			else if(!passwordAgain.Equals(password))
			{
				ViewBag.validatePassword = "The re-entered password does not match the password";
				isError = true;
			}

			string adminID = Guid.NewGuid().ToString();
			var resultImg = img.save(adminID, !isError);

			if(resultImg.data == null)
			{
				ViewBag.validateAvatar = resultImg.message;
				isError = true;
			}

			string bodyToast = "Create new admin failed.";
			if (!isError)
			{
				admin newAdmin = new admin()
				{
					id = adminID,
					firstname = firstname,
					lastname = lastname,
					avatar = resultImg.data,
					email = email,
					password = AuthService.hashPassword(passwordAgain),
					time_create = DateTime.Now,
				};
				AdminService.createNewAdmin(newAdmin);
				bodyToast = "Create new admin success.";

			}

			ViewBag.isError = isError;
			Toast.SetToast(new ServiceResult<string>
			{
				isError = isError,
				message = bodyToast
			});

			return this.CreateAdmin();
		}

		public ActionResult DeleteAdmin(string adminID)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			var ad = AdminService.getAdminByID(adminID);
			if(!adminID.Equals(App.ADMIN_ID_DEFAULT))
			{
				Image img = new Image("~/Areas/Admin/Assets/img/avatar-admin/", ad.avatar);
				var result = AdminService.deleteAdminByID(adminID);
				img.delete();
				if (!result.isError && AdminAuthService.getAdmin().id.Equals(ad.id))
				{
					AdminAuthService.setAdmin(null);
					Console.WriteLine("xin chao");
					Toast.SetToast(new ServiceResult<string>
					{
						isError = false,
						message = $"{ad.lastname}, you have removed yourself from the admin list"
					});
					return RedirectToAction("login", "Auth", new { area = "Admin" });
				}

				Toast.SetToast(new ServiceResult<string>
				{
					isError = false,
					message = $"Delete Admin \"{ad.lastname}\" success."
				});
			}
			else
				Toast.SetToast(new ServiceResult<string>
				{
					isError = true,
					message = "Can not delete account \"Admin Default\""
				});


			return RedirectToAction("Index", "Admin", new { area = "Admin" });
			
		}

	}
}