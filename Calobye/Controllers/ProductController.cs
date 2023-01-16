using Calobye.Models;
using Calobye.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Calobye.Utils;

namespace Calobye.Controllers
{
  public class ProductController : Controller
  {
    // GET: Product
    public ActionResult Index(string slug)
    {
			var product = ProductService.getProductBySlug(slug);
			if(product != null)
			{	
				return View(product);
			}

			return RedirectToAction("PageNotFound", "Error");
    }

    public ActionResult search(string q)
    {
      ViewBag.q = q == "" ? "Sản phẩm" : q;
			return View();
    }

		public ActionResult _HomePageFeaturedProducts()
		{
			var products = ProductService.getFeaturedProducts();
			return PartialView(products);
		}

		public ActionResult _HomePageNewProducts()
		{
			var products = ProductService.getNewProducts(5);
			return PartialView(products);
		}

		public ActionResult _HomePageSpecialProduct()
		{
			var products = ProductService.getSpecialProduct(5);
			return PartialView(products);
		}


		public ActionResult searchJsonByKey(string search_key)
    {
			var products = ProductService.searchProducts(search_key);
      var data = UtilsProduct.StringArrayProduct(products);
			return Json(new { status = 200, message = search_key, data = data }, JsonRequestBehavior.AllowGet);
    }

		public ActionResult JsonGetProductsByCategory(string category_slug)
		{
			var products = ProductService.getProductsByCategorySlugHasChild(category_slug);
			var result = "[]";
			if (result != null)
			{
				result = Utils.UtilsProduct.StringArrayProduct(products);
			}

			return Json(new { data = result, messgae = category_slug }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult JsonGetProductByID(string product_id)
		{
			var product = ProductService.getProductByID(product_id);

			var json = $"{Utils.UtilsProduct.keyValueObject("id", product.ID)}" +
								 $"{Utils.UtilsProduct.keyValueObject("title", product.TITLE)}" +
								 $"{Utils.UtilsProduct.keyValueObject("slug", product.SLUG)}" +
								 $"{Utils.UtilsProduct.keyValueObject("thumbnail", product.THUMBNAIL)}" +
								 $"{Utils.UtilsProduct.keyValueObject("description", product.DESCRIPTION)}" +
								 $"{Utils.UtilsProduct.keyValueObject("category_id", product.CATEGORY_ID)}" +
								 $"{Utils.UtilsProduct.keyValueObject("price", product.PRICE + "")}" +
								 $"{Utils.UtilsProduct.keyValueObject("time_create", product.TIME_CREATE.ToString())}";

			return Json("{" + json + "}", JsonRequestBehavior.AllowGet);
		
		}

	}
}