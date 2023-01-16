using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Calobye.Controllers;
using Calobye.Models;

namespace Calobye.Services
{
	public class AuthService
	{
		private static CalobyeDB calobyeDB = new CalobyeDB();

		public static CUSTOMER getAuth()
		{
			return HttpContext.Current.Session["Auth"] as CUSTOMER;
		}

		public static void setAuth(CUSTOMER auth)
		{
			HttpContext.Current.Session["Auth"] = auth;
		}


		public static bool isLogin()
		{
			return getAuth() != null;
		}

		public static ServiceResult<CUSTOMER> login(string username, string password)
		{
			ServiceResult<CUSTOMER> result = new ServiceResult<CUSTOMER>();
			var hashPass = hashPassword(password);// hashPassword -- method mã hóa mật khẩu
			if (CustomerService.isExistUserName(username))
			{
				var auth = calobyeDB.CUSTOMERs.FirstOrDefault(m => m.USER_NAME.Equals(username) && m.PASSWORD.Equals(hashPass));
				if(auth == null)
				{
					result.message = "Mật khẩu không chính xác.";
				}
				else
				{
					result.data = auth;
					setAuth(auth);
					result.message = "Đăng nhập thành công.";
				}
			}
			else
			{
				result.message = "Không tồn tại thành viên này.";
			}

			return result;
		}

		public static ServiceResult<string> updateCurrentAuth(CUSTOMER auth)
		{
			var serviceResult = new ServiceResult<string>();
			var current = getAuth();
			if(!current.ID.Equals(auth.ID))
			{
				serviceResult.message = "Current Auth not match.";
				serviceResult.isError = true;

				return serviceResult;
			}

			if(isLogin())
			{
				return CustomerService.updateCustomer(auth);
			}

			serviceResult.message = $"Please Login.";
			serviceResult.isError = true;
			return serviceResult;
		}

		public static bool isCorrectPassword(string password)
		{
			var auth = getAuth();
			return auth.PASSWORD.Equals(hashPassword(password));
		}

		public static void changePassword(CUSTOMER auth, string passwordOld, string passworldnew)
		{
			var user = calobyeDB.CUSTOMERs.SingleOrDefault(m => m.ID.Equals(auth.ID));
			if (isCorrectPassword(passwordOld))
			{
				user.PASSWORD = hashPassword(passworldnew);
				calobyeDB.SaveChanges();
			}
		}
		

		public static ServiceResult<string> PasswordRecovery(string email)
		{
			var serviceResult = new ServiceResult<string>();

			var auth = calobyeDB.CUSTOMERs.SingleOrDefault(m => m.EMAIL.Equals(email));
			var newPass = $"{DateTime.Now.Millisecond}{DateTime.Now.Minute}{DateTime.Now.Year}{DateTime.Now.Second}";
			if(auth == null)
			{
				serviceResult.data = null;
				serviceResult.message = "";
				return serviceResult;
			}
			auth.PASSWORD = hashPassword(newPass);
			calobyeDB.SaveChanges();

			serviceResult.data = newPass;
			serviceResult.message = "";
			return serviceResult;
		}

		public static string hashPassword(string chuoi)
		{
			string str_md5 = "";
			byte[] mang = System.Text.Encoding.UTF8.GetBytes(chuoi);

			MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
			mang = my_md5.ComputeHash(mang);

			foreach (byte b in mang)
			{
				str_md5 += b.ToString("X2");
			}

			return str_md5;
		}
	}
}