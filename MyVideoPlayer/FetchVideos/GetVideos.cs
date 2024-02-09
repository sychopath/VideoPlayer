using Microsoft.Data.SqlClient;
using MyVideoPlayer.DB;
using MyVideoPlayer.Log;
using MyVideoPlayer.Models;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MyVideoPlayer.FetchVideos
{
    /// <summary>
    /// Static class to Fetch Video Info from meta data.
    /// </summary>
    public class GetVideos : IGetVideos
    {
        private IDatabaseConnection databaseConnection;

        public static List<VideoInfo> videoList;
        public GetVideos(IDatabaseConnection dbConnection)
        {
            databaseConnection = dbConnection;
        }

        public List<VideoInfo> Fetch()
        {
            videoList = null;
            try
            {
                videoList = new List<VideoInfo>();

                var connectionObject = databaseConnection.OpenConnection();

                string query = "Select * From VideoMetaData";

                using (SqlCommand command = new SqlCommand(query, connectionObject))
                {

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var videoInfo = new VideoInfo();

                            videoInfo.Title = reader["Title"].ToString();
                            videoInfo.FilePath = reader["URL"].ToString();
                            videoInfo.Thumbnail = (byte[])reader["ThumbNail"];
                            videoInfo.UserName = reader["UserName"].ToString();
                            videoInfo.Version = (int)reader["Version"];
                            videoList.Add(videoInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured while fetching videos !!");
                Logger.Log("Exception occured in GetVideos.Fetch :" + ex.Message);
            }

            return videoList;
            
        }

    }
}
