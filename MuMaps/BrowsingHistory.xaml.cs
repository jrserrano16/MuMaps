using MuMaps.Helpers;
using MuMaps.Models;
using SpotifyAPI.Web.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace MuMaps
{
    /// <summary>
    /// Lógica de interacción para BrowsingHistory.xaml
    /// </summary>
    
    public partial class BrowsingHistory : Window
    {
        private string user;
        private string origen;
        private string destino;
        private string token;
        private int tsegundos;
        private MyPlaylist mp;
        private List<SpotifyPlaylist> playlists = new List<SpotifyPlaylist>();
        private List<SpotifyTrack> tracks = new List<SpotifyTrack>();
        public BrowsingHistory(int tsegundos, string token, string user, string origen, string destino)
        {
            this.origen = origen;
            this.destino = destino;
            this.token = token;
            this.tsegundos = tsegundos;
            this.user = user;
            InitializeComponent();
            mostrarPlaylist();
            
        }



        private void mostrarPlaylist()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("listaUsuario.xml");
            XmlNode root = doc.SelectSingleNode("Users");
            foreach (XmlNode usuario in root.ChildNodes)
            {
                if (usuario.Attributes["Username"].Value.Equals(user))
                {
                    if(usuario.SelectSingleNode("Playlists") != null)
                    {

                   
                    XmlNode playlist = usuario.SelectSingleNode("Playlists");
                    foreach (XmlNode play in playlist.ChildNodes)
                    {
                        SpotifyPlaylist p = new SpotifyPlaylist();
                        p.name = play.Attributes["Nombre"].Value;
                        p.origen = play.Attributes["Origen"].Value;
                        p.destino = play.Attributes["Destino"].Value;
                        p.id = play.Attributes["ID"].Value;


                    
                        playlists.Add(p);
                    }
                    }
                }
               
                
            }
            lst_Playlist.ItemsSource = playlists;
            

        }

        private void lst_Playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_Playlist.SelectedIndex >= 0)
            {
               mostrarCanciones();
            }
        }

        private void mostrarCanciones()
        {
         
            int min = 0;
            int seg = 0;
            
            List<SpotifyTrack> result = SearchHelper.SearchTracksP(playlists[lst_Playlist.SelectedIndex].id);

            if (result == null)
            {
                return;
            }
            foreach (SpotifyTrack t in result)
            {
                string [] duracion = t.duration.Split(':');
                min+=Convert.ToInt32(duracion[0]);
                seg+=Convert.ToInt32(duracion[1]);
            }
            int total = min*60 + seg;
            tbxduracion.Text = SearchHelper.getTravelPlaylistTime(total);
            lst_Canciones.ItemsSource = result;
        }

        private void btnView_Playlist_Click(object sender, RoutedEventArgs e)
        {
            if (lst_Playlist.SelectedIndex >= 0)
            {
                string spotifyURL = "https://open.spotify.com/playlist/";
                string id = playlists[lst_Playlist.SelectedIndex].id;
                BrowserUtil.Open(new Uri(spotifyURL+id));
            }
            else
                MessageBox.Show("¡Seleccione primero la playlist!", "Aviso");
           
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            mp= new MyPlaylist(tsegundos,token,user,origen,destino);
            mp.Show();
            this.Close();
        }
    }
}
