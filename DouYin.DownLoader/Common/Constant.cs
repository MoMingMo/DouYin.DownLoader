using System.Text.RegularExpressions;

namespace DouYin.DownLoader.Common
{
    public class Constant
    {

        public static string AwemeDetailUrl = "https://www.douyin.com/aweme/v1/web/aweme/detail/?aweme_id={0}&aid=1128&version_name=23.5.0&device_platform=android&os_version=2333";
        public static string AwemeListUrl = "https://www.douyin.com/aweme/v1/web/aweme/post/?device_platform=webapp&aid=6383&channel=channel_pc_web&sec_user_id={0}&max_cursor={1}&count=20";
        public static string AwemeCommenListtUrl = "https://www.douyin.com/aweme/v1/web/comment/list/?aweme_id={0}&cursor={1}&count=100&cookie_enabled=true";
        public static string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
        public static string DOU_YING = "douying";
        public static string RED_BOOK = "redbook";
        public static string? Cookie { get; private set; } // = "bd_ticket_guard_client_web_domain=2; my_rd=2; ttwid=1%7ChphrvuG5CsUKprYTjK92puqk03h-xkwh8XJW17s6vtM%7C1710406526%7C75b51377bcf92ac8409b6f8bf8def504e4b1bacec9ee3457c054aa15bcb41f75; passport_csrf_token=993620fdbf4c21ef7f7b95ac8662b5f6; passport_csrf_token_default=993620fdbf4c21ef7f7b95ac8662b5f6; FORCE_LOGIN=%7B%22videoConsumedRemainSeconds%22%3A180%7D; pwa2=%220%7C0%7C3%7C0%22; volume_info=%7B%22isUserMute%22%3Afalse%2C%22isMute%22%3Atrue%2C%22volume%22%3A1%7D; strategyABtestKey=%221711203534.407%22; passport_assist_user=CkDvqtTWKav4qqQk9Upz0HF93zFPbcInzLrl8oHVpJWlvnX4tdfVXg-MEoCDZSpLXzgPNl1AUcDi6T7TNy1FIbzCGkoKPHCJa0Jz3UqcM0mxh_-KRLes3oUgUKljwBDVotM_JqUuxKUQUrjHpQ9BwakeoD0jU7aXxd4zqPvcgW6S7xCO3MwNGImv1lQgASIBAwnbJpc%3D; n_mh=WZXOHilLLyxhKidu4EbUDyNMPqSKtT6EC2VBdvhzxeE; sso_uid_tt=543768dd5787b93cec632c4b143e0d41; sso_uid_tt_ss=543768dd5787b93cec632c4b143e0d41; toutiao_sso_user=07e58eefbaa2ac2439ad45847761eceb; toutiao_sso_user_ss=07e58eefbaa2ac2439ad45847761eceb; sid_ucp_sso_v1=1.0.0-KGNhYzNmZjEyYTQxMGE0NDYyOWY1ZGVmMmM3ZjQ0MzRlYjczMmU5NzkKHwj-zKHOkYyxARDuyfuvBhjvMSAMMMC4xpAGOAZA9AcaAmxxIiAwN2U1OGVlZmJhYTJhYzI0MzlhZDQ1ODQ3NzYxZWNlYg; ssid_ucp_sso_v1=1.0.0-KGNhYzNmZjEyYTQxMGE0NDYyOWY1ZGVmMmM3ZjQ0MzRlYjczMmU5NzkKHwj-zKHOkYyxARDuyfuvBhjvMSAMMMC4xpAGOAZA9AcaAmxxIiAwN2U1OGVlZmJhYTJhYzI0MzlhZDQ1ODQ3NzYxZWNlYg; download_guide=%222%2F20240323%2F0%22; GlobalGuideTimes=%221711203567%7C1%22; passport_auth_status=b05b2ede1962a2bb98742e0db19301bb%2C; passport_auth_status_ss=b05b2ede1962a2bb98742e0db19301bb%2C; uid_tt=6664bce219163d3acd2ee73fa94dcdb4; uid_tt_ss=6664bce219163d3acd2ee73fa94dcdb4; sid_tt=84747eb5b54a75d9dc1c4187b95c13de; sessionid=84747eb5b54a75d9dc1c4187b95c13de; sessionid_ss=84747eb5b54a75d9dc1c4187b95c13de; stream_recommend_feed_params=%22%7B%5C%22cookie_enabled%5C%22%3Atrue%2C%5C%22screen_width%5C%22%3A2560%2C%5C%22screen_height%5C%22%3A1440%2C%5C%22browser_online%5C%22%3Atrue%2C%5C%22cpu_core_num%5C%22%3A12%2C%5C%22device_memory%5C%22%3A8%2C%5C%22downlink%5C%22%3A10%2C%5C%22effective_type%5C%22%3A%5C%224g%5C%22%2C%5C%22round_trip_time%5C%22%3A50%7D%22; LOGIN_STATUS=1; _bd_ticket_crypt_doamin=2; _bd_ticket_crypt_cookie=df3b6e87fe976790f2dff2f7e31eefdf; __security_server_data_status=1; store-region=cn-bj; store-region-src=uid; sid_guard=84747eb5b54a75d9dc1c4187b95c13de%7C1711203570%7C5183999%7CWed%2C+22-May-2024+14%3A19%3A29+GMT; sid_ucp_v1=1.0.0-KDA2ZWVhZGIwNWUxZmNmMGVkY2VkZmJhNTdiOWQxZmFiZjY0NzI2MGUKGwj-zKHOkYyxARDyyfuvBhjvMSAMOAZA9AdIBBoCaGwiIDg0NzQ3ZWI1YjU0YTc1ZDlkYzFjNDE4N2I5NWMxM2Rl; ssid_ucp_v1=1.0.0-KDA2ZWVhZGIwNWUxZmNmMGVkY2VkZmJhNTdiOWQxZmFiZjY0NzI2MGUKGwj-zKHOkYyxARDyyfuvBhjvMSAMOAZA9AdIBBoCaGwiIDg0NzQ3ZWI1YjU0YTc1ZDlkYzFjNDE4N2I5NWMxM2Rl; IsDouyinActive=true; bd_ticket_guard_client_data=eyJiZC10aWNrZXQtZ3VhcmQtdmVyc2lvbiI6MiwiYmQtdGlja2V0LWd1YXJkLWl0ZXJhdGlvbi12ZXJzaW9uIjoxLCJiZC10aWNrZXQtZ3VhcmQtcmVlLXB1YmxpYy1rZXkiOiJCSjZiQ1FoQTZuUFNLY2M1cE5ZVGduVEw0NWtIR0NoMXFJM3FYRXp4d21rZHVHZWxNclRlWjFlZDBBTXBOVHg3UEV6UC81Zlh2ZUxYZmxxWkQ4L3lxRVU9IiwiYmQtdGlja2V0LWd1YXJkLXdlYi12ZXJzaW9uIjoxfQ%3D%3D; home_can_add_dy_2_desktop=%221%22; publish_badge_show_info=%220%2C0%2C0%2C1711203681701%22; odin_tt=8723ed03c9ebeddc34f3a396947a54e604c59a1f2ef75b875f4c7c324cb1c910e772e86d85fc68245fe3f75a3128dc16b279b550b6d11be61afa9dc923d9ede4; msToken={0}";

        public static void SetCookie(string cookie)
        {
            string pattern = @"msToken=.*?=="; // 正则表达式匹配不固定部分

            string replacedText = Regex.Replace(cookie, pattern, "msToken={0}");
            Cookie = replacedText;
        }
    }
    public record NotifyMessage(string message, bool isShowProcess = false);
}
