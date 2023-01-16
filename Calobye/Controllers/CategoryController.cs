using Calobye.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Controllers
{
  public class CategoryController : Controller
  {
		// GET: Category
		public ActionResult Index(string slug)
		{	
			var category = CategoryService.getCategoryBySlug(slug);
			if(category != null)
			{
				ViewBag.CategoryTitle =  category.TITLE;
				ViewBag.type = "search";
				ViewBag.slug = category.SLUG;
				return View();
			} 

			return RedirectToAction("PageNotFound", "Error");
				
		}

		

		public ActionResult _CategoryNav()
		{
			var categories = CategoryService.getCategoryParent().OrderBy(m => m.time_create).ToList();
				return PartialView(categories);

				//return RedirectToAction("PageNotFound", "Error");
		}

		public ActionResult _CategoryShowNavMobile()
		{
			var categories = CategoryService.getCategoryParent().OrderBy(m => m.time_create).ToList();
			return PartialView(categories);

			//return RedirectToAction("PageNotFound", "Error");
		}

		public ActionResult _CategorySidebar()
		{
			var categories = CategoryService.getCategoryParent().OrderBy(m => m.time_create).ToList();
			return PartialView(categories);
		}

		public ActionResult _CategoryNavShowDetail()
		{
			var categories = CategoryService.getCategoies().OrderBy(m => m.time_create).ToList();
			return PartialView(categories);
		}
	}
}