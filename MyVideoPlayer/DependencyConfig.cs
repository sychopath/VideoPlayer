using Autofac;
using MyVideoPlayer.Authenctication;
using MyVideoPlayer.Authentication.Login;
using MyVideoPlayer.DB;
using MyVideoPlayer.FetchVideos;
using MyVideoPlayer.UploadVidoes;

namespace MyVideoPlayer
{
    /// <summary>
    /// The DependencyConfig class provides methods to configure and register dependencies using Autofac.
    /// </summary>
    internal static class DependencyConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DatabaseConnection>().As<IDatabaseConnection>().SingleInstance();
            builder.RegisterType<PasswordProvider>().As<IPasswordProvider>();
            builder.RegisterType<AuthenticationHelper>().As<IAuthenticationHelper>();
            builder.RegisterType<UpdateMetaData>().As<IUpdateMetaData>();
            builder.RegisterType<GetVideos>().As<IGetVideos>();

            builder.RegisterType<MainWindow>();

            return builder.Build();
        }
    }
}
