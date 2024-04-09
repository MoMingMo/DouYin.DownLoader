using DouYin.DownLoader.Common.Models;

namespace DouYin.DownLoader.Services
{
    public interface IDouYinDownlaodService
    {
        Task<DouYinAwemeDetailApiModel> GetAwemeDetailAsync(string url);
        Task<DouYinAwemListApiModel> GetAuthorVideos(string userId, long max_cursor);
        Task DownLoadVideoAsync(VideoItem video);
        Task<DouYinCommentListApiModel> GetAwemeCommentList(string awemeId, long max_cursor = 0);
        Task<DouYinAwemeMixApiModel> GetYinAwemeMixList(string mix_id, long max_cursor = 0);
    }
}
