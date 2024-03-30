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
        private string? _userId;
        private long _maxCursor = 0;
        private long _commentMaxCursor = 0;
        private int _hasMore = 1;
        private int _commenthasMore = 1;
        private VideoItem _currenVideo;
        private readonly IDouYinDownlaodService _douYinDownlaodService;
        [ObservableProperty]
        private string _url = "https://www.douyin.com/user/MS4wLjABAAAAyxCER8pqCDPMQN1DqoyaURTcR1lovESyiyZR6dYOaK9bdSR4X7mQQ8R7NRhZ4Lbp?vid=7344231935977327882";
        [ObservableProperty]
        private ICollection<VideoItem> _videoItems;
        [ObservableProperty]
        private bool _isOpen = false;
        [ObservableProperty]
        private ICollection<CommentItem> _commentItems;

        public NoteViewModel(IDouYinDownlaodService douYinDownlaodService)
        {
            VideoItems = new List<VideoItem>();
            CommentItems = new List<CommentItem>();
            _douYinDownlaodService = douYinDownlaodService;
        }

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
            WeakReferenceMessenger.Default.Send(new NotifyMessage($"开始下载", true));
            foreach (var item in VideoItems)
            {
                await _douYinDownlaodService.DownLoadVideoAsync(item);
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

            await _douYinDownlaodService.DownLoadVideoAsync(video);
            await Task.Delay(500);
            await Task.CompletedTask;
        }
        [RelayCommand]
        private async Task ShowComment(VideoItem video)
        {
            _currenVideo = video;
            IsOpen = true;
            var result = await _douYinDownlaodService.GetAwemeCommentList(video.AwemeId!);
            if (result.status_code != 0)
            {
                WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
            }
            _commentMaxCursor = result.cursor;
            CommentItems = result.comments!
                .Where(x => x.content_type != 2)
                .Select(x => new CommentItem
                {
                    Text = x.text,
                    DiggCount = x.digg_count,
                    NickName = x.user.nickname,
                    CreateTime = DateTimeOffset.FromUnixTimeSeconds(x.create_time).DateTime
                }).OrderByDescending(x => x.DiggCount)
                .ToList();
        }
        [RelayCommand]
        private async Task CommentDataGridScrollToBottom(object parameter)
        {
            ScrollViewer scrollViewer = (parameter as ScrollViewer)!;
            if (scrollViewer.ScrollableHeight!=0&&scrollViewer!.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                if (_commenthasMore == 0)
                {
                    WeakReferenceMessenger.Default.Send(new NotifyMessage("已经获取到全部数据"));
                    return;
                }
                var result = await _douYinDownlaodService.GetAwemeCommentList(_currenVideo.AwemeId!);
                if (result.status_code != 0)
                {
                    WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
                }
                _commentMaxCursor = result.cursor;

                var currentComments = result.comments!
                .Where(x => x.content_type != 2)
                .Select(x => new CommentItem
                {
                    Text = x.text,
                    DiggCount = x.digg_count,
                    NickName = x.user.nickname,
                    CreateTime = DateTimeOffset.FromUnixTimeSeconds(x.create_time).DateTime
                }).OrderByDescending(x => x.DiggCount)
                .ToList();
                CommentItems = CommentItems.Concat(currentComments).ToList();


            }
            await Task.CompletedTask;
        }
        private async Task GetAwemeList()
        {
            WeakReferenceMessenger.Default.Send(new NotifyMessage("开始请求数据", true));
            var result = await _douYinDownlaodService.GetAuthorVideos(_userId!, _maxCursor);
            if (result.status_code != 0)
            {
                WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
            }
            _maxCursor = result.max_cursor;
            _hasMore = result.has_more;
            var videos = result.aweme_list!.Select(x => new VideoItem
            {
                AwemeType = x.aweme_type,
                AwemeId = x.aweme_id,
                NikName = x.author!.nickname,
                UId = x.author.uid,
                SecUid = x.author.sec_uid,
                Title = x.preview_title!,
                VideoTag = string.Join(" ", x.video_tag!.Select(y => y.tag_name).ToList()),
                VideoCover = x.video!.cover!.url_list![0],
                Video = x.video!.play_addr!.url_list![0],
                Images = x.images?.Select(x => x.url_list[0])?.ToList(),
                CollectCount = x.statistics!.collect_count,
                ShareCount = x.statistics!.share_count,
                CommentCount = x.statistics!.comment_count,
                DiggCount = x.statistics!.digg_count,
            }).ToList();
            VideoItems = VideoItems.Concat(videos).ToList();
            WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据成功", false));
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
