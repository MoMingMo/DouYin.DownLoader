using CommunityToolkit.Mvvm.Messaging;
using System.Diagnostics;
using System.Windows;
using DouYin.DownLoader.ViewModels;
using static DouYin.DownLoader.ViewModels.MainViewModel;

namespace DouYin.DownLoader.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
            menuListBoxt.SelectionChanged +=(_,_)=> MenuToggleButton.IsChecked=false;
        }


      
        private void ColorZone_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }

        private void mixiMizedBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void maxiMizedBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else 
                this.WindowState = WindowState.Maximized;
        }

        private void closedBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
