using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Calobye.Models
{
	public class Gmail
	{

		public string to { get; set; }

		public string subject { get; set; }

		public string body { get; set; }

		public void send()
		{
			MailMessage mc = new MailMessage(System.Configuration.ConfigurationManager.AppSettings["Email"].ToString(), this.to)
			{
				Subject = this.subject,
				Body = this.body,
				IsBodyHtml = false,
			};

			SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587){
				Timeout = 1000000,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
			};

			NetworkCredential nc = new NetworkCredential("dangduyduc1908@gmail.com", "qnejxwidnyylwkzm");

			smtp.UseDefaultCredentials = false;
			smtp.Credentials = nc;
			smtp.Send(mc);
		}
	}
}