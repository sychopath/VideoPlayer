using Microsoft.Data.SqlClient;
using MyVideoPlayer.DB;

namespace MyVideoPlayer.Authentication.Login
{
    /// <summary>
    /// The PasswordProvider class provides methods for retrieving hashed password from DB.
    /// </summary>
    public class PasswordProvider : IPasswordProvider
    {
        private IDatabaseConnection databaseConnection;
        public PasswordProvider(IDatabaseConnection dbConnection)
        {
            databaseConnection = dbConnection;
        }

        public string GetHashedPassword(string username)
        {
            var connectionObject = databaseConnection.OpenConnection();

            // Query to retrieve hashed password for the entered username
            string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";

            using (SqlCommand command = new SqlCommand(query, connectionObject))
            {
                command.Parameters.AddWithValue("@Username", username);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["PasswordHash"].ToString();
                    }
                }
            }

            return null;
        }
    }
}
