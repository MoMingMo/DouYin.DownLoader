using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DouYin.DownLoader.Common.Models;

namespace DouYin.DownLoader.Common
{
    public class NavigationService
    {
  
        private ViewModelBase? currentViewModel;
        public ViewModelBase? CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }
        public event Action? CurrentViewModelChanged;
        public void NavigateTo<T>() where T:ViewModelBase
            => CurrentViewModel =Ioc.Default.GetService<T>();

    }
}
