using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MuMaps.Models.HereGeoSearch;

namespace MuMaps.Models
{
    public class HereGeoSearch

    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Address
        {
            public string label { get; set; }
            public string countryCode { get; set; }
            public string countryName { get; set; }
            public string stateCode { get; set; }
            public string state { get; set; }
        }

        public class Position
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class MapView
        {
            public double west { get; set; }
            public double south { get; set; }
            public double east { get; set; }
            public double north { get; set; }
        }

        public class FieldScore
        {
            public double state { get; set; }
        }

        public class Scoring
        {
            public double queryScore { get; set; }
            public FieldScore fieldScore { get; set; }
        }

        public class Item
        {
            public string title { get; set; }
            public string id { get; set; }
            public string resultType { get; set; }
            public string administrativeAreaType { get; set; }
            public Address address { get; set; }
            public Position position { get; set; }
            public MapView mapView { get; set; }
            public Scoring scoring { get; set; }
        }

        public class HereGeoResult
        {
            public List<Item> items { get; set; }
        }
    }
}

