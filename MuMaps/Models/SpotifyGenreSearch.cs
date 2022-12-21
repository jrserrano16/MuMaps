using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuMaps.Models
{
    public class SpotifyGenreSearch
    {
        public class Icon
        {
            public int? height { get; set; }
            public string url { get; set; }
            public int? width { get; set; }
        }

        public class Item
        {
            public string href { get; set; }
            public List<Icon> icons { get; set; }
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Categories
        {
            public string href { get; set; }
            public List<Item> items { get; set; }
            public int limit { get; set; }
            public string next { get; set; }
            public int offset { get; set; }
            public object previous { get; set; }
            public int total { get; set; }
        }

        public class SpotifyGenreResult
        {
            public Categories categories { get; set; }
        }
    }
 }
