using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Codapalooza.Services
{
	public class PaypalService
	{
		public const string PaypalUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
		public const string PaypalEmailAccount = "test_1305034166_biz@pyxis-tech.com";

		//public const string PaypalUrl = "https://www.paypal.com/cgi-bin/webscr";
		//public const string PaypalEmailAccount = "sales@pyxis-tech.com";

		private const string VerifiedCode = "VERIFIED";

		public bool PaymentIsCompleted(HttpRequestBase request)
		{
			var requestString = GetRequestString(request);
			var webRequest = CreateWebRequest(requestString);
			var paypalResponse = GetPaypalResponse(webRequest, requestString);

			if (paypalResponse == VerifiedCode)
			{			
				var queryString = HttpUtility.ParseQueryString(requestString);
				var paymentStatus = queryString["payment_status"];

				if (paymentStatus.Equals("Completed"))
					return true;
			}

			return false;
		}

		private string GetRequestString(HttpRequestBase request)
		{
			var param = request.BinaryRead(request.ContentLength);
			var requestString = Encoding.ASCII.GetString(param);
			requestString += "&cmd=_notify-validate";

			return requestString;
		}

		private HttpWebRequest CreateWebRequest(string requestString)
		{
			var webRequest = (HttpWebRequest)WebRequest.Create(PaypalUrl);
			webRequest.Method = "POST";
			webRequest.ContentType = "application/x-www-form-urlencoded";
			webRequest.ContentLength = requestString.Length;

			return webRequest;
		}

		private string GetPaypalResponse(HttpWebRequest webRequest, string request)
		{
			using(var streamOut = new StreamWriter(webRequest.GetRequestStream(), Encoding.ASCII))
			{
				streamOut.Write(request);
			}

			using (var streamIn = new StreamReader(webRequest.GetResponse().GetResponseStream()))
			{
				return streamIn.ReadToEnd();
			}
		}
	}
}