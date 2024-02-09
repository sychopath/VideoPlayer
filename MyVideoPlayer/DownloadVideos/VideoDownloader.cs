using MyVideoPlayer.Log;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace MyVideoPlayer.DownloadVideos
{
    /// <summary>
    /// The VideoDownloader class provides methods for downloading videos asynchronously.
    /// </summary>
    internal class VideoDownloader
    {
        public static async Task DownloadVideoAsync(string blobUrl, string filename)
        {
            string downloadsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string filePath = Path.Combine(downloadsFolderPath, $"{filename}.mp4");

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(new Uri(blobUrl)))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                            {
                                using (FileStream fileStream = File.Create(filePath))
                                {
                                    await contentStream.CopyToAsync(fileStream);
                                    fileStream.Close();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Failed to download video. Status code: {response.StatusCode}");
                        }
                    }
                }

                Logger.Log("Video downloaded successfully!");
                MessageBox.Show("File Downloaded Successfully");
            }
            catch (Exception ex)
            {
                Logger.Log($"VideoDownloader : Error downloading video: {ex.Message}");
                MessageBox.Show($"Error downloading video !!");
            }
        }
    }

}

