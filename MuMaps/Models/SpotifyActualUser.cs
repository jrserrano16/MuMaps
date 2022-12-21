using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuMaps.Models
{
    public class SpotifyActualUser
    {
        public class ExplicitContent
        {
            public bool filter_enabled { get; set; }
            public bool filter_locked { get; set; }
        }

        public class ExternalUrls
        {
            public string spotify { get; set; }
        }

        public class Followers
        {
            public object href { get; set; }
            public int total { get; set; }
        }

        public class SpotifyActualUserRes
        {
            public string country { get; set; }
            public string display_name { get; set; }
            public ExplicitContent explicit_content { get; set; }
            public ExternalUrls external_urls { get; set; }
            public Followers followers { get; set; }
            public string href { get; set; }
            public string id { get; set; }
            public List<object> images { get; set; }
            public string product { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

    }
}
