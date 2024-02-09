using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoPlayer.Authentication.Login
{
    public interface IAuthenticationHelper
    {
        public bool AuthenticateUser(string username, string password);
    }
}

