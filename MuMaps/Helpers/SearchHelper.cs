using MuMaps.Models;
using Newtonsoft.Json;
using RestSharp;
using SpotifyAPI.Web.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static MuMaps.Models.HereGeoSearch;
using static MuMaps.Models.SpotifyActualPlaylist;
using static MuMaps.Models.SpotifyActualUser;
using static MuMaps.Models.SpotifyAlbumSearch;
using static MuMaps.Models.SpotifyArtistSearch;
using static MuMaps.Models.SpotifyGenreSearch;
using static MuMaps.Models.SpotifyPlaylistItems;
using static MuMaps.Models.SpotifyTrackSearch;
using static MuMaps.Models.SpotityArtistTracksSearch;


namespace MuMaps.Helpers
{
    public static class SearchHelper
    {
        public static SpotifyToken Stoken { get; set; }
        public static string Sauthorization;
        public static string Htoken = "lCDUb5Ol6Xos1uRifLp-DFTlgbuXjyMDn73PSETQXhc";
        public static string callback = "https://oauth.pstmn.io/v1/browser-callback";
        public static string clientID = "0e3fd9c1c8fd477785fe7c3145cb6560";
        public static string clientSecret = "3abcb3008f434ef4a001a1be33d50ed1";
        public static string scopes = "playlist-modify-public playlist-read-private playlist-modify-private";


        public static string GetToken()
        {
            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientID + ":" + clientSecret));
            var client = new RestClient("https://accounts.spotify.com/api/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Basic {auth}");
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("scope", "playlist-modify-public playlist-modify-private");
            IRestResponse response = client.Execute(request);
            Stoken = JsonConvert.DeserializeObject<SpotifyToken>(response.Content);

