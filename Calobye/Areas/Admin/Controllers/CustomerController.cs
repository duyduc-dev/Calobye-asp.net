using Calobye.Controllers;
using Calobye.Services;
using Calobye.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Areas.Admin.Controllers
{
  public class CustomerController : Controller
  {
    // GET: Admin/Customer
    public ActionResult Index()
    {

			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

      var customers = CustomerService.getCustomers().OrderByDescending(m => m.time_create).ToList();

			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				Toast.SetToast<string>(null);
			}
			return View(customers);
    }

    public ActionResult EditCustomer(string customer_id)
    {
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

      if(customer_id == null)
      {
        return RedirectToAction("Index", "Customer");
      }

			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				Toast.SetToast<string>(null);
			}

			var customer = CustomerService.getCustomerByID(customer_id);
			return View(customer);
    }

		public ActionResult DeleteCustomer(string customer_id)
		{
			var cus = CustomerService.getCustomerByID(customer_id);
			CustomerService.deleteCustomer(cus);
			Toast.SetToast(new Models.ServiceResult<string>
			{
				isError = false,
				message = $"Delete customer {cus.LAST_NAME} success."
			});
			return RedirectToAction("Index", "Customer");
		}


  }
}