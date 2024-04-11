using DouYin.DownLoader.Common.Models;

namespace DouYin.DownLoader.Services
{
    public interface IDouYinDownlaodService
    {
        Task<DouYinAwemeDetailApiModel> GetAwemeDetailAsync(string url);
        Task<DouYinAwemListApiModel> GetAuthorVideosAsync(string userId, long max_cursor);
        Task<DouYinAwemeSearchListApiModel> GetSearchVideosAsync(string keyWord, long max_cursor = 0);
        Task DownLoadVideoAsync(VideoItem video, string tag = "");
        Task<DouYinCommentListApiModel> GetAwemeCommentListAsync(string awemeId, long max_cursor = 0);
        Task<DouYinAwemeMixListApiModel> GetYinAwemeMixListAsync(string mix_id, long max_cursor = 0);
        Task<DouYinAwemeMixApiModel> GetYinAwemeMixAwemesAsync(string mix_id, long max_cursor = 0);
    }
}
