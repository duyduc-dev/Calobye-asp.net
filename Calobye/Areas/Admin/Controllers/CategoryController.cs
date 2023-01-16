using Calobye.Models;
using Calobye.Services;
using Calobye.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Areas.Admin.Controllers
{
  public class CategoryController : Controller
  {
    // GET: Admin/Category
    public ActionResult Index()
    {
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

      var category = CategoryService.getCategoies().OrderByDescending(m => m.time_create).ToList();
			
			
			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.bodyToast = result.message;
				ViewBag.isErrorToast = result.isError;
				Toast.SetToast<string>(null);
			}

			return View(category);
    }

    public ActionResult CreateCategory()
    {
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			ViewBag.categories = CategoryService.getCategoryParent();

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
		public ActionResult CreateCategory(FormCollection form)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			var title = form["title"];
			var slug = form["slug"];
			var parent_id = form["parent_id"];

			string messErr = "This feild is requierd";
			bool isError = false;
			string bodyToast = "";

			if(String.IsNullOrEmpty(title))
			{
				ViewBag.validateTitle = messErr;
				isError = true;
			}
			else if(CategoryService.isExistCategoryTitle(title))
			{
				ViewBag.validateTitle = "Title is exist.";
				isError = true;
			}

			if (CategoryService.isExistCategoryTitle(slug))
			{
				ViewBag.validateTitle = "Slug is exist.";
				isError = true;
			}

			bodyToast = "Create new category failed.";

			if (!isError)
			{
				CATEGORY cat = new()
				{
					ID = Guid.NewGuid().ToString(),
					TITLE = title,
					SLUG = slug,
					PARENT_ID = parent_id == "0" ? null : parent_id,
					time_create = DateTime.Now
				};
				var result = CategoryService.createCategory(cat);
				bodyToast = result.message;
				isError = result.isError;
			}

			ViewBag.isError = isError;

			Toast.SetToast(new ServiceResult<string>
			{
				isError = isError,
				message = bodyToast,
			});

			return this.CreateCategory();
		}

		public ActionResult EditCategory(string category_id)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			var cate = CategoryService.getCategoryByID(category_id);

			if (cate.PARENT_ID != null)
			{
				var parentCategory = CategoryService.getCategoryByID(cate.PARENT_ID);
				ViewBag.parentCategory = parentCategory.TITLE;
			}

			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				Toast.SetToast<string>(null);
			}
			return View(cate);
		}

		[HttpPost]
		public ActionResult EditCategory(string category_id, FormCollection form)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			var title = form["title"];
			var slug = form["slug"];

			var isError = false;

			if(String.IsNullOrEmpty(title))
			{
				ViewBag.validateTitle = "Title is rquired";
				isError = true;
			}
			else if(CategoryService.isExistCategoryTitleButCategoryID(title, category_id))
			{
				ViewBag.validateTitle = "Title is exist";
				isError = true;
			}

			if (CategoryService.isExistCategorySlugButCategoryID(slug, category_id))
			{
				ViewBag.validateSlug = "Slug is exist";
				isError = true;
			}

			string bodyToast = $"Can't update category";

			if(!isError)
			{
				var cate = CategoryService.getCategoryByID(category_id);
				cate.TITLE = title;
				cate.SLUG = slug;
				var result = CategoryService.updateCategory(cate);
				bodyToast = result.message;
				isError = result.isError;
			}
			
			Toast.SetToast(new ServiceResult<string>
			{
				isError = isError,
				message = bodyToast
			});


			return this.EditCategory(category_id);
		}

		public ActionResult DeleteCategory(string category_id)
		{
			var result = CategoryService.deleteCategory(category_id);

			Toast.SetToast(result);
			
			return RedirectToAction("Index", "Category", new {area = "Admin"});
		}
	}
}