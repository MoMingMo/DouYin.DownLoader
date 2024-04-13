using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DouYin.DownLoader.Common.Models
{
    public class MixItem
    {
        public string MixId { get; set; }
        public string MixCorver { get; set; }
        public string MixTitle { get; set; }

        public long? PlayCount { get; set; }
        public string MixPlay => (PlayCount / 10000.00)?.ToString("f2") + "W";
        public string Chapter { get; set; }
    }
}
