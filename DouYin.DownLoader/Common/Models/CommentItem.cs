using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouYin.DownLoader.Common.Models
{
   public class CommentItem
    {
        public string? Text { get; set; }
        public int DiggCount { get; set; }
        public string? NickName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
