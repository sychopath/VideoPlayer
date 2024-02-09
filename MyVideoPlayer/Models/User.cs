using System;

namespace MyVideoPlayer.Models
{
    internal class User
    {
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        // Constructor with parameters to set properties
        public User(string username, string password)
        {
            UserID = Guid.NewGuid();
            Username = username;
            Password = password;
        }

    }
}
