using System;
using System.Collections.Generic;
using System.Text;

namespace BaGet.Core.Authentication
{
    public interface IUserService
    {
        bool IsValidUser(string username, string password);
    }
}
