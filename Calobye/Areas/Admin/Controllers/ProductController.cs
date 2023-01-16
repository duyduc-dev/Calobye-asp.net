using Calobye.Constants;
using Calobye.Models;
using Calobye.Services;
using Calobye.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Calobye.Areas.Admin.Controllers
{
  public class ProductController : Controller
  {
      // GET: Admin/Product
    public ActionResult Index()
    {
			  if (!AdminAuthService.isAdminLogin())
			  {
				  return RedirectToAction("login", "Auth", new { area = "Admin" });
			  }

        var products = ProductService.getProducts().OrderByDescending(m => m.TIME_CREATE).ToList();

				if(Toast.isShowToast)
				{
					ViewBag.isShowToast = Toast.isShowToast;
					var result = Toast.GetToast<string>();
					ViewBag.isErrorToast = result.isError;
					ViewBag.bodyToast = result.message;
					Toast.SetToast<string>(null);
				}

			  return View(products);
    }

		[HttpGet]
		public ActionResult EditProduct(string product_id)
		{
			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			var product = ProductService.getProductByID(product_id);

			if (product == null)
			{
				return RedirectToAction("Index", "Home", new { area = "Admin" });
			}

			var categories = CategoryService.getCategoies();
			List<CATEGORY> listCate = new List<CATEGORY>();

			if(categories != null)
			{
				listCate.AddRange(categories);
			}

			ViewBag.categories = listCate;

			if (Toast.isShowToast)
			{
				ViewBag.isShowToast = Toast.isShowToast;
				var result = Toast.GetToast<string>();
				ViewBag.isErrorToast = result.isError;
				ViewBag.bodyToast = result.message;
				Toast.SetToast<string>(null);
			}

			return View(product);
		}

		[HttpPost]
		public ActionResult EditProduct(FormCollection form, string product_id)
		{

			if (!AdminAuthService.isAdminLogin())
			{
				return RedirectToAction("login", "Auth", new { area = "Admin" });
			}

			var title = form["title"];
			var description = form["description"];
			var price = form["price"];
			var category_id = form["category_id"];
			var slug = form["slug"];

			var product = ProductService.getProductByID(product_id);

			bool isError = false;
			var messError = "This field is required";
			string bodyToast = "";

			if (String.IsNullOrEmpty(title))
			{
				ViewBag.validateTitle = messError;
				isError = true;
			}
			else if (ProductService.isExistProductTitleButProductID(title, product_id))
			{
				ViewBag.validateTitle = "This name is exist.";
				isError = true;
			}

			if (String.IsNullOrEmpty(description))
			{
				ViewBag.validateDesc = messError;
				isError = true;
			}

			if (String.IsNullOrEmpty(price))
			{
				ViewBag.validatePrice = messError;
				isError = true;
			}
			else if (!AppRegex.regexNumberHasDot.IsMatch(price))
			{
				ViewBag.validatePrice = "Price does not match the format";
				isError = true;
			}
			else if (price.Length > 13)
			{
				ViewBag.validatePrice = "the price must be less than 9,999,999,999,999 VND";
				isError = true;
			}

				if (String.IsNullOrEmpty(slug))
			{
				ViewBag.validateSlug = messError;
				isError = true;
			} else if(ProductService.isExistProductSlugButProductID(slug, product_id))
			{
				ViewBag.validateSlug = "This slug is exist.";
				isError = true;
			}

			bodyToast = "Update failed.";

			if (!isError)
			{
				product.TITLE = title;
				product.DESCRIPTION = description;
				product.PRICE = Convert.ToDecimal(price);
				product.CATEGORY_ID = category_id;
				product.SLUG = slug;

				ProductService.updateProduct(product);
				bodyToast = "Update successful.";
			}

			ViewBag.isError = isError;
			Toast.SetToast(new ServiceResult<string>
			{
				isError = isError,
				message = bodyToast
			});


			return this.EditProduct(product_id);
		}

		public ActionResult DeleteProduct(string product_id)
		{
			var prod = ProductService.getProductByID(product_id);
			var result = ProductService.deleteProductByID(product_id);
			Image img = new Image($"~/Assets/images/product/", prod.THUMBNAIL);
			img.delete();
			Toast.SetToast(result);
			return RedirectToAction("Index", "Product");
		}

		[HttpGet]
		public ActionResult CreateProduct()
		{
			var categories = CategoryService.getCategoies();
			List<CATEGORY> listCate = new List<CATEGORY>();

			if (categories != null)
			{
				listCate.AddRange(categories);
			}

			ViewBag.categories = listCate;

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
		public ActionResult CreateProduct(HttpPostedFileBase thumbnail, FormCollection form)
		{
			var idProduct = Guid.NewGuid().ToString();
			var title = form["title"];
			var description = form["description"];
			var price = form["price"];
			var category_id = form["category_id"];
			var slug = form["slug"];

			bool isError = false;
			var messError = "This field is required";
			string bodyToast = "";

			if (String.IsNullOrEmpty(title))
			{
				ViewBag.validateTitle = messError;
				isError = true;
			}
			else if (ProductService.isExistProductTitle(title))
			{
				ViewBag.validateTitle = "This name is exist.";
				isError = true;
			}

			if (String.IsNullOrEmpty(description))
			{
				ViewBag.validateDesc = messError;
				isError = true;
			}

			if (String.IsNullOrEmpty(price))
			{
				ViewBag.validatePrice = messError;
				isError = true;
			}
			else if (!AppRegex.regexNumberHasDot.IsMatch(price))
			{
				ViewBag.validatePrice = "Price does not match the format";
				isError = true;
			}
			else if (price.Length > 13)
			{
				ViewBag.validatePrice = "The price must be less than 9,999,999,999,999 VND";
				isError = true;
			}

			if (String.IsNullOrEmpty(slug))
			{
				ViewBag.validateSlug = messError;
				isError = true;
			}
			else if (ProductService.isExistProductSlug(slug))
			{
				ViewBag.validateSlug = "This slug is exist.";
				isError = true;
			}

			Image img = new Image(thumbnail, "~/Assets/images/product", slug);
			var resultUploadImg = img.save(!isError);

			if (resultUploadImg.data == null)
			{
				ViewBag.validateThumbnail = resultUploadImg.message;
				isError = true;
			}

			bodyToast = "Create failed.";

			if (!isError)
			{
				var product = new PRODUCT()
				{
					ID = idProduct,
					TITLE = title,
					DESCRIPTION = description,
					PRICE = Convert.ToDecimal(price),
					CATEGORY_ID = category_id,
					SLUG = slug,
					THUMBNAIL = resultUploadImg.data,
					TIME_CREATE = DateTime.Now
				};
				

				ProductService.createProduct(product);
				bodyToast = "Create new product successful.";
			}

				ViewBag.isError = isError;
				Toast.SetToast(new ServiceResult<string>
				{
					isError = isError,
					message = bodyToast
				});

			return this.CreateProduct();
		}
	}
}