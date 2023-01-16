using System;
using System.Web.Mvc;

namespace Calobye.Areas.Admin
{
  public class AdminAreaRegistration : AreaRegistration 
  {
    public override string AreaName 
    {
        get 
        {
            return "Admin";
        }
    }

    public override void RegisterArea(AreaRegistrationContext context)
    {
			context.MapRoute("Admin-profile", "admin/profile", new { controller = "Auth", action = "profile" });
			context.MapRoute("Admin-register", "admin/register", new { controller = "Auth", action = "register" });
			context.MapRoute("Admin-login", "admin/login", new { controller = "Auth", action = "login" });
			context.MapRoute("Admin-sign-out", "admin/sign-out", new { controller = "Auth", action = "signOut" });

			context.MapRoute("Admin-customer", "admin/customer-management/", new { controller = "Customer", action = "Index" });
			context.MapRoute("Admin-customer-edit", "admin/customer-management/edit/{customer_id}", new { controller = "Customer", action = "EditCustomer", customer_id = UrlParameter.Optional });
			context.MapRoute("Admin-customer-delete", "admin/customer-management/delete/{customer_id}", new { controller = "Customer", action = "DeleteCustomer", customer_id = UrlParameter.Optional });

			context.MapRoute("Admin-admin", "admin/admin-management/", new { controller = "Admin", action = "Index" });
			context.MapRoute("Admin-admin-edit", "admin/admin-management/edit/{admin_id}/", new { controller = "Admin", action = "EditAdmin", admin_id = UrlParameter.Optional });
			context.MapRoute("Admin-admin-delete", "admin/admin-management/delete/{adminID}", new { controller = "Admin", action = "DeleteAdmin", adminID = UrlParameter.Optional });
			context.MapRoute("Admin-admin-create", "admin/admin-management/create/", new { controller = "Admin", action = "CreateAdmin" });

			context.MapRoute("Admin-category", "admin/category/", new { controller = "Category", action = "Index", area = "Admin" });
			context.MapRoute("Admin-category-create", "admin/category/create", new { controller = "Category", action = "CreateCategory", area = "Admin" });
			context.MapRoute("Admin-category-delete", "admin/category/delete/{category_id}", new { controller = "Category", action = "DeleteCategory", area = "Admin", category_id = UrlParameter.Optional});
			context.MapRoute("Admin-category-edit", "admin/category/edit/{category_id}", new { controller = "Category", action = "EditCategory", area = "Admin", category_id = UrlParameter.Optional });

			context.MapRoute("Admin-order", "admin/order/", new { controller = "Order", action = "Index" });
			context.MapRoute("Admin-order-edit", "admin/order/edit/{order_id}", new { controller = "Order", action = "EditOrder" });
			context.MapRoute("Admin-order-delete", "admin/order/delete/{order_id}", new { controller = "Order", action = "DeleteOrder" });


			context.MapRoute("Admin-product", "admin/product/", new { controller = "Product", action = "Index" });
			context.MapRoute("Admin-product-create", "admin/product/create/", new { controller = "Product", action = "CreateProduct", order_id = UrlParameter.Optional });
			context.MapRoute("Admin-product-edit", "admin/product/edit/{product_id}", new { controller = "Product", action = "EditProduct", product_id = UrlParameter.Optional });
			context.MapRoute("Admin-product-delete", "admin/product/delete/{product_id}", new { controller = "Product", action = "DeleteProduct", product_id = UrlParameter.Optional });

			context.MapRoute("Admin-home", "admin/", new { controller = "Home", action = "Index" });

      context.MapRoute(
          "Admin_default",
          "Admin/{controller}/{action}/{id}",
          new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
    }
  }
}