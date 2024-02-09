using Microsoft.Data.SqlClient;
using MyVideoPlayer.Log;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace MyVideoPlayer.DB
{
    /// <summary>
    /// The DatabaseConnection class provides methods for handling Azure SQL database connection.
    /// </summary>
    public sealed class DatabaseConnection : IDisposable, IDatabaseConnection
    {
        private SqlConnection connection;

        private static readonly DatabaseConnection Connection =  new DatabaseConnection();
        
        public static DatabaseConnection Instance => Connection;

        #region public methods

        public DatabaseConnection()
        {
            try
            {
                connection = new SqlConnection(GetConnectionString().ConnectionString);
            }
            catch(Exception ex)  
            {
                Logger.Log($"Exception occured inside DatabaseConnection constructor : '{ex.Message}'");
            }
        }

        public SqlConnection OpenConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not connect to Azure Database. Check connection or retry !!");
                Logger.Log($"Exception occured inside DatabaseConnection.OpenConnection : '{ex.Message}'");
            }

            return connection;
        }

        public void CloseConnection()
        {
            try
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Exception occured inside DatabaseConnection.CloseConnection : '{ex.Message}'");
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }

        #endregion

        #region private methods

        private static SqlConnectionStringBuilder GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;
            builder.UserID = ConfigurationManager.ConnectionStrings["UserID"].ConnectionString;
            builder.Password = ConfigurationManager.ConnectionStrings["Password"].ConnectionString;
            builder.InitialCatalog = ConfigurationManager.ConnectionStrings["InitialCatalog"].ConnectionString;

            return builder;
        }

        #endregion
    }
}
