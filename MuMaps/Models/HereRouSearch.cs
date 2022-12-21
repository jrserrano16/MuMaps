using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuMaps.Models
{
    public class HereRouSearch
    {
            public string action { get; set; }
            public int duration { get; set; }
            public int length { get; set; }
            public string instruction { get; set; }
            public int offset { get; set; }
            public string direction { get; set; }
            public string severity { get; set; }
            public int? exit { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class OriginalLocation
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Place
        {
            public string type { get; set; }
            public Location location { get; set; }
            public OriginalLocation originalLocation { get; set; }
        }

        public class Departure
        {
            public DateTime time { get; set; }
            public Place place { get; set; }
        }

        public class Arrival
        {
            public DateTime time { get; set; }
            public Place place { get; set; }
        }

        public class Summary
        {
            public int duration { get; set; }
            public int length { get; set; }
            public int baseDuration { get; set; }
        }

        public class Transport
        {
            public string mode { get; set; }
        }

        public class Section
        {
            public string id { get; set; }
            public string type { get; set; }
            public List<HereRouSearch> actions { get; set; }
            public Departure departure { get; set; }
            public Arrival arrival { get; set; }
            public Summary summary { get; set; }
            public string polyline { get; set; }
            public string language { get; set; }
            public Transport transport { get; set; }
        }

        public class Route
        {
            public string id { get; set; }
            public List<Section> sections { get; set; }
        }

        public class HereRouResult
    {
            public List<Route> routes { get; set; }
        }

    }