using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calobye.Models
{
	public class ServiceResult<T>
	{
		public ServiceResult()
		{
			this.data = default (T);
			this.message = null;
			this.isError = false;
		}

		public T data { get; set; }

		public string message { get; set; }
		public bool isError { set; get; }
	}
}