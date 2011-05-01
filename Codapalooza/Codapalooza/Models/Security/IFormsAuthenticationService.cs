namespace Codapalooza.Models.Security
{
  public interface IFormsAuthenticationService
  {
    void SignIn(string userName, bool createPersistentCookie);
    void SignOut();
  }
}