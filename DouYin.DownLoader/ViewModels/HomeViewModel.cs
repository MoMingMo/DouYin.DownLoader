﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DouYin.DownLoader.Common;
using DouYin.DownLoader.Common.Models;
using DouYin.DownLoader.Services;
using FlyleafLib;
using FlyleafLib.MediaPlayer;
using System;

namespace DouYin.DownLoader.ViewModels
{
    public partial class HomeViewModel : ViewModelBase
    {
        public HomeViewModel(IDouYinDownlaodService douYinDownlaodService)
        {
            Player = new Player(new Config 
            {
                
            });
            _douYinDownlaodService = douYinDownlaodService;
        }
        [ObservableProperty]
        private string _url = "https://www.douyin.com/user/MS4wLjABAAAAiXeL8UUfi0KfVrjbpc2LJKSGiPXEBomMz5i_DCbDsSYXhCJ6PZm9c7DUE1KCQ2cy?modal_id=7349499329259818275&vid=7348013111326084404";

        [ObservableProperty]
        private VideoItem? _douYin;
        private readonly IDouYinDownlaodService _douYinDownlaodService;
        [ObservableProperty]
        private Player player;
        [RelayCommand]
        private async Task Download()
        {

            try
            {
                var awemeDetail = await _douYinDownlaodService.GetAwemeDetailAsync(Url);
                if (awemeDetail.status_code != 0)
                {
                    WeakReferenceMessenger.Default.Send(new NotifyMessage("请求接口异常"));
                    return;
                }
                
                var author = awemeDetail!.aweme_detail!.author!;
                var video = awemeDetail!.aweme_detail!.video!;
                var nikName = author.nickname!;
                var uid = author.uid!;
                var aweme_id = awemeDetail.aweme_detail.aweme_id!;
                var aweme_type = awemeDetail.aweme_detail.aweme_type;
                var statistics = awemeDetail.aweme_detail.statistics!;
                var basePath = $"{AppContext.BaseDirectory}\\{nikName}_{uid}\\";
                var tags = awemeDetail.aweme_detail.video_tag!.Select(x => x.tag_name)!;

                var imageUlrs = awemeDetail.aweme_detail.images?.Select(x => x.url_list[0])?.ToList();

                DouYin = new VideoItem
                {
                    Title = awemeDetail.aweme_detail.preview_title!,
                    Avatar = awemeDetail.aweme_detail.author!.avatar_thumb!.url_list![0],
                    NikName = nikName,
                    Video = awemeDetail.aweme_detail.video!.play_addr!.url_list![0],
                    VideoCover = awemeDetail.aweme_detail.video!.origin_cover!.url_list![0],
                    CollectCount = statistics.collect_count,
                    CommentCount = statistics.comment_count,
                    ShareCount = statistics.share_count,
                    DiggCount = statistics.digg_count,
                    VideoTag = string.Join(' ', tags),
                    UId = uid!,
                    AwemeId = aweme_id!,
                    AwemeType = aweme_type,
                    Images=imageUlrs,
                };
                Player.Commands.Open.Execute(DouYin.Images);
                _ = _douYinDownlaodService.DownLoadVideoAsync(DouYin);
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(new NotifyMessage("下载数据异常"));
            }
        }


    }
}
