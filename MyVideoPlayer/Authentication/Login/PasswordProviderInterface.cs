using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoPlayer.Authentication.Login
{
    public interface IPasswordProvider
    {
        public string GetHashedPassword(string username);
    }
}
