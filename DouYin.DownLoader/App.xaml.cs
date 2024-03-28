﻿using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using DouYin.DownLoader.Common;
using DouYin.DownLoader.Services;
using DouYin.DownLoader.ViewModels;
using DouYin.DownLoader.Views;

namespace DouYin.DownLoader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddHttpClient()
                .AddTransient<MainViewModel>()
                .AddTransient<MainView>()


                .AddTransient<HomeViewModel>()
                .AddTransient<NoteViewModel>()
                .AddTransient<SettingViewModel>()

                .AddScoped<IDouYinDownlaodService, DouYinDownlaodService>()
                .AddSingleton<NavigationService>()
                .BuildServiceProvider()); ;
            InitializeComponent();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Ioc.Default.GetRequiredService<MainView>().Show();
        }

    }

}
