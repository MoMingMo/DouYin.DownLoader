using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouYin.DownLoader.Common.Models
{


    public class DouYinAwemeMixListApiModel
    {
        public int cursor { get; set; }
        public Extra extra { get; set; }
        public int has_more { get; set; }
        public Log_Pb log_pb { get; set; }
        public object min_cursor { get; set; }
        public Mix_Infos[] mix_infos { get; set; }
        public int status_code { get; set; }
        public object status_msg { get; set; }
        public int? total { get; set; }
    }

    public class Extra
    {
        public object[] fatal_item_ids { get; set; }
        public string logid { get; set; }
        public long now { get; set; }
    }

    public class Log_Pb
    {
        public string impr_id { get; set; }
    }

    public class Mix_Infos
    {
       
        public Cover_Url cover_url { get; set; }
        public int create_time { get; set; }
        public string desc { get; set; }
        public string extra { get; set; }
        public string mix_id { get; set; }
        public string mix_name { get; set; }
        public int mix_type { get; set; }

        public Statis statis { get; set; }
  
        public int update_time { get; set; }
    }
    public class Cover_Url
    {
        public int height { get; set; }
        public string uri { get; set; }
        public string[] url_list { get; set; }
        public int width { get; set; }
    }

   

   

    public class Statis
    {
        public int collect_vv { get; set; }
        public int current_episode { get; set; }
        public long? play_vv { get; set; }
        public int updated_to_episode { get; set; }
    }

  

}
