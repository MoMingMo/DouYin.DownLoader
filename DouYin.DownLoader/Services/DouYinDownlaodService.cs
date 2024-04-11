﻿using CommunityToolkit.Mvvm.Messaging;
using Jint;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DouYin.DownLoader.Common;
using DouYin.DownLoader.Common.Models;

namespace DouYin.DownLoader.Services
{
    public class DouYinDownlaodService : IDouYinDownlaodService
    {
        private HttpClient _client;

        public DouYinDownlaodService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(Constant.DOU_YING);
            SetHeaders();
        }
        private string ms = string.Empty;
        public void SetHeaders()
        {

            _client.BaseAddress = new Uri("https://www.douyin.com/");
            _client.DefaultRequestHeaders.Add("authority", "www.douyin.com");
            _client.DefaultRequestHeaders.Add("user-agent", Constant.UserAgent);
            _client.DefaultRequestHeaders.Add("referer", "https://www.douyin.com/search/%E7%83%AD%E9%97%A8?publish_time=0&sort_type=0&source=switch_tab&type=video");
            var ms = GetMsToken();
            _client.DefaultRequestHeaders.Add("cookie", string.Format(Constant.Cookie!, ms));
        }
        public async Task<DouYinAwemeDetailApiModel> GetAwemeDetailAsync(string url)
        {
            var modal_id = await ExtractModalId(url);
           
            url = await GenerateRequestParams(string.Format(Constant.AwemeDetailUrl, modal_id), Constant.UserAgent);
            var awemeDetail = await _client.GetFromJsonAsync<DouYinAwemeDetailApiModel>(url);
            return awemeDetail!;

        }
        public async Task<DouYinAwemListApiModel> GetAuthorVideosAsync(string userId, long max_cursor = 0)
        {
            var url = await GenerateRequestParams(string.Format(Constant.AwemeListUrl, userId, max_cursor), Constant.UserAgent);
            var awemeList = await _client.GetFromJsonAsync<DouYinAwemListApiModel>(url);
            return awemeList!;
        }
        public async Task<DouYinAwemeSearchListApiModel> GetSearchVideosAsync(string keyWord, long max_cursor = 0)
        {
            var url = string.Format(Constant.AwemeSearchListUrl, keyWord, max_cursor); ;
            var awemeList = await _client.GetFromJsonAsync<DouYinAwemeSearchListApiModel>(url);
            return awemeList!;
        }
        public async Task<DouYinCommentListApiModel> GetAwemeCommentListAsync(string awemeId, long max_cursor = 0)
        {
            var url = await GenerateRequestParams(string.Format(Constant.AwemeCommenListtUrl, awemeId, max_cursor), Constant.UserAgent);
            var awemeCommentList = await _client.GetFromJsonAsync<DouYinCommentListApiModel>(url);
            return awemeCommentList!;
        }
        public async Task<DouYinAwemeMixListApiModel> GetYinAwemeMixListAsync(string userId, long max_cursor = 0)
        {
            var url = await GenerateRequestParams(string.Format(Constant.AwemeMixListUrl, userId, max_cursor), Constant.UserAgent);
            var awemeMixList = await _client.GetFromJsonAsync<DouYinAwemeMixListApiModel>(url);
            return awemeMixList!;
        }
        public async Task<DouYinAwemeMixApiModel> GetYinAwemeMixAwemesAsync(string mix_id, long max_cursor = 0)
        {
            var url = string.Format(Constant.AwemeMixUrl, mix_id, max_cursor);
            var awemeMixAwmes = await _client.GetFromJsonAsync<DouYinAwemeMixApiModel>(url);
            return awemeMixAwmes!;
        }
        public async Task DownLoadVideoAsync(VideoItem video, string tag = "")
        {
            var directory = string.IsNullOrWhiteSpace(tag) ? $"{Constant.FilePath ?? ""}{video.NikName}_{video.UId}\\" : ((Constant.FilePath ?? "") + tag+"\\");
            directory += video.AwemeType == 68 ? "images\\" : "";
            directory += string.IsNullOrEmpty(video.MixName) ? "" : video.MixName + "\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var coverFileName = $"{directory}{video.AwemeId}_cover.png";
            var videoFileName = $"{directory}{video.AwemeId}.mp4";
            try
            {
                string? msg;
                if (!File.Exists(coverFileName))
                {
                    using (HttpResponseMessage response = await _client.GetAsync(video.VideoCover, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();

                        using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                        {
                            using (FileStream fileStream = new FileStream(coverFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                await contentStream.CopyToAsync(fileStream);
                            }
                        }
                    }
                    msg = $"下载完成{video.NikName}-{video.AwemeId}的封面";
                    WeakReferenceMessenger.Default.Send(new NotifyMessage(msg));
                }
                if (video.AwemeType == 68 && video.Images is { Count: > 0 })
                {

                    for (int i = 0; i < video.Images.Count; i++)
                    {
                        videoFileName = $"{directory}{video.AwemeId}" + "_{0}.png";
                        var url = video.Images[i];

                        if (!File.Exists(string.Format(videoFileName, i)))
                        {
                            using (HttpResponseMessage response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                            {
                                response.EnsureSuccessStatusCode();

                                using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                                {
                                    using (FileStream fileStream = new FileStream(string.Format(videoFileName, i), FileMode.Create, FileAccess.Write, FileShare.None))
                                    {
                                        await contentStream.CopyToAsync(fileStream);
                                    }
                                }
                            }

                        }
                    }
                    msg = $"下载完成{video.NikName}-{video.AwemeId}的图集";
                    WeakReferenceMessenger.Default.Send(new NotifyMessage(msg));
                }
                else
                {
                    if (!File.Exists(videoFileName))
                    {
                        using (HttpResponseMessage response = await _client.GetAsync(video.Video, HttpCompletionOption.ResponseHeadersRead))
                        {
                            response.EnsureSuccessStatusCode();

                            using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                            {
                                using (FileStream fileStream = new FileStream(videoFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    await contentStream.CopyToAsync(fileStream);
                                }
                            }
                        }
                        msg = $"下载完成{video.NikName}-{video.AwemeId}的视频";
                        WeakReferenceMessenger.Default.Send(new NotifyMessage(msg));
                    }
                }



            }
            catch (Exception)
            {
                throw;
            }

        }
        private async Task<string> GenerateRequestParams(string url, string userAgent)
        {

            Uri uri = new(url);
            string query = HttpUtility.ParseQueryString(uri.Query).ToString()!;
            var engine = new Engine();
            engine.Execute(File.ReadAllText("./douyin_x-bogus.js"));
            var xbogus = engine.Invoke("sign", query, userAgent).AsString();
            await Console.Out.WriteLineAsync("X-Bogus=" + xbogus);
            string newUrl = url + "&X-Bogus=" + xbogus;

            return newUrl;
        }
        public string GenerateRandomString(int randomLength)
        {
            string baseStr = "ABCDEFGHIGKLMNOPQRSTUVWXYZabcdefghigklmnopqrstuvwxyz0123456789=";
            Random random = new Random();
            StringBuilder randomStr = new StringBuilder();

            for (int i = 0; i < randomLength; i++)
            {
                randomStr.Append(baseStr[random.Next(baseStr.Length)]);
            }
            return randomStr.ToString();
        }
        public string GetMsToken()
        {
            return GenerateRandomString(107);
        }
        private async Task<string> ExtractModalId(string url)
        {
            string pattern = @"/(\d+)\?";
            if (url.Contains("v.douyin.com"))
            {
                var res = await _client.GetAsync(url);
                url = res.RequestMessage!.RequestUri!.AbsoluteUri;
            }
            else
            {
                pattern = @"modal_id=(\d+)";
            }

            Match match = Regex.Match(url, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                throw new Exception("url地址异常");
            }
        }


    }
}
