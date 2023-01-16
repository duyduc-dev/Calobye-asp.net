using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calobye.Models;

namespace Calobye.Services
{
	public class AdminService
	{
		private static CalobyeDB db = new CalobyeDB();	

		public static List<admin> getAllAdmin()
		{
			return db.admins.ToList();
		}

		public static admin getAdminByID(string ID)
		{
			return db.admins.FirstOrDefault(m => m.id.Equals(ID));
		}

		public static admin getAdminByEmail(string email)
		{
			return db.admins.FirstOrDefault(m => m.email.Equals(email));
		}

		public static bool isExistAdminEmailButAdminID(string email, string id)
		{
			return getAllAdmin().Exists(m => m.email.Equals(email) && !m.id.Equals(id));
		}

		public static bool isExistAdminEmail(string email)
		{
			return getAdminByEmail(email) != null;
		}

		public static void updateAdmin(admin ad)
		{
			var a = db.admins.FirstOrDefault(m => m.id.Equals(ad.id));
			a.firstname = ad.firstname;
			a.lastname = ad.lastname;
			a.email = ad.email;
			a.avatar = ad.avatar;
			db.SaveChanges();
		}

		public static void createNewAdmin(admin ad)
		{
			db.admins.Add(ad);
			db.SaveChanges();
		}

		public static ServiceResult<string> deleteAdminByID(string id)
		{
			var ad = db.admins.FirstOrDefault(m => m.id.Equals(id));
			if(ad != null && !id.Equals("e9fe0a2b-5bd0-4d6a-acd3-2c6f5bc13800"))
			{
				db.admins.Remove(ad);
				db.SaveChanges();
				return new ServiceResult<string>
				{
					isError = false,
					message = $"Delete Admin \"{ad.lastname}\""
				};
			}

			return new ServiceResult<string>
			{
				isError = true,
				message = $"Can not delete Admin \"{ad.lastname}\""
			}; ;
		}
	}

}