            return Stoken.access_token;

        }
        public static void getAuthToken()
        {
            var client = new RestClient("https://accounts.spotify.com/authorize?");
            var request = new RestRequest($"client_id={clientID}&client_secret={clientSecret}&response_type=token" +
                $"&redirect_uri={callback}&scope={scopes}", Method.GET);
            BrowserUtil.Open(new Uri(client.BaseUrl+request.Resource.ToString()));

        }
   
        
        public static string actualUser()
        {
            string id = null;

            var client = new RestClient("https://api.spotify.com/v1/me");

            client.AddDefaultHeader("Authorization", $"Bearer {Sauthorization}");
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyActualUserRes>(response.Content);
                id = result.id;
            }

            return id;
        }
        public static void createPlaylist(string n, string d, bool p, List<SpotifyTrack> tracks, string token,string origen, string destino, string muser)
        {
            Sauthorization = token;
            string userid = actualUser();
            string playlistid = null;
    

            var client = new RestClient("https://api.spotify.com/v1/users/"+userid+"/playlists");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {Sauthorization}");
            request.AddHeader("Accept", "application/json");
            request.AddJsonBody("{\"name\":\""+n+"\",\"description\":\""+d+"\",\"public\":\""+p+"\"}");
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyActualPlaylistRes>(response.Content);
                playlistid = result.id;
                

            }
            client = new RestClient("https://api.spotify.com/v1/playlists/"+playlistid+"");

            foreach (var t in tracks)
            { 
                request = new RestRequest($"/tracks?uris={t.uri}", Method.POST);
                request.AddHeader("Authorization", $"Bearer {Sauthorization}");
                request.AddHeader("Accept", "application/json");
                response = client.Execute(request);
            }


            crearXMLPlaylist(n,origen,destino,playlistid,muser);
    }
        public static SpotifyArtistRes SearchArtist(string searchWord)
        {
            var client = new RestClient("https://api.spotify.com/v1/search");

            client.AddDefaultHeader("Authorization", $"Bearer {Stoken.access_token}");
            var request = new RestRequest($"?q={searchWord}&type=artist", Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyArtistRes>(response.Content);
                return result;
            }
            else
            {
                return null;
            }

        }
        public static SpotifyAlbumRes SearchAlbum(string searchWord)
        {
            var client = new RestClient("https://api.spotify.com/v1/search");
            client.AddDefaultHeader("Authorization", $"Bearer {Stoken.access_token}");
            var request = new RestRequest($"?q={searchWord}&type=album", Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyAlbumRes>(response.Content);
                return result;
            }
            else
            {
                return null;
            }

        }
            public static SpotifyTrackRes SearchTrack(string searchWord)
            {
                var client = new RestClient("https://api.spotify.com/v1/search");
                client.AddDefaultHeader("Authorization", $"Bearer {Stoken.access_token}");
                var request = new RestRequest($"?q={searchWord}&type=track", Method.GET);
                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    var result = JsonConvert.DeserializeObject<SpotifyTrackRes>(response.Content);
                    return result;
                }
                else
                {
                    return null;
                }




            }
        public static SpotifyGenreResult SearchCategories()
        {
            var client = new RestClient("https://api.spotify.com/v1/browse/categories");
            client.AddDefaultHeader("Authorization", $"Bearer {Stoken.access_token}");
            var request = new RestRequest($"?country=ES&limit=50&locale=es_ES", Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyGenreResult>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }
        public static SpotifyArtistTracksResult SearchTracks(string idArtista)
        {
            var client = new RestClient("https://api.spotify.com/v1/artists/"+idArtista+"/top-tracks");
            client.AddDefaultHeader("Authorization", $"Bearer {Stoken.access_token}");
            var request = new RestRequest($"?market=ES", Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyArtistTracksResult>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public static List<SpotifyTrack> SearchTracksP(string idPlaylist)
        {
            List<SpotifyTrack> tracks = new List<SpotifyTrack>();
            int ncanciones = 0;
            int offset = 0;
            var client = new RestClient("https://api.spotify.com/v1/playlists/"+idPlaylist+"/tracks");
            client.AddDefaultHeader("Authorization", $"Bearer {Stoken.access_token}");
            var request = new RestRequest($"?market=ES", Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyPlaylistItemsRes>(response.Content);
                ncanciones = result.total;
                int limit = 50;
                while (offset < ncanciones)
                {
                    request = new RestRequest($"?market=ES&offset={offset}&limit={limit}", Method.GET);
                    response = client.Execute(request);
                    if (response.IsSuccessful)
                    {
                        result = JsonConvert.DeserializeObject<SpotifyPlaylistItemsRes>(response.Content);
                        foreach (var item in result.items)
                        {
                            tracks.Add(new SpotifyTrack()
                            {
                                nombre = item.track.name,
                                uri = item.track.uri,
                                image = item.track.album.images.Any() ? item.track.album.images[0].url : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png",
                                artist = item.track.artists[0].name,
                                duration = SearchHelper.getTimeTrack(item.track.duration_ms)
                            });
                            offset++;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            return tracks;
        }

        public static SpotifyAlbumTracksResult SearchTracksA(string idAlbum)
        {
            var client = new RestClient("https://api.spotify.com/v1/albums/"+idAlbum+"/tracks");
            client.AddDefaultHeader("Authorization", $"Bearer {Stoken.access_token}");
            var request = new RestRequest($"?market=ES", Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<SpotifyAlbumTracksResult>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public static HereGeoResult SearchCity(string place)
        {
            var client = new RestClient("https://geocode.search.hereapi.com/v1/geocode");
            var request = new RestRequest($"?q={place}&apikey={Htoken}", Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                HereGeoResult result = JsonConvert.DeserializeObject<HereGeoResult>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public static string getTravelPlaylistTime(int tsegundos)
        {
            Int32 horas = (tsegundos / 3600);
            Int32 minutos = ((tsegundos-horas*3600)/60);
            Int32 segundos = tsegundos-(horas*3600+minutos*60);
            return horas.ToString() + " hora(s) : " + minutos.ToString()
                            + " minuto(s) : " + segundos.ToString()+" segundo(s)";
        }

       



        public static string getTimeTrack(int ms)
        {
            Double tsegundos = ms/1000;
            Double mins = tsegundos / 60;
            Int32 minutos = ((int)tsegundos)/60;
            Double seg = (mins - minutos) * 60;
            Int32 segundos = ((int)seg);
            return minutos.ToString() +":"+segundos.ToString();
        }
        public static HereRouResult generateRoute(double lat1, double lng1, double lat2, double lng2, string transport)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            var client = new RestClient("https://router.hereapi.com/v8/routes");

            var request = new RestRequest($"?transportMode={transport}&lang=es&origin={lat1},{lng1}&destination={lat2},{lng2}&return=polyline,actions,instructions,summary&apikey={Htoken}", Method.GET);
            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                HereRouResult result = JsonConvert.DeserializeObject<HereRouResult>(response.Content);
                return result;
            }
            else
            {
                return null;
            }
        }

        public static void crearXMLPlaylist(string nombre, string origen, string destino, string id, string user)
        {

            SpotifyPlaylist nuevo = new SpotifyPlaylist(nombre, origen,  destino,  id);
            XmlDocument doc = new XmlDocument();
            doc.Load("listaUsuario.xml");
            XmlNode root = doc.SelectSingleNode("Users");
            XmlNode playlists = null;
            foreach (XmlNode usuario in root.ChildNodes)
            {
                if (usuario.Attributes["Username"].Value.Equals(user))
                {

                    if (usuario.SelectSingleNode("Playlists") == null)
                    {
                        playlists = doc.CreateElement("Playlists");
                        usuario.AppendChild(playlists);
                    }
                    else
                    {
                        playlists = usuario.SelectSingleNode("Playlists");
                    }
                    XmlElement nodoPlaylist = doc.CreateElement("Playlist");
                    XmlAttribute nomb = doc.CreateAttribute("Nombre");
                    nomb.Value = nuevo.name;
                    nodoPlaylist.Attributes.Append(nomb);
                    XmlAttribute orig= doc.CreateAttribute("Origen");
                    orig.Value = nuevo.origen;
                    nodoPlaylist.Attributes.Append(orig);
                    XmlAttribute dest = doc.CreateAttribute("Destino");
                    dest.Value = nuevo.destino;
                    nodoPlaylist.Attributes.Append(dest);
                    XmlAttribute iD = doc.CreateAttribute("ID");
                    iD.Value = nuevo.id;
                    nodoPlaylist.Attributes.Append(iD);
                    playlists.AppendChild(nodoPlaylist);
                    doc.Save("listaUsuario.xml");

                    
                    
                }
                
            }
        }




            
    }
}

