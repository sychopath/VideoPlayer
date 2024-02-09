using Microsoft.Win32;
using MyVideoPlayer.Log;
using MyVideoPlayer.UploadVidoes;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MyVideoPlayer
{
    /// <summary>
    /// Interaction logic for AddVideos.xaml
    /// </summary>
    public partial class AddVideos : UserControl
    {

        public event EventHandler<string> VideoUploaded;

        public event EventHandler GoBackRequested;

        private string selectedFilePath;

        private string thumbnailFilePath;

        private BitmapImage image;

        public IUpdateMetaData updateMetaData { get; set; }

        string thumbnailFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\thumbnails";

        public AddVideos()
        {
            InitializeComponent();
        }

        #region click events
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Video Files (*.mp4;*.mkv;*.avi)|*.mp4;*.mkv;*.avi|All Files (*.*)|*.*",
                Title = "Select a Video File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                selectedFilePath = openFileDialog.FileName;
                DisplayFileDetails(selectedFilePath);
            }
        }

        private async void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    string title = titleTextBox.Text.ToString();
                    Upload_Btn.IsEnabled = false;
                    Browse_Btn.IsEnabled = false;
                    titleTextBox.Text = "";
                    titleTextBox.IsEnabled = false;

                    // Use BackgroundWorker to perform the upload asynchronously
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.WorkerReportsProgress = true;
                    worker.DoWork += (sender, args) => UploadVideo(selectedFilePath, title);
                    //worker.ProgressChanged += UploadVideoProgress;
                    worker.RunWorkerCompleted += UploadVideoCompleted;

                    // Start the upload process and show progress
                    progressBar.Visibility = Visibility.Visible;
                    await Task.Run(() => worker.RunWorkerAsync());
                }
                else
                {
                    MessageBox.Show("Please select a video file before uploading.", "Upload Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error occured while uploading video");
            }
        }

        private void LoadHomePageBtn_Click(object sender, RoutedEventArgs e)
        {
            GoBackRequested.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region helper methods
        private void UploadVideo(string sourceFilePath, string title)
        {
            try
            {
                var metaData = UploadBlob.Upload(sourceFilePath, thumbnailFilePath, title);
                if (metaData != null)
                {
                    updateMetaData.Update(metaData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading video: {ex.Message}", "Upload Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UploadVideoCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Update UI after upload completion on the UI thread
            Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Collapsed;
                MessageBox.Show("Video upload complete!", "Upload Complete", MessageBoxButton.OK, MessageBoxImage.Information);

                Upload_Btn.IsEnabled = true;
                Browse_Btn.IsEnabled = true;
                titleTextBox.IsEnabled = true;
                fileDetailsPanel.Visibility = Visibility.Collapsed;
            });
        }

        private void DisplayFileDetails(string filePath)
        {
            fileNameTextBlock.Text = $"Name: {System.IO.Path.GetFileName(filePath)}";
            fileSizeTextBlock.Text = $"Size: {GetFileSize(filePath)}";

            // Display thumbnail
            thumbnailImage.Source = GenerateThumbnail(filePath);

            // Show the file details panel
            fileDetailsPanel.Visibility = Visibility.Visible;
        }

        private string GetFileSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            long sizeInBytes = fileInfo.Length;

            // Convert bytes to KB
            double sizeInKB = sizeInBytes / 1024.0;

            return $"{sizeInKB:F2} KB";
        }

        private BitmapImage GenerateThumbnail(string filePath)
        {
            try
            {
                thumbnailFilePath = System.IO.Path.Combine(thumbnailFolderPath, $"{Path.GetFileNameWithoutExtension(filePath) + $"{Guid.NewGuid().ToString()}"}.jpg");

                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

                ffMpeg.GetVideoThumbnail(filePath, thumbnailFilePath);

                image = new BitmapImage(new Uri(thumbnailFilePath));

                return image;
            }
            catch (Exception ex)
            {
                Logger.Log("Error while generating thumbnail");
            }
            return null;
        }

        //private void UploadVideoProgress(object sender, ProgressChangedEventArgs e)
        //{
        //    // Update the progress bar with the reported progress value
        //    progressBar.Value = e.ProgressPercentage;
        //}

        #endregion

    }
}
