namespace DouYin.DownLoader.Common.Models
{


    public class DouYinAwemListApiModel
    {
        public int status_code { get; set; }
        public long min_cursor { get; set; }
        public long max_cursor { get; set; }
        public int has_more { get; set; }
        public AwemeDetail[]? aweme_list { get; set; }
        public object? time_list { get; set; }
        public long request_item_cursor { get; set; }
        public int post_serial { get; set; }
        public int replace_series_cover { get; set; }
    }

   
  
   

}
