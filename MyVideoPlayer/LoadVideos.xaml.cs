using MyVideoPlayer.DB;
using MyVideoPlayer.DownloadVideos;
using MyVideoPlayer.FetchVideos;
using MyVideoPlayer.Log;
using MyVideoPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyVideoPlayer
{
    /// <summary>
    /// Interaction logic for LoadVideos.xaml
    /// </summary>
    public partial class LoadVideos : UserControl
    {
        private Dictionary<Tuple<string, string>, int> videoVersionInfo ;
        public ObservableCollection<VideoInfo> Videos { get; set; }

        public event EventHandler UploadVideos;

        public event EventHandler LoadLoginPage;

        public IGetVideos getVideosService{ get; set; }

        #region public member functions

        public LoadVideos()
        {
            InitializeComponent();
            Videos = new ObservableCollection<VideoInfo>();
            videoVersionInfo = new Dictionary<Tuple<string, string>, int>();
            mediaPlayerStackPanel.Visibility = Visibility.Hidden;
            mediaPlayerControlStackPanel.Visibility = Visibility.Hidden;
            videoListBox.ItemsSource = Videos;
        }

        public void SetLoginButtonVisibility(bool visibility)
        {
            if (visibility)
                LoginBtn.Visibility = Visibility.Visible;
            else
                LoginBtn.Visibility = Visibility.Collapsed;
        }

        public void ReLoadVideos()
        {
            try
            {
                Videos.Clear();
                LoadAllVideos();
                searchTextBox.Text = string.Empty;
                videoListBox.ItemsSource = Videos;
            }
            catch (Exception ex)
            {
                Logger.Log($"Excpetion occured while Loading videos : {ex.Message}");
                MessageBox.Show($"Error loading videos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region private member functions

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadLoginPage.Invoke(this, EventArgs.Empty);
        }

        public void LoadAllVideos()
        {
            try
            {
                var videoInfoList = getVideosService.Fetch();

                foreach (var videoInfo in videoInfoList)
                {
                    Videos.Add(videoInfo);
                    Tuple<string, string> tuple = new Tuple<string, string>(videoInfo.UserName, videoInfo.Title);


                    if(videoVersionInfo.ContainsKey(tuple))
                    {
                        if (videoVersionInfo[tuple] < videoInfo.Version)
                        {
                            videoVersionInfo[tuple] = videoInfo.Version;
                        }
                    }
                    videoVersionInfo[tuple] = videoInfo.Version;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Inside LoadAllVideos : Excpetion occured while Loading videos : {ex.Message}");
                MessageBox.Show($"Error loading videos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.ToLower();

            // Filter videos based on the search term
            var filteredVideos = Videos.Where(video => video.Title.ToLower().Contains(searchTerm)).ToList();

            // Update the ListBox with the filtered videos
            videoListBox.ItemsSource = filteredVideos;
        }

        private void AddVideoButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to the second window content
            if (CurrentUserInfo.UserName != null)
            {
                UploadVideos.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Please login to upload videos ");
            }
        }

        private void VideoListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            VideoInfo selectedVideo = videoListBox.SelectedItem as VideoInfo;
            if (selectedVideo != null)
            {
                NotifyIfNotLatest(selectedVideo);

                myMediaElement.Source = new Uri(selectedVideo.FilePath);
                myMediaElement.Visibility = Visibility.Visible;
                mediaPlayerStackPanel.Visibility = Visibility.Visible;
                mediaPlayerControlStackPanel.Visibility = (Visibility)Visibility.Visible;
                myMediaElement.Play();
            }
        }

        private void NotifyIfNotLatest(VideoInfo selectedVideo)
        {
            var tuple = new Tuple<string,string>(CurrentUserInfo.UserName, selectedVideo.Title);

            if (videoVersionInfo.ContainsKey(tuple))
            {
                if (videoVersionInfo[tuple] > selectedVideo.Version)
                {
                    MessageBox.Show("This is not the latest version of this video !!");
                    Logger.Log("NotifyIfNotLatest : Notified user");
                }
            }

        }

        #region media_player_controls
        // Play the media.
        private void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
        {

            myMediaElement.Play();

            InitializePropertyValues();
        }

        // Pause the media.
        private void OnMouseDownPauseMedia(object sender, MouseButtonEventArgs args)
        {
            myMediaElement.Pause();
        }

        // Stop the media.
        private void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
        {
            myMediaElement.Stop();
            myMediaElement.Close();

            mediaPlayerControlStackPanel.Visibility = Visibility.Collapsed;
            mediaPlayerStackPanel.Visibility = (Visibility)Visibility.Collapsed;
        }

        // Change the volume of the media.
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            myMediaElement.Volume = (double)volumeSlider.Value;
        }

        private void Element_MediaOpened(object sender, EventArgs e)
        {
            timelineSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
        }


        private void Element_MediaEnded(object sender, EventArgs e)
        {
            myMediaElement.Stop();
        }

        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)timelineSlider.Value;

            // The arguments days, hours, minutes, seconds, milliseconds.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            myMediaElement.Position = ts;
        }

        private void InitializePropertyValues()
        {
            myMediaElement.Volume = (double)volumeSlider.Value;
        }

        #endregion

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUserInfo.UserName == null)
            {
                MessageBox.Show("Please login to download videos");
            }
            else
            {
                var selectedItem = (VideoInfo)videoListBox.SelectedItem;
                if (selectedItem != null)
                {
                    MessageBox.Show("Started Downloading File !!");
                    string downloadLink = selectedItem.FilePath;
                    Logger.Log("DownloadButton_Click : Downloading file");
                    Task.Run(() => VideoDownloader.DownloadVideoAsync(downloadLink, selectedItem.Title));
                }
            }
        }


        #endregion
    }

}
