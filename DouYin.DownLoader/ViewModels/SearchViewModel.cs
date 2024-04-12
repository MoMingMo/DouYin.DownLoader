using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using DouYin.DownLoader.Common.Models;
using DouYin.DownLoader.Services;
using CommunityToolkit.Mvvm.Messaging;
using DouYin.DownLoader.Common;
using MiniExcelLibs;
using System.IO;

namespace DouYin.DownLoader.ViewModels
{
    public partial class SearchViewModel : ViewModelBase
    {
        private string? _userId;
        private long _maxCursor = 0;
        private long _commentMaxCursor = 0;
        private int _hasMore = 1;
        private int _commenthasMore = 1;
        private VideoItem _currenVideo;
        private readonly IDouYinDownlaodService _douYinDownlaodService;
        [ObservableProperty]
        private string _keyWord = "灵魂附体";
        [ObservableProperty]
        private ICollection<VideoItem> _videoItems;
        [ObservableProperty]
        private bool _isOpen = false;
        [ObservableProperty]
        private CommentList _commentList;

        public SearchViewModel(IDouYinDownlaodService douYinDownlaodService)
        {
            VideoItems = new List<VideoItem>();
            _douYinDownlaodService = douYinDownlaodService;
        }
        
      
        [RelayCommand]
        private async Task GetData()
        {
            _maxCursor = 0;
            VideoItems = new List<VideoItem>();
            await GetAwemeList();
        }
        [RelayCommand]
        private async Task Download()
        {
            WeakReferenceMessenger.Default.Send(new NotifyMessage($"开始下载", true));
            foreach (var item in VideoItems)
            {
                await _douYinDownlaodService.DownLoadVideoAsync(item, KeyWord);
                await Task.Delay(500);
            }
            WeakReferenceMessenger.Default.Send(new NotifyMessage($"本次下载完成共{VideoItems.Count}条记录", false));
            await Task.CompletedTask;
        }
        [RelayCommand]
        private async Task ScrollToBottom(object parameter)
        {
            ScrollViewer scrollViewer = (parameter as ScrollViewer)!;
            if (scrollViewer!.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                if (_hasMore == 1)
                    await GetAwemeList();
                else
                    WeakReferenceMessenger.Default.Send(new NotifyMessage("已经获取到全部数据"));
            }
            await Task.CompletedTask;
        }
        [RelayCommand]
        private async Task DownloadOne(VideoItem video)
        {

            await _douYinDownlaodService.DownLoadVideoAsync(video,KeyWord);
            await Task.Delay(500);
            await Task.CompletedTask;
        }
        [RelayCommand]
        private async Task ShowComment(VideoItem video)
        {
            _currenVideo = video;
            IsOpen = true;
            var result = await _douYinDownlaodService.GetAwemeCommentListAsync(video.AwemeId!);
            if (result.status_code != 0)
            {
                WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
            }
            _hasMore = result.has_more;
            _commentMaxCursor = result.cursor;
            var comments = result.comments!
                .Where(x => x.content_type != 2)
                .Select(x => new CommentItem
                {
                    Text = x.text,
                    DiggCount = x.digg_count,
                    NickName = x.user.nickname,
                    CreateTime = DateTimeOffset.FromUnixTimeSeconds(x.create_time).DateTime
                }).OrderByDescending(x => x.DiggCount)
                .ToList();
            CommentList = new CommentList
            {
                CommentItems = comments,
                HasMore = _hasMore == 1
            };
        }

        [RelayCommand]
        private async Task NextPage()
        {
            var result = await _douYinDownlaodService.GetAwemeCommentListAsync(_currenVideo.AwemeId!, _commentMaxCursor);
            if (result.status_code != 0)
            {
                WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
            }
            if (result.comments is null) return;
            _commentMaxCursor = result.cursor;
            await Console.Out.WriteLineAsync("_commentMaxCursor" + result.cursor);
            var comments = result.comments?
                .Where(x => x.content_type != 2)
                .Select(x => new CommentItem
                {
                    Text = x.text,
                    DiggCount = x.digg_count,
                    NickName = x.user.nickname,
                    CreateTime = DateTimeOffset.FromUnixTimeSeconds(x.create_time).DateTime
                }).OrderByDescending(x => x.DiggCount)
                .ToList();
            var data = CommentList.CommentItems;
            CommentList = new CommentList
            {
                CommentItems = data!.Concat(comments!).OrderByDescending(x => x.DiggCount).ToList(),
                HasMore = result.has_more == 1
            };
        }

        [RelayCommand]
        private async Task ExportComments()
        {

            var fileName = _currenVideo.AwemeId + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var directory = Constant.FilePath ?? "" + "excel\\";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            await MiniExcel.SaveAsAsync(directory + fileName, CommentList.CommentItems);
            WeakReferenceMessenger.Default.Send(new NotifyMessage($"保存成功：{Constant.FilePath ?? AppDomain.CurrentDomain.BaseDirectory + directory + fileName}", false));
        }

        private async Task GetAwemeList()
        {
            WeakReferenceMessenger.Default.Send(new NotifyMessage("开始请求数据", true));
            var result = await _douYinDownlaodService.GetSearchVideosAsync(KeyWord, _maxCursor);
            if (result.status_code != 0)
            {
                WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
            }
            _maxCursor = result.cursor;
            _hasMore = result.has_more;
            var videos = result.data!.Select(x => new VideoItem
            {
                AwemeType = x.aweme_info.aweme_type,
                AwemeId = x.aweme_info.aweme_id,
                NikName = x.aweme_info.author!.nickname,
                UId = x.aweme_info.author.uid,
                Title = x.aweme_info.desc!,
              
                VideoCover = x.aweme_info.video!.cover!.url_list![0],
                Video = x.aweme_info.video!.play_addr!.url_list![0],
                CollectCount = x.aweme_info.statistics!.collect_count,
                ShareCount = x.aweme_info.statistics!.share_count,
                CommentCount = x.aweme_info.statistics!.comment_count,
                DiggCount = x.aweme_info.statistics!.digg_count,
            }).ToList();
            VideoItems = VideoItems.Concat(videos).DistinctBy(x=>x.AwemeId).OrderByDescending(x=>x.DiggCount).ToList();
            WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据成功", false));
        }
        

    }
}
