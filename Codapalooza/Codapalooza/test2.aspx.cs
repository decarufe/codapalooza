using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Codapalooza
{
  public partial class test : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      var ipHostEntry = Dns.GetHostEntry("mail.decarufel.net");
      var ipAddress = ipHostEntry.AddressList.First();

      lblMessage.Text = ipAddress.ToString();
    }
  }
}