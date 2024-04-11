using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouYin.DownLoader.Common.Models
{

    public class DouYinAwemeMixApiModel
    {
        public Aweme_List[] aweme_list { get; set; }
        public int cursor { get; set; }
        public int has_more { get; set; }
    }

  
    public class Aweme_List
    {
      
        public Author author { get; set; }
        public Video? video { get; set; }
        public IEnumerable<Image>? images { get; set; }
        public string aweme_id { get; set; }
        public int aweme_type { get; set; }
       
    }

   

}
