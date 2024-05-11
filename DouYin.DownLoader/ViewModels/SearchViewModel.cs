using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Controls;
using DouYin.DownLoader.Common.Models;
using DouYin.DownLoader.Services;
using CommunityToolkit.Mvvm.Messaging;
using DouYin.DownLoader.Common;
using MiniExcelLibs;
using System.IO;
using FlyleafLib.MediaPlayer;
using System.Windows;
using FlyleafLib;

namespace DouYin.DownLoader.ViewModels
{
    public partial class SearchViewModel : ViewModelBase
    {
        private string? _userId;
        private long _maxCursor = 0;
        private long? _commentMaxCursor = 0;
        private int? _hasMore = 1;
        private int? _commenthasMore = 1;
        private VideoItem _currenVideo;
        private readonly IDouYinDownlaodService _douYinDownlaodService;
        [ObservableProperty]
        private string _keyWord = "";
        [ObservableProperty]
        private ICollection<VideoItem> _videoItems;
        [ObservableProperty]
        private bool _isOpen = false;
        [ObservableProperty]
        private CommentList _commentList;
        [ObservableProperty]
        private Player _flPlayer;
        [ObservableProperty]
        private ICollection<string> _comments;
        [ObservableProperty]
        private Visibility isShowHotCommentCard = Visibility.Hidden;
        public SearchViewModel(IDouYinDownlaodService douYinDownlaodService)
        {
            VideoItems = new List<VideoItem>();
            Comments = new List<string>();
            var config = new Config();


            FlPlayer = new Player(config);
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
        private async Task MouseDown(VideoItem video)
        {
            IsShowHotCommentCard = Visibility.Visible;
            Comments = new List<string>();
            FlPlayer.Commands.Open.Execute(video.Video);
            try
            {
                var result = await _douYinDownlaodService.GetAwemeCommentListAsync(video.AwemeId!);
                if (result.status_code != 0)
                {
                    WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
                }
                var allComments = result.comments!
                   .Where(x => x.content_type != 2)
                   //.OrderByDescending(x => x.digg_count)
                   .Select(x => new { x.digg_count, x.text })
                   .ToList();

                _ = Task.Run(async () =>
                {
                    int i = 0;
                    while (result.has_more == 1 && i < 20)
                    {
                        result = await _douYinDownlaodService.GetAwemeCommentListAsync(video.AwemeId!, result.cursor);
                        if (result.status_code != 0)
                        {
                            WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
                        }
                        var comments = result.comments!
                                        .Where(x => x.content_type != 2)
                                        //.OrderByDescending(x => x.digg_count)
                                        .Select(x => new { x.digg_count, x.text })
                                        .ToList();
                        allComments.AddRange(comments);
                        i++;
                    }
                    Comments = allComments.OrderByDescending(x => x.digg_count).Take(20).Select(x => x.text).ToList();
                    IsShowHotCommentCard = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                IsShowHotCommentCard = Visibility.Hidden;
                WeakReferenceMessenger.Default.Send(new NotifyMessage("获取评论异常"));
            }

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

            await _douYinDownlaodService.DownLoadVideoAsync(video, KeyWord);
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
                    CreateTime = DateTimeOffset.FromUnixTimeSeconds(x.create_time!.Value).DateTime
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
                    CreateTime = DateTimeOffset.FromUnixTimeSeconds(x.create_time!.Value).DateTime
                })
                .OrderByDescending(x => x.DiggCount)
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
            _ = Task.Run(async () =>
            {
                List<VideoItem> videos = new List<VideoItem>(0);
                WeakReferenceMessenger.Default.Send(new NotifyMessage("开始请求数据", true));
                if (string.IsNullOrWhiteSpace(KeyWord))
                {
                    var discoverResult = await _douYinDownlaodService.GetDouYinDiscoverAsync();
                    if (discoverResult.status_code != 0)
                    {
                        WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
                    }
                    var videosTasks = discoverResult.cards!.Where(x => x.aweme_info.author != null).Select(async x =>
                    {
                        var aw = await _douYinDownlaodService.GetAwemeDetailAsync($"https://www.douyin.com/user/MS4wLjABAAAACirq2YgCDHFt4JJLwl1l5Hj4WpThkGSm8uKQJY7a2hU?modal_id={x.aweme_info.aweme_id}");

                        return new VideoItem
                        {
                            AwemeType = x.aweme_info.aweme_type,
                            AwemeId = x.aweme_info.aweme_id,
                            NikName = x.aweme_info.author?.nickname,
                            UId = x.aweme_info.author?.uid,
                            Title = x.aweme_info.desc!,

                            VideoCover = x.aweme_info.video!.cover!.url_list![0],
                            Video = aw.aweme_detail.video!.play_addr!.url_list![0],
                            CollectCount = x.aweme_info.statistics!.collect_count,
                            ShareCount = x.aweme_info.statistics!.share_count,
                            CommentCount = x.aweme_info.statistics!.comment_count,
                            DiggCount = x.aweme_info.statistics!.digg_count,
                            CreateAt = x.aweme_info.create_time,
                        };
                    }).ToList();
                    videos = (await Task.WhenAll(videosTasks)).ToList();
                    VideoItems = VideoItems.Concat(videos).DistinctBy(x => x.AwemeId)
                        //.OrderByDescending(x => x.DiggCount)
                        .ToList();
                    WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据成功", false));
                    return;
                }
                var result = await _douYinDownlaodService.GetSearchVideosAsync(KeyWord, _maxCursor);
                if (result.status_code != 0)
                {
                    WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
                }
                _maxCursor = result.cursor;
                _hasMore = result.has_more;
                videos = result.data!.Select(x => new VideoItem
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
                    CreateAt = x.aweme_info.create_time,
                }).ToList();
                VideoItems = VideoItems.Concat(videos).DistinctBy(x => x.AwemeId)
                    //.OrderByDescending(x => x.DiggCount)
                    .ToList();
                WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据成功", false));
            });
        }


    }
}
