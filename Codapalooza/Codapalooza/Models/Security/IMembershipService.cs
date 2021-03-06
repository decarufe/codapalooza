﻿using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace Codapalooza.Models.Security
{
  public interface IMembershipService
  {
    int MinPasswordLength { get; }

    bool ValidateUser(string userName, string password);
    MembershipCreateStatus CreateUser(string userName, string password, string email);
    bool ChangePassword(string userName, string oldPassword, string newPassword);
    IEnumerable<MembershipUser> GetAllUsers();
    MembershipUser GetUserByName(string userName);
    void DeleteUser(string userName);
  }
}