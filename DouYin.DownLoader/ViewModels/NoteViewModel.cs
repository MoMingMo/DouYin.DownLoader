using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using DouYin.DownLoader.Common.Models;
using DouYin.DownLoader.Services;
using CommunityToolkit.Mvvm.Messaging;
using DouYin.DownLoader.Common;

namespace DouYin.DownLoader.ViewModels
{
    public partial class NoteViewModel : ViewModelBase
    {
        private readonly IDouYinDownlaodService _douYinDownlaodService;
        [ObservableProperty]
        private string _url = "https://www.douyin.com/user/MS4wLjABAAAAAk_GG0VItn8-7TcD_o4a9FV44d6zrYO4LDpBEU_wIr8?vid=7350636142548536628";
        [ObservableProperty]
        private ICollection<VideoItem> _videoItems;

        public NoteViewModel(IDouYinDownlaodService douYinDownlaodService)
        {
            VideoItems = new List<VideoItem>();
            _douYinDownlaodService = douYinDownlaodService;
        }
        private string _userId;
        private long _maxCursor = 0;
        private int hasMore = 1;
        [RelayCommand]
        private async Task GetData()
        {
            VideoItems = new List<VideoItem>();
            ExtraUserId(Url);
            await GetAwemeList();
        }
        [RelayCommand]
        private async Task Download()
        {
            foreach (var item in VideoItems)
            {
                await _douYinDownlaodService.DownLoadVideoAsync(item);
                await Task.Delay(500);
            }
            WeakReferenceMessenger.Default.Send(new ShowMessage($"本次下载完成共下载{VideoItems.Count}条记录"));
            await Task.CompletedTask;
        }
        [RelayCommand]
        private async Task ScrollToBottom(object parameter)
        {
            ScrollViewer scrollViewer = (parameter as ScrollViewer)!;
            if (scrollViewer!.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                if (hasMore == 1)
                    await GetAwemeList();
                else
                {
                    WeakReferenceMessenger.Default.Send(new ShowMessage("已经获取到全部数据"));
                    return;
                }
            }
            await Task.CompletedTask;
        }
        private async Task GetAwemeList()
        {
            var result = await _douYinDownlaodService.GetAuthorVideos(_userId, _maxCursor);
            if (result.status_code != 0)
            {
                WeakReferenceMessenger.Default.Send(new ShowMessage("获取数据异常"));
            }
            _maxCursor = result.max_cursor;
            hasMore = result.has_more;
            var videos = result.aweme_list!.Select(x => new VideoItem
            {
                AwemeType=x.aweme_type,
                AwemeId = x.aweme_id,
                NikName = x.author!.nickname,
                UId = x.author.uid,
                SecUid = x.author.sec_uid,
                Title = x.preview_title!,
                VideoTag = string.Join(" ", x.video_tag!.Select(y => y.tag_name).ToList()),
                VideoCover = x.video!.cover!.url_list![0],
                Video =x.video!.play_addr!.url_list![0],
                Images= x.images?.Select(x => x.url_list[0])?.ToList()
            }).ToList();
            VideoItems = VideoItems.Concat(videos).ToList();
        }
        private void ExtraUserId(string url)
        {
            string pattern = @"/([^/?]+)(?:\?|$)";

            Match match = Regex.Match(url, pattern);

            if (match.Success)
            {
                _userId = match.Groups[1].Value;
            }
        }


    }
}
