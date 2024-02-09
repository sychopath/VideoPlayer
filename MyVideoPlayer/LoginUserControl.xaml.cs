using MyVideoPlayer.Authenctication;
using MyVideoPlayer.Authentication.Login;
using MyVideoPlayer.DB;
using MyVideoPlayer.Log;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MyVideoPlayer
{
    /// <summary>
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public IAuthenticationHelper authenticationHelper { get; set; }

        public LoginUserControl()
        {
            InitializeComponent();
        }

        public event EventHandler LoginSuccessful;

        public event EventHandler GuestLogin;

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredUsername = usernameTextBox.Text;
            string enteredPassword = passwordBox.Password;

            // Authentication
            if (IsValidCredentials(enteredUsername, enteredPassword))
            {
                CurrentUserInfo.UserName = enteredUsername;
                OnLoginSuccessful();
                Logger.Log("LoginButton_Click: Logged In");
            }
            else
            {
                Logger.Log("LoginButton_Click: Invalid credentials");
                MessageBox.Show("Invalid username or password. Please try again.", "Authentication Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidCredentials(string username, string password)
        {
            return authenticationHelper.AuthenticateUser(username, password);
        }
        

        private void ContinueWithoutLogin_Click(object sender, RoutedEventArgs e)
        {
            OnGuestLogin();
        }

        private void OnLoginSuccessful()
        {
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }

        private void OnGuestLogin()
        {
            GuestLogin?.Invoke(this, EventArgs.Empty);
        }
    }
}

