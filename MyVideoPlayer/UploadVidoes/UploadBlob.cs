using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MyVideoPlayer.DB;
using MyVideoPlayer.Log;
using MyVideoPlayer.Models;
using System;
using System.Configuration;
using System.IO;
using System.Windows;

namespace MyVideoPlayer.UploadVidoes
{
    /// <summary>
    /// The UploadBlob class provides methods for uploading files to Blob(Azure) database.
    /// </summary>
    internal static class UploadBlob
    {
        private static BlobServiceClient blobServiceClient;

        private static void Initialize()
        {
            if (blobServiceClient == null)
            {
                blobServiceClient = new BlobServiceClient(new Uri(ConfigurationManager.ConnectionStrings["BlobServiceUri"].ConnectionString), new DefaultAzureCredential());
            }
        }

        public static MetaData Upload(string sourceFilePath, string thumbnailFilePath, string title)
        {
            MetaData videoMetaData = null;

            try
            {
                Initialize();

                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(CurrentUserInfo.UserName.ToLower());
                containerClient.CreateIfNotExists(publicAccessType:PublicAccessType.BlobContainer);

                string fileName = Path.GetFileName(sourceFilePath);

                // Get a reference to a blob
                BlobClient blobClient = containerClient.GetBlobClient(fileName);

                // Upload data from the local file
                var azureResponse = blobClient.Upload(sourceFilePath, true);

                if (azureResponse != null)
                {
                    Logger.Log("UploadBlob: Blob uploaded in Azure, updating metadata.");
                    videoMetaData = new MetaData(title, blobClient.Uri, thumbnailFilePath);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Exception occured in UploadBlob.Upload method : {ex.Message}");
                MessageBox.Show($"Error uploading video: {ex.Message}", "Upload Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return videoMetaData;
        }
    }
}
