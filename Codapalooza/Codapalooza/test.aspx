<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="System.Net" %> 
<%@ Import Namespace="System.Net.Mail" %> 
 
<script language="C#" runat="server">

  protected void Page_Load(object sender, EventArgs e)
  {
    //create the mail message 
    MailMessage mail = new MailMessage();

    //set the addresses 
    mail.From = new MailAddress("eric@decarufel.net");
    mail.To.Add("edecarufel@pyxis-tech.com");

    //set the content 
    mail.Subject = "This is an email";
    mail.Body = "This is from system.net.mail using C sharp with smtp authentication.";
    //send the message 
    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
    smtp.EnableSsl = true;
    smtp.Port = 465;

    NetworkCredential Credentials = new NetworkCredential("eric.decarufel@gmail.com", "$Amix0214!20");
    smtp.Credentials = Credentials;
    smtp.Send(mail);
    lblMessage.Text = "Mail Sent";
  }

</script> 
<html> 
<body> 
    <form id="Form2" runat="server"> 
        <asp:Label id="lblMessage" runat="server"></asp:Label> 
    </form> 
</body> 
</html>