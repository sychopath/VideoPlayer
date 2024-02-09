using Autofac;
using Autofac.Core;
using MyVideoPlayer.Log;
using System;
using System.IO;
using System.Windows;

namespace MyVideoPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static string logFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Log";
        static string filePath = Path.Combine(logFolderPath, "log.txt");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = DependencyConfig.Configure();

            Logger.Initialize(filePath);

            Logger.Log("Log file Initialized");

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();

        }
    }
}
