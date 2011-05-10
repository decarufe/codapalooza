using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Codapalooza.Services
{
	public class PaypalService
	{
		private const string PaypalUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
		//private const string PaypalUrl = "https://www.paypal.com/cgi-bin/webscr";
		private const string VerifiedCode = "VERIFIED";

		public bool PaymentIsCompleted(HttpRequestBase request)
		{
			var webRequest = (HttpWebRequest)WebRequest.Create(PaypalUrl);

			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";
			var param = request.BinaryRead(HttpContext.Current.Request.ContentLength);
			var strRequest = Encoding.ASCII.GetString(param);
			strRequest += "&cmd=_notify-validate";
			webRequest.ContentLength = strRequest.Length;

			var streamOut = new StreamWriter(webRequest.GetRequestStream(), Encoding.ASCII);
			streamOut.Write(strRequest);
			streamOut.Close();
			var streamIn = new StreamReader(webRequest.GetResponse().GetResponseStream());
			string strResponse = streamIn.ReadToEnd();
			streamIn.Close();

			if (strResponse == VerifiedCode)
			{
				//check the payment_status is Completed
				//check that txn_id has not been previously processed
				//check that receiver_email is your Primary PayPal email
				//check that payment_amount/payment_currency are correct
				//process payment

				var queryString = HttpUtility.ParseQueryString(strRequest);
				var paymentStatus = queryString["payment_status"];

				if (paymentStatus.Equals("Completed"))
					return true;
			}

			return false;
		}
	}
}