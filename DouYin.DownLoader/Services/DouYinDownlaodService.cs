using CommunityToolkit.Mvvm.Messaging;
using Jint;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
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
        private readonly string apiUrl = "https://www.douyin.com/aweme/v1/web/aweme/detail/?aweme_id={0}&aid=1128&version_name=23.5.0&device_platform=android&os_version=2333";
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
            _client.DefaultRequestHeaders.Add("cookie", string.Format(Constant.Cookie, ms));
        }
        public async Task<DouYinAwemeDetailApiModel> GetAwemeDetailAsync(string url)
        {
            var modal_id = ExtractModalId(url);
            url = string.Format(Constant.AwemeDetailUrl, modal_id);
            var awemeDetail = await _client.GetFromJsonAsync<DouYinAwemeDetailApiModel>(url);
            return awemeDetail!;

        }
        public async Task<DouYinAwemListApiModel> GetAuthorVideos(string userId, long max_cursor = 0)
        {
            var url = await GenerateRequestParams(string.Format(Constant.AwemeListUrl, userId, max_cursor), Constant.UserAgent);
            var awemeList = await _client.GetFromJsonAsync<DouYinAwemListApiModel>(url);
            return awemeList!;
        }

        public async Task DownLoadVideoAsync(VideoItem video)
        {
            var msg = $"开始下载{video.NikName}-{video.AwemeId}的相关文件";
            WeakReferenceMessenger.Default.Send(new ShowMessage(msg));
            var directory = $"{video.NikName}_{video.UId}\\";
            directory += video.AwemeType == 68 ? "images\\" : "";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var coverFileName = $"{directory}{video.AwemeId}_cover.png";
            var videoFileName = $"{directory}{video.AwemeId}.mp4";
            try
            {
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
                    WeakReferenceMessenger.Default.Send(new ShowMessage(msg));
                }
                if (video.AwemeType == 68 && video.Images is { Count: > 0 })
                {
                   
                    for (int i = 0; i < video.Images.Count; i++)
                    {
                        videoFileName = $"{directory}{video.AwemeId}" + "_{0}.png";
                        var url = video.Images[i];
                      
                        if (!File.Exists(string.Format(videoFileName,i)))
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
                    WeakReferenceMessenger.Default.Send(new ShowMessage(msg));
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
                        WeakReferenceMessenger.Default.Send(new ShowMessage(msg));
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
        private string ExtractModalId(string url)
        {
            string pattern = @"modal_id=(\d+)";
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
