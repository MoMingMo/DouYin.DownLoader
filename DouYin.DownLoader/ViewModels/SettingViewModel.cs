using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using DouYin.DownLoader.Common.Models;

namespace DouYin.DownLoader.ViewModels
{
   public partial class SettingViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string? _cookies;
    }
}
