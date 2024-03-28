using DouYin.DownLoader.Common.Models;

namespace DouYin.DownLoader.Services
{
    public interface IDouYinDownlaodService
    {
        Task<DouYinAwemeDetailApiModel> GetAwemeDetailAsync(string url);
        Task<DouYinAwemListApiModel> GetAuthorVideos(string userId, long max_cursor);
        Task DownLoadVideoAsync(VideoItem video);
    }
}
