using CommunityToolkit.Mvvm.Messaging;
using Jint;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DouYin.DownLoader.Common;
using DouYin.DownLoader.Common.Models;
using Microsoft.VisualBasic.ApplicationServices;
using DryIoc.ImTools;

namespace DouYin.DownLoader.Services
{
    public class DouYinDownlaodService : IDouYinDownlaodService
    {
        private HttpClient _client;

        public DouYinDownlaodService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            SetHeaders();
        }
        private string ms = string.Empty;
        public void SetHeaders()
        {

            _client.BaseAddress = new Uri("https://www.douyin.com/");
            _client.DefaultRequestHeaders.Add("authority", "www.douyin.com");
            _client.DefaultRequestHeaders.Add("user-agent", Constant.UserAgent);
            _client.DefaultRequestHeaders.Add("referer", "https://www.douyin.com/");
            var ms = GetMsToken();
            if (!string.IsNullOrWhiteSpace(Constant.Cookie))
                _client.DefaultRequestHeaders.Add("cookie", string.Format(Constant.Cookie!, ms));
        }
        public async Task<DouYinDiscoverApiModel> GetDouYinDiscoverAsync()
        {
            var url = await GenerateRequestParams(Constant.DouYinDicoverUrl, Constant.UserAgent);
            var discoverResutl = await _client.GetFromJsonAsync<DouYinDiscoverApiModel>(url);
            //var cardsTask = discoverResutl!.cards.Select(async x =>
            //    {
            //        url = await GenerateRequestParams(string.Format(Constant.AwemeDetailUrl, x.aweme_info.aweme_id), Constant.UserAgent);
            //        var awemeDetail = await _client.GetFromJsonAsync<DouYinAwemeDetailApiModel>(url);
            //        x.aweme_info.video = awemeDetail?.aweme_detail?.video!;
            //        return x;
            //    });
            //var cards = await Task.WhenAll(cardsTask);
            //discoverResutl.cards = cards;
            return discoverResutl!;
        }
        public async Task<DouYinUserProfileApiModel> GetUserProfileAsync(string userId)
        {
            var url = await GenerateRequestParams(string.Format(Constant.UserProfileUrl, userId), Constant.UserAgent);
            var userProfile = await _client.GetFromJsonAsync<DouYinUserProfileApiModel>(url);
            return userProfile!;
        }
        /// <summary>
        /// 获取视频
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<DouYinAwemeDetailApiModel> GetAwemeDetailAsync(string url)
        {
            var modal_id = await ExtractModalId(url);

            url = await GenerateRequestParams(string.Format(Constant.AwemeDetailUrl, modal_id), Constant.UserAgent);
            var awemeDetail = await _client.GetFromJsonAsync<DouYinAwemeDetailApiModel>(url);
            return awemeDetail!;

        }
        /// <summary>
        /// 获取主页视频列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="max_cursor"></param>
        /// <returns></returns>
        public async Task<DouYinAwemListApiModel> GetAuthorVideosAsync(string userId, long? max_cursor = 0)
        {
            var url = await GenerateRequestParams(string.Format(Constant.AwemeListUrl, userId, max_cursor), Constant.UserAgent);
            var awemeList = await _client.GetFromJsonAsync<DouYinAwemListApiModel>(url);
            return awemeList!;
        }
        /// <summary>
        /// 获取查询视频列表
        /// </summary>
        /// <param name="keyWord"></param>
        /// <param name="max_cursor"></param>
        /// <returns></returns>
        public async Task<DouYinAwemeSearchListApiModel> GetSearchVideosAsync(string keyWord, long? max_cursor = 0)
        {
            var url = string.Format(Constant.AwemeSearchListUrl, keyWord, max_cursor); ;
            var awemeList = await _client.GetFromJsonAsync<DouYinAwemeSearchListApiModel>(url);
            return awemeList!;
        }
        /// <summary>
        /// 获取视频评论
        /// </summary>
        /// <param name="awemeId"></param>
        /// <param name="max_cursor"></param>
        /// <returns></returns>
        public async Task<DouYinCommentListApiModel> GetAwemeCommentListAsync(string awemeId, long? max_cursor = 0)
        {
            var url = await GenerateRequestParams(string.Format(Constant.AwemeCommenListtUrl, awemeId, max_cursor), Constant.UserAgent);
            var awemeCommentList = await _client.GetFromJsonAsync<DouYinCommentListApiModel>(url);
            return awemeCommentList!;
        }
        /// <summary>
        /// 获取合集列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="max_cursor"></param>
        /// <returns></returns>
        public async Task<DouYinAwemeMixListApiModel> GetAwemeMixListAsync(string userId, long? max_cursor = 0)
        {
            var url = await GenerateRequestParams(string.Format(Constant.AwemeMixListUrl, userId, max_cursor), Constant.UserAgent);
            var awemeMixList = await _client.GetFromJsonAsync<DouYinAwemeMixListApiModel>(url);
            return awemeMixList!;
        }
        /// <summary>
        ///获取合集中视频列表   
        /// </summary>
        /// <param name="mix_id"></param>
        /// <param name="max_cursor"></param>
        /// <returns></returns>
        public async Task<DouYinAwemeMixApiModel> GetAwemeMixAwemesAsync(string mix_id, long? max_cursor = 0)
        {
            var url = string.Format(Constant.AwemeMixUrl, mix_id, max_cursor);
            var awemeMixAwmes = await _client.GetFromJsonAsync<DouYinAwemeMixApiModel>(url);
            return awemeMixAwmes!;
        }
        /// <summary>
        /// 下载视频及图集
        /// </summary>
        /// <param name="video"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task DownLoadVideoAsync(VideoItem video, string tag = "")
        {
            var directory = string.IsNullOrWhiteSpace(tag) ? $"{Constant.FilePath ?? ""}{video.NikName}_{video.UId}\\" : ((Constant.FilePath ?? "") + tag + "\\");
            directory += video.AwemeType == 68 ? "images\\" : "";
            directory += string.IsNullOrEmpty(video.MixName) ? "" : video.MixName + "\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var coverFileName = $"{directory}{video.Digg}{video.AwemeId}_cover.png";
            var videoFileName = $"{directory}{video.Digg}{video.AwemeId}.mp4";
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
        /// <summary>
        /// 生成X-Bogus
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        private async Task<string> GenerateRequestParams(string url, string userAgent)
        {

            Uri uri = new(url);
            string query = HttpUtility.ParseQueryString(uri.Query).ToString()!;
            var engine = new Engine();
            engine.Execute(File.ReadAllText("./douyin_x-bogus.js"));
            var xbogus = engine.Invoke("sign", query, userAgent).AsString();
            string newUrl = url + "&X-Bogus=" + xbogus;

            return newUrl;
        }
        /// <summary>
        /// 生成mstoken
        /// </summary>
        /// <param name="randomLength"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 生成mstoken
        /// </summary>
        /// <param name="randomLength"></param>
        /// <returns></returns>
        public string GetMsToken()
        {
            return GenerateRandomString(107);
        }
        /// <summary>
        /// 解析地址获取视频id
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
