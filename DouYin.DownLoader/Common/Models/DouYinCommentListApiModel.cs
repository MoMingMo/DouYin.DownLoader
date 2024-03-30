using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouYin.DownLoader.Common.Models
{

    public class DouYinCommentListApiModel
    {
        public int status_code { get; set; }
        public IEnumerable<Comment>? comments { get; set; }
        public long cursor { get; set; }
        public long has_more { get; set; }
        public long reply_style { get; set; }
        public long total { get; set; }
      
        public long hotsoon_filtered_count { get; set; }
        public long user_commented { get; set; }
        public long show_management_entry_point { get; set; }
        public long folded_comment_count { get; set; }
    }





    public class Comment
    {
        public string cid { get; set; }
        public string text { get; set; }
        public string aweme_id { get; set; }
        public long create_time { get; set; }
        public int digg_count { get; set; }
        public int status { get; set; }
        public User user { get; set; }
        public string reply_id { get; set; }
        public int user_digged { get; set; }
        public object reply_comment { get; set; }
        public object[] text_extra { get; set; }
        public string label_text { get; set; }
        public int label_type { get; set; }
        public int reply_comment_total { get; set; }
        public string reply_to_reply_id { get; set; }
        public bool is_author_digged { get; set; }
        public int stick_position { get; set; }
        public bool user_buried { get; set; }
        public object label_list { get; set; }
        public bool is_hot { get; set; }
        public object text_music_info { get; set; }
        public object image_list { get; set; }
        public int is_note_comment { get; set; }
        public bool can_share { get; set; }
        public int item_comment_total { get; set; }
        public int level { get; set; }
        public object video_list { get; set; }
        public string sort_tags { get; set; }
        public bool is_user_tend_to_reply { get; set; }
        public int content_type { get; set; }
        public bool is_folded { get; set; }
    }

    public class User
    {
        public string uid { get; set; }
        public string short_id { get; set; }
        public string nickname { get; set; }
        public string signature { get; set; }
      
        public int follow_status { get; set; }
        public int aweme_count { get; set; }
        public int following_count { get; set; }
        public int follower_count { get; set; }
        public int favoriting_count { get; set; }
        public int total_favorited { get; set; }
        public bool is_block { get; set; }
        public bool hide_search { get; set; }
        public int constellation { get; set; }
        public int disable_image_comment_saved { get; set; }
        public bool hide_location { get; set; }
        public string weibo_verify { get; set; }
        public string custom_verify { get; set; }
        public string unique_id { get; set; }
        public object verification_permission_ids { get; set; }
        public int special_lock { get; set; }
        public int need_recommend { get; set; }
        public bool is_binded_weibo { get; set; }
        public string weibo_name { get; set; }
        public string weibo_schema { get; set; }
        public string weibo_url { get; set; }
        public bool story_open { get; set; }
        public int story_count { get; set; }
        public bool has_facebook_token { get; set; }
        public bool has_twitter_token { get; set; }
        public int fb_expire_time { get; set; }
        public int tw_expire_time { get; set; }
        public bool has_youtube_token { get; set; }
        public int youtube_expire_time { get; set; }
        public int room_id { get; set; }
        public int live_verify { get; set; }
        public int authority_status { get; set; }
        public string verify_info { get; set; }
        public int shield_follow_notice { get; set; }
        public int shield_digg_notice { get; set; }
        public int shield_comment_notice { get; set; }
        public object profile_mob_params { get; set; }
        public object avatar_schema_list { get; set; }
        public string awemehts_greet_info { get; set; }
        public bool with_commerce_entry { get; set; }
        public int verification_type { get; set; }
        public string enterprise_verify_reason { get; set; }
        public bool is_ad_fake { get; set; }
        public int live_high_value { get; set; }
        public string region { get; set; }
        public string account_region { get; set; }
        public int sync_to_toutiao { get; set; }
        public int commerce_user_level { get; set; }
        public int live_agreement { get; set; }
        public object platform_sync_info { get; set; }
        public bool with_shop_entry { get; set; }
        public bool is_discipline_member { get; set; }
        public int secret { get; set; }
        public bool has_orders { get; set; }
        public bool prevent_download { get; set; }
        public bool show_image_bubble { get; set; }
        public object[] geofencing { get; set; }
        public int unique_id_modify_time { get; set; }
        public string ins_id { get; set; }
        public string google_account { get; set; }
        public string youtube_channel_id { get; set; }
        public string youtube_channel_title { get; set; }
        public int apple_account { get; set; }
        public bool with_dou_entry { get; set; }
        public bool with_fusion_shop_entry { get; set; }
        public bool is_phone_binded { get; set; }
        public bool accept_private_policy { get; set; }
        public string twitter_id { get; set; }
        public string twitter_name { get; set; }
        public bool user_canceled { get; set; }
        public bool has_email { get; set; }
        public bool is_gov_media_vip { get; set; }
        public int live_agreement_time { get; set; }
        public int status { get; set; }
        public string avatar_uri { get; set; }
        public int follower_status { get; set; }
        public int neiguang_shield { get; set; }
        public int comment_setting { get; set; }
        public int duet_setting { get; set; }
        public int reflow_page_gid { get; set; }
        public int reflow_page_uid { get; set; }
        public int user_rate { get; set; }
        public int download_setting { get; set; }
        public int download_prompt_ts { get; set; }
        public int react_setting { get; set; }
        public bool live_commerce { get; set; }
        public int show_gender_strategy { get; set; }
        public string language { get; set; }
        public bool has_insights { get; set; }
        public object item_list { get; set; }
        public int user_mode { get; set; }
        public int user_period { get; set; }
        public bool has_unread_story { get; set; }
        public object new_story_cover { get; set; }
        public bool is_star { get; set; }
        public string cv_level { get; set; }
        public object type_label { get; set; }
        public object ad_cover_url { get; set; }
        public int comment_filter_status { get; set; }
     
        public object relative_users { get; set; }
        public object cha_list { get; set; }
        public string sec_uid { get; set; }
      
        public object need_points { get; set; }
        public object homepage_bottom_toast { get; set; }
        public int aweme_hotsoon_auth { get; set; }
        public object can_set_geofencing { get; set; }
        public string room_id_str { get; set; }
        public object white_cover_url { get; set; }
        public object user_tags { get; set; }
        public int stitch_setting { get; set; }
        public bool is_mix_user { get; set; }
        public bool enable_nearby_visible { get; set; }
        public object[] ban_user_functions { get; set; }
       
        public int user_not_show { get; set; }
      
        public int user_not_see { get; set; }
        public object card_entries { get; set; }
        public int signature_display_lines { get; set; }
        public object display_info { get; set; }
        public int follower_request_status { get; set; }
        public int live_status { get; set; }
        public bool is_not_show { get; set; }
        public object card_entries_not_display { get; set; }
        public object card_sort_priority { get; set; }
        public bool show_nearby_active { get; set; }
        public object interest_tags { get; set; }
        public int school_category { get; set; }
     
        public object link_item_list { get; set; }
        public object user_permissions { get; set; }
        public object offline_info_list { get; set; }
        public int is_cf { get; set; }
        public bool is_blocking_v2 { get; set; }
        public bool is_blocked_v2 { get; set; }
        public int close_friend_type { get; set; }
        public object signature_extra { get; set; }
        public int max_follower_count { get; set; }
        public object personal_tag_list { get; set; }
        public object cf_list { get; set; }
        public object im_role_ids { get; set; }
        public object not_seen_item_id_list { get; set; }
        public object familiar_visitor_user { get; set; }
        public int contacts_status { get; set; }
        public string risk_notice_text { get; set; }
        public object follower_list_secondary_information_struct { get; set; }
        public object endorsement_info_list { get; set; }
        public object text_extra { get; set; }
        public object contrail_list { get; set; }
        public object data_label_list { get; set; }
        public object not_seen_item_id_list_v2 { get; set; }
        public bool is_ban { get; set; }
        public object special_people_labels { get; set; }
        public int special_follow_status { get; set; }
    }
}
