using Microsoft.Data.SqlClient;
using MyVideoPlayer.Authentication.Login;
using MyVideoPlayer.DB;
using MyVideoPlayer.Log;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MyVideoPlayer.Authenctication
{
    /// <summary>
    /// The AuthenticationHelper class provides methods for user authentication.
    /// </summary>
    public class AuthenticationHelper: IAuthenticationHelper
    {
        private IPasswordProvider passwordProvider;

        // Constructor to inject dependencies
        public AuthenticationHelper(IPasswordProvider provider)
        {
            passwordProvider = provider;
        }

        public bool AuthenticateUser(string username, string password)
        {
            Logger.Log("Inside AuthenticateUser");
            // Hash the entered password
            string hashedPassword = HashPassword(password);

            string hashValueFromDB = passwordProvider.GetHashedPassword(username);

            // Compare hashed passwords
            if (string.Equals(hashValueFromDB, hashedPassword))
            {
                // Authentication successful
                Logger.Log("AuthenticateUser : returning true");
                return true;
            }

            Logger.Log("AuthenticateUser : returning false");
            return false;
        }

        /// <summary>
        /// Computes the SHA-256 hash of the given password.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The hashed password as a hexadecimal string.</returns>
        internal string HashPassword(string password)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                    // Convert the hashed bytes to a string
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < hashedBytes.Length; i++)
                    {
                        // append hexa-decimal representation of hashed byte
                        builder.Append(hashedBytes[i].ToString("x2"));
                    }

                    return builder.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Exception occured inside AuthenticationHelper HashPassword : '{ex.Message}'");
            }

            return string.Empty;
        }

    }
}
