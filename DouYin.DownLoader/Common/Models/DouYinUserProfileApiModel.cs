using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouYin.DownLoader.Common.Models
{


    public class DouYinUserProfileApiModel
    {

        public int status_code { get; set; }
        public object status_msg { get; set; }
        public UserInfo user { get; set; }
    }



   

    public class UserInfo
    {
        public Anchor_Info anchor_info { get; set; }


        public Avatar_Larger avatar_larger { get; set; }

        public int aweme_count { get; set; }
        public int aweme_count_correction_threshold { get; set; }
        public int birthday_hide_level { get; set; }
        public bool can_set_item_cover { get; set; }
        public int can_show_group_card { get; set; }
     
        public string city { get; set; }
        public int close_friend_type { get; set; }
       
        public string country { get; set; }
     
        public string custom_verify { get; set; }
        public object district { get; set; }
        public int dongtai_count { get; set; }
       
     
        public int favoriting_count { get; set; }
        public bool follow_guide { get; set; }
        public int follow_status { get; set; }
        public int follower_count { get; set; }
        public int follower_request_status { get; set; }
        public int follower_status { get; set; }
        public int following_count { get; set; }
        public int forward_count { get; set; }
        public int gender { get; set; }
        public string ip_location { get; set; }
        public int max_follower_count { get; set; }
       
        public int mix_count { get; set; }
       
        public string nickname { get; set; }
        
        public string province { get; set; }
      
        public string sec_uid { get; set; }
        
       
        public string signature { get; set; }
       
        public int total_favorited { get; set; }
        public int total_favorited_correction_threshold { get; set; }
        public string twitter_id { get; set; }
        public string twitter_name { get; set; }
        public string uid { get; set; }
        public string unique_id { get; set; }
       
        public int user_age { get; set; }
     
        public int verification_type { get; set; }
       
        public string youtube_channel_id { get; set; }
        public string youtube_channel_title { get; set; }
    }

    public class Anchor_Info
    {
        public bool scheduled_master_switch { get; set; }
        public bool scheduled_profile_switch { get; set; }
        public string scheduled_time_text { get; set; }
    }

   
    public class Avatar_Larger
    {
        public int height { get; set; }
        public string uri { get; set; }
        public string[] url_list { get; set; }
        public int width { get; set; }
    }

  

  

}
