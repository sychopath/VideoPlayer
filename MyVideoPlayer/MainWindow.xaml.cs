using MyVideoPlayer.Authenctication;
using MyVideoPlayer.Authentication.Login;
using MyVideoPlayer.DB;
using MyVideoPlayer.FetchVideos;
using MyVideoPlayer.UploadVidoes;
using MyVideoPlayer.ViewModel;
using System;
using System.Windows;

namespace MyVideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow(IAuthenticationHelper helper, IUpdateMetaData metadataUpdateService, IGetVideos getVideosService)
        {
            InitializeComponent();

            // Arranging Login User control
            loginUserControl.authenticationHelper = helper;
            loginUserControl.LoginSuccessful += OnLoginSuccessful;
            loginUserControl.GuestLogin += OnGuestLogin;

            // Arranging Load Vidoes User control
            loadVideos.getVideosService = getVideosService;
            loadVideos.UploadVideos += OnUploadVideoButtonClick;
            loadVideos.LoadLoginPage += OnLoadLoginPageClick;
            loadVideos.LoadAllVideos();
            

            addVideos.updateMetaData = metadataUpdateService;
            addVideos.GoBackRequested += AddVideos_GoBackRequested;
            
            // Create an instance of the ViewModel
            viewModel = new MainViewModel();

            // Set ViewModel as the DataContext of MainWindow
            DataContext = viewModel;
        }

        #region private methods

        private void AddVideos_GoBackRequested(object? sender, EventArgs e)
        {
            addVideos.Visibility= Visibility.Collapsed;
            loadVideos.Visibility = Visibility.Visible;
            loadVideos.ReLoadVideos();
        }

        private void OnLoginSuccessful(object sender, EventArgs e)
        {
            loginUserControl.Visibility = Visibility.Collapsed;
            loadVideos.Visibility = Visibility.Visible;
            viewModel.Username = CurrentUserInfo.UserName;

            loadVideos.SetLoginButtonVisibility(false);
        }

        private void OnGuestLogin(object sender, EventArgs e)
        {
            loginUserControl.Visibility = Visibility.Collapsed;
            loadVideos.Visibility = Visibility.Visible;
            
            loadVideos.SetLoginButtonVisibility(true);
        }

        private void OnUploadVideoButtonClick(object sender, EventArgs e)
        {
            loadVideos.Visibility = Visibility.Collapsed;
            addVideos.Visibility = Visibility.Visible;
        }

        private void OnLoadLoginPageClick(object sender, EventArgs e)
        {
            loadVideos.Visibility = Visibility.Collapsed;
            loginUserControl.Visibility = Visibility.Visible;
        }

        #endregion
    }
}
