using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuMaps.Models
{
    public class ExternalUrls
    {
        public string spotify { get; set; }
    }

    public class Artist
    {
        public ExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class Item
    {
        public List<Artist> artists { get; set; }
        public int disc_number { get; set; }
        public int duration_ms { get; set; }
        public bool @explicit { get; set; }
        public ExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public bool is_local { get; set; }
        public bool is_playable { get; set; }
        public string name { get; set; }
        public string preview_url { get; set; }
        public int track_number { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class SpotifyAlbumTracksResult
    {
        public string href { get; set; }
        public List<Item> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public string previous { get; set; }
        public int total { get; set; }
    }
}
