using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuMaps.Models
{
    public class SpotifyPlaylist
    {
       
        public string name { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
      
        public string id { get; set; }


        public SpotifyPlaylist(string name, string origen, string destino, string id)
        {
            this.name=name;
            this.origen=origen;
            this.destino=destino;
            this.id=id;
        }

        public SpotifyPlaylist()
        {
        }
    }
}
