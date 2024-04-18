using FlyleafLib.MediaPlayer;

namespace DouYin.DownLoader.Common.Models
{
    public class VideoItem
    {
        public string? Title { get; set; }
        public string? NikName { get; set; }
        public string? Avatar { get; set; }
        public string? VideoCover { get; set; }
        public string? Video { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public double CollectCount { get; set; }
        /// <summary>
        /// 收藏数
        /// </summary>
        public string Collect => (CollectCount / 1000.00).ToString("f2") + "K";
        /// <summary>
        /// 评论数
        /// </summary>
        public double CommentCount { get; set; }
        public string Comment=> (CommentCount / 1000.00).ToString("f2") + "K";
        /// <summary>
        /// 点赞数据
        /// </summary>
        public double DiggCount { get; set; }
        public string Digg => ((DiggCount / 1000.00)).ToString("f2") + "K";
        /// <summary>
        /// 分享数
        /// </summary>
        public double ShareCount { get; set; }
        public string Share => (ShareCount / 1000.00).ToString("f2") + "K";

        public string? VideoTag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ? UId { get; set; }

        public string? AwemeId { get; set;}

        public int AwemeType { get; set; }

        public List<string>? Images { get; set; }

        public string MixName { get; set; } = default!;

        public long CreateAt { get; set; }
    }
}
