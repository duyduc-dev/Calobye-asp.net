using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Utils
{
	public static class Helpers
	{
		public static IHtmlString Toast(this HtmlHelper helper)
		{
			bool isShowToast = helper.ViewBag.isShowToast != null;
			string html = "";
			if (isShowToast)
			{
				string isErr = helper.ViewBag.isErrorToast ? "bg-danger" : helper.ViewBag.themeToast == null ?"bg-success" : helper.ViewBag.themeToast;
				string body = helper.ViewBag.bodyToast as string;

				 html = @$"
				<div class=""position-fixed end-0 p-3"" data-time-out=""3500"" style=""z-index: 100; top: 50px;"">
					<div class=""toast show align-items-center text-white {isErr}"" id=""toast"" role=""alert"" aria-live=""assertive"" aria-atomic=""true"">
						<div class=""d-flex"">
							<div class=""toast-body"">
								{body}
							</div>
							<button type=""button"" class=""btn-close fs-2 btn-close-white me-2 m-auto text-white"" data-bs-dismiss=""toast"" aria-label=""Close""></button>
						</div>
					</div>
				</div>
			";
			}
			
			return new MvcHtmlString(html);
		}
	}
}