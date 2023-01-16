using Calobye.Models;
using Calobye.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Controllers
{
	public class CustomerService
	{
		private static CalobyeDB calobyeDB = new CalobyeDB();

		public static List<CUSTOMER> getCustomers()
		{
			return calobyeDB.CUSTOMERs.ToList();
		}

		public static bool isExistEmail(string email)
		{
			var customer = calobyeDB.CUSTOMERs.FirstOrDefault(m => m.EMAIL.Equals(email));

			return customer != null;
		}

		public static CUSTOMER getCustomerByGmail(string email)
		{
			if (isExistEmail(email))
			{
				var auth = calobyeDB.CUSTOMERs.FirstOrDefault(m => m.EMAIL.Equals(email));
				return auth;
			}

			return null;
		}

		public static CUSTOMER getCustomerByID(string ID)
		{
			return calobyeDB.CUSTOMERs.FirstOrDefault(m => m.ID.Equals(ID));
		}

		public static bool isExistUserName(string username)
		{
			var auth = calobyeDB.CUSTOMERs.FirstOrDefault(m => m.USER_NAME.Equals(username));

			return auth != null;
		}

		public static void createCustomer(CUSTOMER auth)
		{
			calobyeDB.CUSTOMERs.Add(auth);
			calobyeDB.SaveChanges();
		}

		public static ServiceResult<string> updateCustomer(CUSTOMER cus)
		{
			try
			{
				var user = calobyeDB.CUSTOMERs.SingleOrDefault(m => m.ID.Equals(cus.ID));
				user = cus;
				calobyeDB.SaveChanges();
					
				return new ServiceResult<string>
				{
					message = "Update success.",
					isError = false
				};
			}
			catch (Exception e)
			{
					
				return  new ServiceResult<string>
				{
					message = $"update failed: {e.Message}",
					isError = true
				}; 
			}
			
		}

		public static void deleteCustomer(CUSTOMER cus)
		{
			var customer = calobyeDB.CUSTOMERs.FirstOrDefault(m => m.ID.Equals(cus.ID));
			var order = calobyeDB.THE_ORDER.FirstOrDefault(m => m.CUSTOMER_ID.Equals(cus.ID));
			if(order != null)
			{
				var result = OrderService.deleteOrderByID(order.ID);
			}
			calobyeDB.CUSTOMERs.Remove(customer);
			calobyeDB.SaveChanges();
		}

	}
}