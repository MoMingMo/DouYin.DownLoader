namespace DouYin.DownLoader.Common.Models
{
    public class DouYinAwemeDetailApiModel
    {
        public AwemeDetail? aweme_detail { get; set; }
        public int status_code { get; set; } = -1;
    }

    public class AwemeDetail
    {
        public Author? author { get; set; }
        public long author_user_id { get; set; }
        public string? aweme_id { get; set; }
        /// <summary>
        /// 0视频68图文
        /// </summary>
        public int aweme_type { get; set; }
        public int boost_status { get; set; }
        public string? caption { get; set; }
        public string? preview_title { get; set; }
        public int create_time { get; set; }
        public string? desc { get; set; }
        public int duration { get; set; }
        public Music? music { get; set; }
        public Video? video { get; set; }
        public IEnumerable<Image>? images { get; set; }
        public IEnumerable<Video_Tag>? video_tag { get; set; }
        public int status_code { get; set; }

        public Statistics? statistics { get; set; }
    }

    public class Image
    {
        public string[] download_url_list { get; set; }
        public int height { get; set; }
        public string uri { get; set; }
        public string[] url_list { get; set; }
        public int width { get; set; }
    }

    public class Author
    {
        public Avatar_Thumb? avatar_thumb { get; set; }
        public string? nickname { get; set; }
        public string? unique_id { get; set; }
        public string? uid { get; set; }
        public string? short_id { get; set; }
        public string? sec_uid { get; set; }
    }

    public class Avatar_Thumb
    {
        public string[]? url_list { get; set; }
    }

    public class Music
    {
        public Play_Url? play_url { get; set; }
    }

    public class Play_Url
    {
        public string? uri { get; set; }
        public string? url_key { get; set; }
        public string[]? url_list { get; set; }
    }

    public class Video
    {
        public Cover? cover { get; set; }
        public Cover? origin_cover { get; set; }
        public Play_Addr? play_addr { get; set; }
        public Play_Addr? play_addr_265 { get; set; }
        public Play_Addr? play_addr_h264 { get; set; }
        public string? ratio { get; set; }
        public string? video_model { get; set; }
        public int? width { get; set; }
    }

    public class Cover
    {
        public int? height { get; set; }
        public string? uri { get; set; }
        public string[]? url_list { get; set; }
        public int? width { get; set; }
    }

    public class Play_Addr
    {
        public int? data_size { get; set; }
        public string? file_cs { get; set; }
        public string? file_hash { get; set; }
        public int? height { get; set; }
        public string? uri { get; set; }
        public string? url_key { get; set; }
        public string[]? url_list { get; set; }
        public int? width { get; set; }
    }

    public class Video_Tag
    {
        public int? level { get; set; }
        public long? tag_id { get; set; }
        public string? tag_name { get; set; }
    }


    public class Statistics
    {
        public int admire_count { get; set; }
        public string? aweme_id { get; set; }
        public int collect_count { get; set; }
        public int comment_count { get; set; }
        public int digg_count { get; set; }
        public int play_count { get; set; }
        public int share_count { get; set; }
    }
}
