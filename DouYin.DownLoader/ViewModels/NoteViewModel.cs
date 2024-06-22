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
using FlyleafLib.MediaPlayer;
using FlyleafLib;
using System.Windows;

namespace DouYin.DownLoader.ViewModels
{
    public partial class NoteViewModel : ViewModelBase
    {
        private string? _userId;
        private long? _maxCursor = 0;
        private long? _commentMaxCursor = 0;
        private int? _hasMore = 1;
        private int? _commenthasMore = 1;
        private VideoItem _currenVideo;
        private readonly IDouYinDownlaodService _douYinDownlaodService;
        [ObservableProperty]
        private string _url = "https://www.douyin.com/user/MS4wLjABAAAA4M7BBMvfTzE-ijnMRqTV3tWNMy8mGiRlF-iqEgnQ3VlIk33Yc79bgjbkv4BrDbnF?vid=7353439971585346867";
        [ObservableProperty]
        private ICollection<VideoItem> _videoItems;
        [ObservableProperty]
        private bool _isOpen = false;
        [ObservableProperty]
        private CommentList _commentList;
        [ObservableProperty]
        private ICollection<MixItem> _mixItems;
        [ObservableProperty]
        private bool _isMixShow;
        [ObservableProperty]
        private Player _flPlayer;
        [ObservableProperty]
        private ICollection<string> _comments;
        [ObservableProperty]
        private Visibility isShowHotCommentCard = Visibility.Hidden;
        public NoteViewModel(IDouYinDownlaodService douYinDownlaodService)
        {
            VideoItems = new List<VideoItem>();
            Comments = new List<string>();
            var config = new Config();


            FlPlayer = new Player(config);
            _douYinDownlaodService = douYinDownlaodService;
        }
        [RelayCommand]
        private async Task GetMix()
        {
            ExtraUserId(Url);
            MixItems = new List<MixItem>();
            var result = await _douYinDownlaodService.GetAwemeMixListAsync(_userId!, 0);

            MixItems = result.mix_infos?.Select(x => new MixItem
            {
                MixId = x?.mix_id,
                MixCorver = x?.cover_url.url_list[0],
                PlayCount = x?.statis.play_vv,
                MixTitle = x?.mix_name,
                Chapter = $"更新至{x?.statis.updated_to_episode}集"

            })?.ToList() ?? new List<MixItem>();

            IsMixShow = MixItems.Any();
        }
        [RelayCommand]
        private async Task DownloadMix(MixItem mixItem)
        {
            _ = Task.Run(async () =>
             {
                 try
                 {
                     var result = await _douYinDownlaodService.GetAwemeMixAwemesAsync(mixItem.MixId, 0);
                     var videoItems = result.aweme_list.Select(x => new VideoItem
                     {
                         AwemeId = x.aweme_id,
                         AwemeType = x.aweme_type,
                         NikName = x.author.nickname,
                         UId = x.author.uid,
                         MixName = mixItem.MixTitle,
                         VideoCover = x.video!.cover!.url_list![0],
                         Video = x.video!.play_addr!.url_list![0],
                         Images = x.images?.Select(x => x.url_list[0])?.ToList(),

                     }).ToList();
                     while (result.has_more == 1)
                     {
                         result = await _douYinDownlaodService.GetAwemeMixAwemesAsync(mixItem.MixId, result.cursor);
                         var awemes = result.aweme_list.Select(x => new VideoItem
                         {
                             AwemeId = x.aweme_id,
                             AwemeType = x.aweme_type,
                             NikName = x.author.nickname,
                             UId = x.author.uid,
                             VideoCover = x.video!.cover!.url_list![0],
                             Video = x.video!.play_addr!.url_list![0],
                             Images = x.images?.Select(x => x.url_list[0])?.ToList(),
                         }).ToList();
                         videoItems.AddRange(awemes);
                     }
                     WeakReferenceMessenger.Default.Send(new NotifyMessage($"开始下载合集" + mixItem.MixTitle, true));
                     foreach (var item in videoItems)
                     {
                         await _douYinDownlaodService.DownLoadVideoAsync(item);
                         await Task.Delay(500);
                     }
                     WeakReferenceMessenger.Default.Send(new NotifyMessage($"本次下载完成共{videoItems.Count}条记录", false));
                     await Task.CompletedTask;
                 }
                 catch (Exception ex)
                 {
                     WeakReferenceMessenger.Default.Send(new NotifyMessage($"下载合集异常{ex.Message}", false));
                 }

             });
        }
        [RelayCommand]
        private async Task GetData()
        {
            _maxCursor = 0;
            VideoItems = new List<VideoItem>();
            ExtraUserId(Url);
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
                    return;
                }
                var allComments = result.comments!
                   .Where(x => x.content_type != 2)
                   .OrderByDescending(x => x.digg_count)
                   .Select(x => new { x.digg_count, x.text })
                   .ToList();

                _ = Task.Run(async () =>
                {
                    int i = 0;
                    while (result.has_more == 1 && i < 5)
                    {
                        result = await _douYinDownlaodService.GetAwemeCommentListAsync(video.AwemeId!, result.cursor);
                        if (result.status_code != 0)
                        {
                            WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
                        }
                        var comments = result.comments!
                                        .Where(x => x.content_type != 2)
                                        .OrderByDescending(x => x.digg_count)
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
                WeakReferenceMessenger.Default.Send(new NotifyMessage($"获取评论异常{ex.Message}"));
            }


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
                    CreateTime = DateTimeOffset.FromUnixTimeSeconds(x.create_time.Value).DateTime
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

        private Task GetAwemeList()
        {
            _ = Task.Run(async () =>
            {
                WeakReferenceMessenger.Default.Send(new NotifyMessage("开始请求数据", true));
                var result = await _douYinDownlaodService.GetAuthorVideosAsync(_userId!, _maxCursor);
                if (result.status_code != 0)
                {
                    WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据异常"));
                }
                _maxCursor = result.max_cursor;
                _hasMore = result.has_more;
                var videos = result.aweme_list!.Select(x =>
               new VideoItem
               {
                   AwemeType = x.aweme_type,
                   AwemeId = x.aweme_id,
                   NikName = x.author!.nickname,
                   UId = x.author.uid,
                   Title = x.preview_title!,
                   VideoTag = string.Join(" ", x.video_tag!.Select(y => y.tag_name).ToList()),
                   VideoCover = x.video!.cover!.url_list![0],
                   Video = x.video!.play_addr!.url_list![0],
                   Images = x.images?.Select(x => x.url_list[0])?.ToList(),
                   CollectCount = x.statistics!.collect_count,
                   ShareCount = x.statistics!.share_count,
                   CommentCount = x.statistics!.comment_count,
                   DiggCount = x.statistics!.digg_count,
                   CreateAt = x.create_time,
               }).ToList();
                VideoItems = VideoItems.Concat(videos).DistinctBy(x => x.AwemeId).OrderByDescending(x => x.DiggCount).ToList();
                WeakReferenceMessenger.Default.Send(new NotifyMessage("获取数据成功", false));
            });
            return Task.CompletedTask;
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
