using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using DouYin.DownLoader.Common;
using DouYin.DownLoader.Services;
using DouYin.DownLoader.ViewModels;
using DouYin.DownLoader.Views;
using Microsoft.Extensions.Configuration;
using System.IO;
using Jint;
using FlyleafLib;

namespace DouYin.DownLoader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            var config = builder.Build();
            Constant.SetCookie(config["Cookie"]!);
            Constant.SetFilePath(config["FilePath"]!);
            Ioc.Default.ConfigureServices(
                new ServiceCollection()

                .AddHttpClient()
                .AddTransient<MainViewModel>()
                .AddTransient<MainView>()


                .AddTransient<HomeViewModel>()
                .AddTransient<NoteViewModel>()
                .AddTransient<SettingViewModel>()
                  .AddTransient<SearchViewModel>()
                .AddScoped<IDouYinDownlaodService, DouYinDownlaodService>()
                .AddSingleton<NavigationService>()
                .BuildServiceProvider()); ;
            InitializeComponent();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            FlyleafLib.Engine.Start(new EngineConfig()
            {
                FFmpegPath = @$"{AppDomain.CurrentDomain.BaseDirectory}\FFmpeg",
                FFmpegDevices = false,    // Prevents loading avdevice/avfilter dll files. Enable it only if you plan to use dshow/gdigrab etc.

#if RELEASE
                FFmpegLogLevel      = FFmpegLogLevel.Quiet,
                LogLevel            = LogLevel.Quiet,

#else
                FFmpegLogLevel = FFmpegLogLevel.Warning,
                LogLevel = LogLevel.Debug,
                LogOutput = ":debug",
                //LogOutput         = ":console",
                //LogOutput         = @"C:\Flyleaf\Logs\flyleaf.log",                
#endif

                //PluginsPath       = @"C:\Flyleaf\Plugins",

                UIRefresh = false,    // Required for Activity, BufferedDuration, Stats in combination with Config.Player.Stats = true
                UIRefreshInterval = 250,      // How often (in ms) to notify the UI
                UICurTimePerSecond = true,     // Whether to notify UI for CurTime only when it's second changed or by UIRefreshInterval
            });
            Ioc.Default.GetRequiredService<MainView>().Show();
        }

    }

}
