using Calobye.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Calobye.Services
{
	public class ProductService
	{
		private static CalobyeDB calobyeDB = new CalobyeDB();

		public static List<PRODUCT> searchProducts(string searchKey)
		{
			if (searchKey == null)
			{
				return calobyeDB.PRODUCTs.ToList();
			}
			return calobyeDB.PRODUCTs.Where(m => m.TITLE.Trim().ToLower().Contains(searchKey.Trim().ToLower())).ToList();
		}

		public static PRODUCT searchProduct(string searchKey)
		{
			return calobyeDB.PRODUCTs.First(m => m.TITLE.ToLower().Contains(searchKey.ToLower()));
		}

		public static PRODUCT getProductBySlug(string slug)
		{
			return calobyeDB.PRODUCTs.FirstOrDefault(m => m.SLUG.Equals(slug));
		}

		public static PRODUCT getProductByID(string ID)
		{
			return calobyeDB.PRODUCTs.FirstOrDefault(m => m.ID.Equals(ID));
		}

		public static List<PRODUCT> getProductsByCategoryID(string categoryID)
		{
			return calobyeDB.PRODUCTs.Where(m => m.CATEGORY_ID.Equals(categoryID)).ToList();
		}

		public static List<PRODUCT> getProductsByCategorySlugHasChild(string categorySlug)
		{
			var category = CategoryService.getCategoryBySlug(categorySlug);
			List<PRODUCT> products = new List<PRODUCT>();

			if (category != null)
			{
				products = getProductsByCategoryID(category.ID);
				if (category.PARENT_ID == null)
				{
					var categoriesChild = CategoryService.getCategoriesChildByCategoryParentID(category.ID);

					if (categoriesChild != null)
					{
						foreach (var item in categoriesChild)
						{
							var prod = getProductsByCategoryID(item.ID);
							if (prod != null)
							{
								products.AddRange(prod);
							}
						}
					}
				}
			}

			return products;
		}

		public static List<PRODUCT> getFeaturedProducts()
		{
			return getProductsByCategorySlugHasChild("san-pham-noi-bat");
		}

		public static List<PRODUCT> getNewProducts(int amount)
		{
			return calobyeDB.PRODUCTs.OrderByDescending(m => m.TIME_CREATE).Take(amount).ToList();
		}

		public static List<PRODUCT> getProducts()
		{
			return calobyeDB.PRODUCTs.ToList();
		}

		public static List<PRODUCT> getSpecialProduct(int amount)
		{
			var products = getProducts();
			var output = new List<PRODUCT>();

			if (products != null && products.Count > 0)
			{
				Random rd = new Random();
				for (int i = 0; i < amount; ++i)
				{
					output.Add(products.ElementAt(rd.Next(0, products.Count())));
				}
			}

			return output;
		}

		public static void updateProduct(PRODUCT product)
		{
			var prod = calobyeDB.PRODUCTs.SingleOrDefault(m => m.ID.Equals(product.ID));
			prod.THUMBNAIL = product.THUMBNAIL;
			prod.PRICE = product.PRICE;
			prod.CATEGORY_ID = product.CATEGORY_ID;
			prod.TITLE = product.TITLE;
			prod.SLUG = product.SLUG;
			prod.DESCRIPTION = product.DESCRIPTION;

			calobyeDB.SaveChanges();
		}

		public static bool isExistProductTitle(string title)
		{
			var product = getProducts();
			return product.Exists(m => m.TITLE.Equals(title));
		}
		public static bool isExistProductTitleButProductID(string title, string product_id)
		{
			var product = getProducts();
			return product.Exists(m => m.TITLE.Equals(title) && !m.ID.Equals(product_id));
		}

		public static bool isExistProductSlug(string slug)
		{
			var product = getProducts();
			return product.Exists(m => m.SLUG.Equals(slug));
		}

		public static bool isExistProductSlugButProductID(string slug, string product_id)
		{
			var product = getProducts();
			return product.Exists(m => m.SLUG.Equals(slug) && !m.ID.Equals(product_id));
		}

		public static ServiceResult<string> deleteProductByID(string id)
		{
			var prod = getProductByID(id);
			var orderDetail = calobyeDB.ORDER_DETAILS.Where(m => m.PRODUCT_ID.Equals(id)).ToList();


			if (prod == null)
			{
				return new ServiceResult<string>
				{
					isError = true,
					message = $"Not found product \"{prod.TITLE}\"."
				};
			}

			if(orderDetail != null)
			{	
				calobyeDB.ORDER_DETAILS.RemoveRange(orderDetail);
			}

			calobyeDB.PRODUCTs.Remove(prod);
			calobyeDB.SaveChanges();
			return new ServiceResult<string>
			{
				isError = false,
				message = $"Delete product \"{prod.TITLE}\" success"
			};
		}

		public static void createProduct(PRODUCT prod)
		{
			calobyeDB.PRODUCTs.Add(prod);
			calobyeDB.SaveChanges();
		}
	}
}