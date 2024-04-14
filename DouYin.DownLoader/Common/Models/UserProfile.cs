using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouYin.DownLoader.Common.Models
{
    public class UserProfile
    {
        public string AvatorUrl { get; set; }
        public string NickName { get; set; }
        public int FollowingCount { get; set; }
        public string Following { get; set; }
        public string Follower { get; set; }
        public int FollowerCount { get; set; }
        public string TotalFavorited { get; set; }
        public int TotalFavoritedCount { get; set; }
        public string UniqueId { get; set; }
        public string IPLocation { get; set; }
        public string Gender { get; set; }
        public int UserAge { get; set; }
        public string Country { get; set; }
        public string Signature { get; set; }
        public int AwemeCount { get; set; }
    }
}
