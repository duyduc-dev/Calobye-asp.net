using Calobye.Services;
using Calobye.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
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
            ViewBag.themeToast = result.data;

						Toast.SetToast<string>(null);
			    }

			    return View();
        }
    }
}