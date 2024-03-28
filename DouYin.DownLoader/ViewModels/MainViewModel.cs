using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Windows;
using DouYin.DownLoader.Common;
using DouYin.DownLoader.Common.Models;

namespace DouYin.DownLoader.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly Common.NavigationService _navigationService;
        [ObservableProperty]
        private ICollection<MenuBar>? _menuBars;
        [ObservableProperty]
        private ViewModelBase? currentViewModel;
        [ObservableProperty]
        private string message;
        public MainViewModel(Common.NavigationService navigationService)
        {
            _navigationService = navigationService;
            _menuBars = new List<MenuBar>()
                {
                    new MenuBar(){ Icon="Home",Title="视频下载",NameSpace=nameof(HomeViewModel)},
                    new MenuBar(){ Icon="Note",Title="主页下载",NameSpace=nameof(NoteViewModel)},
                    new MenuBar(){ Icon="NotebookOutline",Title="备忘录",NameSpace=nameof(HomeViewModel)},
                    new MenuBar(){ Icon="Cog",Title="设置",NameSpace=nameof(SettingViewModel)},
                };
            _navigationService.CurrentViewModelChanged += () => CurrentViewModel = _navigationService.CurrentViewModel;
            _navigationService.NavigateTo<HomeViewModel>();
            WeakReferenceMessenger.Default.Register<ShowMessage>(this, (_, m) =>
            {
                Message = m.message;
            });
        }
        [RelayCommand]
        private void Navigate(MenuBar menu)
        {
            switch (menu.NameSpace)
            {
                case nameof(HomeViewModel):
                    _navigationService.NavigateTo<HomeViewModel>();
                    break;
                case nameof(NoteViewModel):
                    _navigationService.NavigateTo<NoteViewModel>();
                    break;
                case nameof(SettingViewModel):
                    _navigationService.NavigateTo<SettingViewModel>();
                    break;
                default:
                    _navigationService.NavigateTo<HomeViewModel>();
                    break;
            }
        }
      


    }
}
