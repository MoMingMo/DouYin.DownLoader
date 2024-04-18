namespace DouYin.DownLoader.Common.Models
{

    public class DouYinDiscoverApiModel
    {
        public int status_code { get; set; }
        public string status_msg { get; set; }
        public Card[] cards { get; set; }
        public int has_more { get; set; }
        public Baseresp BaseResp { get; set; }
    }

    public class Baseresp
    {
        public string StatusMessage { get; set; }
        public int StatusCode { get; set; }
    }

    public class Card
    {
        public int type { get; set; }
        public string aweme { get; set; }

        public Aweme_Info aweme_info => System.Text.Json.JsonSerializer.Deserialize<Aweme_Info>(aweme)!;
    }

}
