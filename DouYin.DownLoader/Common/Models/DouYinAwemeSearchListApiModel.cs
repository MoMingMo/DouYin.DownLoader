using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouYin.DownLoader.Common.Models
{


    public class DouYinAwemeSearchListApiModel
    {
        public int status_code { get; set; }
        public int has_more { get; set; }
        public int cursor { get; set; }
        public Datum[] data { get; set; }
        public Global_Doodle_Config global_doodle_config { get; set; }
        public string path { get; set; }
    }

   

    public class Global_Doodle_Config
    {
        public string keyword { get; set; }
    }

    public class Datum
    {
        public int type { get; set; }
        public Aweme_Info aweme_info { get; set; }
    }

    public class Aweme_Info
    {
        public string aweme_id { get; set; }
        public string desc { get; set; }
        public int create_time { get; set; }
        public Author author { get; set; }
        public Music music { get; set; }
        public object cha_list { get; set; }
        public Video video { get; set; }
        public int user_digged { get; set; }
        public Statistics statistics { get; set; }
        public object video_labels { get; set; }
        public int aweme_type { get; set; }
        public object image_infos { get; set; }
        public object position { get; set; }
        public object uniqid_position { get; set; }
        public object comment_list { get; set; }
        public long author_user_id { get; set; }
        public object geofencing { get; set; }
        public object video_text { get; set; }
        public int collect_stat { get; set; }
        public object label_top_text { get; set; }
        public object promotions { get; set; }
        public string group_id { get; set; }
        public bool prevent_download { get; set; }
        public object nickname_position { get; set; }
        public object challenge_position { get; set; }
        public object long_video { get; set; }
        public object interaction_stickers { get; set; }
        public object origin_comment_ids { get; set; }
        public object commerce_config_data { get; set; }
     
        public object anchors { get; set; }
        public string rawdata { get; set; }
        public object hybrid_label { get; set; }
        public object geofencing_regions { get; set; }
        public object cover_labels { get; set; }
        public object images { get; set; }
        public object relation_labels { get; set; }
    
        public object social_tag_list { get; set; }
        public object original_images { get; set; }
        public object img_bitrate { get; set; }
        public object video_tag { get; set; }
        public object chapter_list { get; set; }
        public object dislike_dimension_list { get; set; }
        public object standard_bar_info_list { get; set; }
        public object image_list { get; set; }
        public object origin_text_extra { get; set; }
        public object packed_clips { get; set; }
        public object tts_id_list { get; set; }
        public object ref_tts_id_list { get; set; }
        public object voice_modify_id_list { get; set; }
        public object ref_voice_modify_id_list { get; set; }
        public object dislike_dimension_list_v2 { get; set; }
        public object yumme_recreason { get; set; }
        public object slides_music_beats { get; set; }
        public object jump_tab_info_list { get; set; }
        public object reply_smart_emojis { get; set; }
        public object create_scale_type { get; set; }
    }
}
