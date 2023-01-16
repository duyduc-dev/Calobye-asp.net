using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Calobye
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("api-search-product", "api/search-product/{search_key}", new { controller = "Product", action = "searchJsonByKey", search_key = UrlParameter.Optional }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("api-product-category", "api/product-category/{category_slug}", new { controller = "Product", action = "JsonGetProductsByCategory", category_slug = UrlParameter.Optional }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("api-product-by-id", "api/product/{product_id}", new { controller = "Product", action = "JsonGetProductByID", product_id = UrlParameter.Optional }, new string[] { "Calobye.Controllers" });


			routes.MapRoute("Product-search", "search", new { controller = "Product", action = "search" }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("Products", "product/{slug}", new { controller = "Product", action = "Index", slug = UrlParameter.Optional }, new string[] { "Calobye.Controllers" });

			routes.MapRoute("Category", "category/{slug}", new { controller = "Category", action = "Index", slug = UrlParameter.Optional }, new string[] { "Calobye.Controllers" });

			routes.MapRoute("User-change-password", "user/change-password", new { controller = "User", action = "changePassword" }, new string[] { "Calobye.Controllers" });
      routes.MapRoute("User", "user/{action}", new { controller = "User", action = "Index"}, new string[] { "Calobye.Controllers" });

			routes.MapRoute("Order-history", "order/history", new { controller = "Order", action = "OrderHistory" }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("Order-success", "order/success", new { controller = "Order", action = "orderSucsess" }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("Order", "order", new { controller = "Order", action = "Index" }, new string[] { "Calobye.Controllers" });

			routes.MapRoute("Cart-buy-now", "cart/buy-now", new { controller = "Cart", action = "BuyProductNow" }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("Cart-delete-prod-to-cart", "cart/delete-product-to-cart", new { controller = "Cart", action = "DeleteProductInCartByID" }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("Cart-add-prod-to-cart", "cart/add-product-to-cart", new { controller = "Cart", action = "AddProductToCart" }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("Cart", "cart", new { controller = "Cart", action ="Index" }, new string[] { "Calobye.Controllers" });

			routes.MapRoute("auth-signout", "auth/signout", new { controller = "Auth", action = "signout" }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("Auth-forgot-password", "auth/forgot-password", new { controller = "Auth", action = "ForgotPassword" }, new string[] { "Calobye.Controllers" });
			routes.MapRoute("Auth", "auth/{action}", new { controller = "Auth" },  new string[] { "Calobye.Controllers" });

			routes.MapRoute("Home", "", new { controller = "Home", action = "Index" }, new string[] { "Calobye.Controllers" });

			routes.MapRoute("PageNotFound", "error/404", new { controller = "Error", action = "PageNotFound" });
			routes.MapRoute("All-route-no-match", "{*url}", new { controller = "Error", action = "RedirectPage404" });

			routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional }, new string[] {"Calobye.Controllers"});
		}
  }
}
