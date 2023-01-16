using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Calobye.Models;


namespace Calobye.Services
{
	public class CategoryService
	{
		private static readonly CalobyeDB calobyeDB = new();

		public static List<CATEGORY> getCategoryParent()
		{
			return calobyeDB.CATEGORies.Where(m => m.PARENT_ID == null).ToList();
		}

		public static CATEGORY getCategoryBySlug(string slug)
		{
			return calobyeDB.CATEGORies.FirstOrDefault(m => m.SLUG.Equals(slug));
		}

		public static CATEGORY getCategoryByID(string categoryID)
		{
			return calobyeDB.CATEGORies.FirstOrDefault(m => m.ID.Equals(categoryID));
		}

		public static List<CATEGORY> getCategoies()
		{
			return calobyeDB.CATEGORies.ToList();
		}

		public static List<CATEGORY> getCategoriesChildByCategoryParentID(string categoryParentID)
		{
			return calobyeDB.CATEGORies.Where(m => m.PARENT_ID.Equals(categoryParentID)).ToList();
		}

		public static bool isExistCategoryTitle(string title)
		{
			return getCategoies().Exists(m => m.TITLE == title);
		}

		public static bool isExistCategoryTitleButCategoryID(string title, string categoryID)
		{
			return getCategoies().Exists(m => m.TITLE == title && m.ID!= categoryID);
		}

		public static bool isExistCategorySlugButCategoryID(string title, string categoryID)
		{
			return getCategoies().Exists(m => m.SLUG == title && m.ID != categoryID);
		}

		public static bool isExistCategorySlug(string slug)
		{
			return getCategoies().Exists(m => m.SLUG == slug);
		}

		public static ServiceResult<string> createCategory(CATEGORY category)
		{
			try{
				calobyeDB.CATEGORies.Add(category);
				calobyeDB.SaveChanges();
				return new ServiceResult<string>
				{
					isError = false,
					message = "Create new category success."
				};
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return new ServiceResult<string>
				{
					isError = true,
					message = $"ERROR: {e.Message}"
				};
			}
		}

		public static ServiceResult<string> deleteCategory(string categoryID)
		{
			CATEGORY category = getCategoryByID(categoryID);

			if(category != null)
			{
				if(category.PARENT_ID != null)
				{
					List<PRODUCT> prod = ProductService.getProductsByCategoryID(category.ID);
					if(prod.Count == 0)
					{
						calobyeDB.CATEGORies.Remove(category);
						calobyeDB.SaveChanges();
						return new ServiceResult<string>
						{
							isError = false,
							message = $"Delete Category \"{category.TITLE}\" success."
						};
					}
					else
					{
						return new ServiceResult<string>
						{
							isError = true,
							message = $"Can't delete category \"{category.TITLE}\" because it's being used by products."
						};
					}
				}
				else
				{
					var categoriesChild = CategoryService.getCategoriesChildByCategoryParentID(category.ID);
					var prod = ProductService.getProductsByCategoryID(category.ID);
					
					if(categoriesChild.Count == 0)
					{
						if(prod.Count <= 0)
						{
							calobyeDB.CATEGORies.Remove(category);
							calobyeDB.SaveChanges();
							return new ServiceResult<string>
							{
								isError = false,
								message = $"Delete Category \"{category.TITLE}\" success."
							};
						}
						else
						{
							return new ServiceResult<string>
							{
								isError = true,
								message = $"Can't delete category \"{category.TITLE}\" because it's being used by products."
							};

						}
					}
					else
					{
						bool subcateHasProd = false;
						foreach(var i in categoriesChild)
						{
							var p = ProductService.getProductsByCategoryID(i.ID);
							if (p.Count > 0)
								subcateHasProd = true;
						}

						if(subcateHasProd)
						{
							return new ServiceResult<string>
							{
								isError = true,
								message = $"Can't delete category \"{category.TITLE}\" because its subcategory being used by products."
							};
						}

						calobyeDB.CATEGORies.RemoveRange(categoriesChild);
						calobyeDB.CATEGORies.Remove(category);
						calobyeDB.SaveChanges();
						return new ServiceResult<string>
						{
							isError = false,
							message = $"Delete catefory \"{category.TITLE}\" and its subcategory success. "
						};
					}
				}
			}
			
			return new ServiceResult<string>
			{
				isError = true,
				message = $"Not found Category \"{category.TITLE}\" to delete. "
			};

			
		}

		public static ServiceResult<string> updateCategory(CATEGORY ca)
		{
			var c = calobyeDB.CATEGORies.FirstOrDefault(m => m.ID == ca.ID);

			if (c == null)
			{
				return new ServiceResult<string>
				{
					isError = true,
					message = "Not found category."
				};
			}

			c.TITLE = ca.TITLE;
			c.SLUG = ca.SLUG;
			calobyeDB.SaveChanges();

			return new ServiceResult<string>
			{
				isError = false,
				message = "Save category success."
			};
		}
	}
}