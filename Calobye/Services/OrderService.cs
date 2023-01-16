using Calobye.Controllers;
using Calobye.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Services
{
	public class OrderService
	{

		private static CalobyeDB calobyeDB = new CalobyeDB();

		public static List<OrderHistory> getHistoryOrder()
		{
			List<OrderHistory> products = new List<OrderHistory>();
			var orderDetails = calobyeDB.ORDER_DETAILS.ToList();
			var auth = AuthService.getAuth();

			foreach(var i in orderDetails)
			{
				var order = calobyeDB.THE_ORDER.FirstOrDefault(m => m.ID.Equals(i.ID) && m.CUSTOMER_ID.Equals(auth.ID));
				if(order != null)
				{
					products.Add(new OrderHistory
					{
						Product = ProductService.getProductByID(i.PRODUCT_ID),
						Amount = i.AMOUNT,
						Total = (double)i.TOTAL,
						DateCreate = order.DATE_ORDER,
						DeliveryStatus = order.DELIVERY_STATUS,
						isPaid = order.IS_PAID
					});
				}
			}
			return products.OrderByDescending(m => m.DateCreate).ToList();
		}

		public static List<OrderManagement> ordersManagement()
		{
			var result = new List<OrderManagement>();
			OrderManagement orderManage = null;

			var orders = calobyeDB.THE_ORDER.ToList();
			
			foreach(var item in orders)
			{
				var orderDetail = OrderDetailService.getOrderDetailByID(item.ID);
				var customer = CustomerService.getCustomerByID(item.CUSTOMER_ID);
				if(orderDetail != null && customer != null)
				{
					orderManage = new OrderManagement()
					{
						ID = item.ID,
						listOrderDetail = orderDetail,
						customer = customer,
						dateOrder = item.DATE_ORDER,
						deliveryStatus = item.DELIVERY_STATUS,
						isPaid = item.IS_PAID,
						totalOrder = OrderDetailService.getTotalOrderDetailByID(item.ID),
					};
					result.Add(orderManage);
				}
			}

			return result;
		}

		public static OrderManagement getOrderManagementByID(string ID)
		{
			return ordersManagement().SingleOrDefault(m => m.ID.Equals(ID));
		}

		public static ServiceResult<string> updateOrder(string orderID, bool deliveryStatus, bool isPaid)
		{
			
			var order = calobyeDB.THE_ORDER.FirstOrDefault(m => m.ID.Equals(orderID));
			if(order == null)
			{
				return new ServiceResult<string>
				{
					isError = true,
					message = "Not found order."
				};
			}
			order.DELIVERY_STATUS = deliveryStatus;
			order.IS_PAID = isPaid;
			calobyeDB.SaveChanges();

			return new ServiceResult<string>
			{
				isError = false,
				message = "Update order success."
			};
		}

		public static ServiceResult<string> deleteOrderByID(string orderID)
		{
			var order = calobyeDB.THE_ORDER.FirstOrDefault(m => m.ID.Equals(orderID));
			if (order == null)
			{
				return new ServiceResult<string>
				{
					isError = true,
					message = $"Not found order to delete."
				};
			}
			var lisrOrderDetail = calobyeDB.ORDER_DETAILS.Where(m => m.ID.Equals(orderID)).ToList();
			if (lisrOrderDetail != null)
			{
				calobyeDB.ORDER_DETAILS.RemoveRange(lisrOrderDetail);
			}
			calobyeDB.THE_ORDER.Remove(order);
			
			calobyeDB.SaveChanges();
			return new ServiceResult<string>
			{
				isError = false,
				message = $"Delete order success."
			};
		}
	
		public static THE_ORDER getOrderByID(string ID)
		{
			return calobyeDB.THE_ORDER.FirstOrDefault(m => m.ID.Equals(ID));
		}
	}
}