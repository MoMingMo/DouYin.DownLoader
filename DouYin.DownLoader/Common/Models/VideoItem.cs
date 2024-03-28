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
        /// 评论数
        /// </summary>
        public double CommentCount { get; set; }
        /// <summary>
        /// 点赞数据
        /// </summary>
        public double DiggCount { get; set; }
        /// <summary>
        /// 分享数
        /// </summary>
        public double ShareCount { get; set; }

        public string? VideoTag { get; set; }
        /// <summary>
        /// 地址栏中的
        /// MS4wLjABAAAAZD35Wpqy1uyKvLyV8pfQen_TxIDCREYM-jGwaL1YXI0
        /// </summary>
        public string? SecUid {  get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ? UId { get; set; }

        public string? AwemeId { get; set;}

        public int AwemeType { get; set; }

        public List<string>? Images { get; set; }
    }
}
