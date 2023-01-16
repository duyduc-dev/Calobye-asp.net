using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Controllers
{
    public class ErrorController : Controller
    {

      public ActionResult RedirectPage404()
      {
        return Redirect("/error/404");
      }


			public ActionResult PageNotFound()
      {
         return View();
      }

		}
}