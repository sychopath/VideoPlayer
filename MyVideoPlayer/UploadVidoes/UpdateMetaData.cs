using Microsoft.Data.SqlClient;
using MyVideoPlayer.DB;
using MyVideoPlayer.FetchVideos;
using MyVideoPlayer.Log;
using MyVideoPlayer.Models;
using System;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace MyVideoPlayer.UploadVidoes
{
    /// <summary>
    /// The UpdateMetaData class provides methods for updating metadata Azure SQL database with relevant information.
    /// </summary>
    public class UpdateMetaData : IUpdateMetaData
    {
        private IDatabaseConnection databaseConnection;
        public UpdateMetaData(IDatabaseConnection dbConnection)
        {
            databaseConnection= dbConnection;
        }

        public int Update(MetaData metaData)
        {
            try
            {
                var connectionObject = databaseConnection.OpenConnection();

                var vidoeID = Guid.NewGuid().ToString();
                
                var currentUser = CurrentUserInfo.UserName;

                // Convert BitmapImage to byte array
                byte[] imageBytes = File.ReadAllBytes(metaData.thumbnailFilePath);

                var version = SetFileVersion(metaData.title);

                // Query to retrieve hashed password for the entered username
                string query = "INSERT INTO VideoMetaData (VideoID, Title, ThumbNail, UserName, URL, Version) VALUES (@VideoID,@fileName,@imageBytes,@currentUser,@azureResponse,@Version)";


                using (SqlCommand command = new SqlCommand(query, connectionObject))
                {

                    command.Parameters.AddWithValue("@VideoID", vidoeID);
                    command.Parameters.AddWithValue("@fileName", metaData.title);
                    command.Parameters.AddWithValue("@imageBytes", imageBytes);
                    command.Parameters.AddWithValue("@currentUser", currentUser);
                    command.Parameters.AddWithValue("@azureResponse", metaData.uri.OriginalString);
                    command.Parameters.AddWithValue("@Version", version);


                    // Execute the query
                    command.ExecuteNonQuery();
                }

                return 0;
            }
            catch (Exception ex)
            {
                Logger.Log($"Exception occured in UpdateMetaData.Update method : {ex.Message}");
                return -1;
            }
        }

        private static int SetFileVersion(string title)
        {
            int version = GetVideos.videoList.FindAll(_ => _.Title== title && _.UserName == CurrentUserInfo.UserName).Count + 1;
            return version;
        }

    }
}
