using MuMaps.Helpers;
using MuMaps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using static MuMaps.Models.SpotifyAlbumSearch;
using static MuMaps.Models.SpotityArtistTracksSearch;

namespace MuMaps
{
    /// <summary>
    /// Lógica de interacción para MyPlaylist.xaml
    /// </summary>
    public partial class MyPlaylist : Window
    {
        private List<SpotifyArtist> listaArtistas;
        private List<SpotifyAlbum> listaAlbumes;
        private List<SpotifyTrack> listaCanciones;
        private List<SpotifyTrack> cancionesPlaylist = new List<SpotifyTrack>();
        private int tiempoRestante = 0;
        private int tiempoCancion = 0;
        private string token;
        private string user;
        private string origen;
        private string destino;
        private MyTravels win;
        private int tsegundos;
        private BrowsingHistory bw;


        public MyPlaylist(int tsegundos, string token, string user, string origen, string destino)
        {
            this.tsegundos = tsegundos;
            this.origen = origen;
            this.destino = destino;
            this.token = token;
            InitializeComponent();
            SearchHelper.GetToken();
            this.user = user;
            tiempoRestante = tsegundos;
        }
        private void topTracks()
        {

            SpotifyArtistTracksResult result = null;
            if (check_idArtist())
            {
                result = SearchHelper.SearchTracks(listaArtistas[lst_Artistas.SelectedIndex].ID);

            }
            else
            {
                return;
            }

            if (result == null)
            {
                return;
            }
            lst_Playlist.ItemsSource = null;
            foreach (var item in result.tracks)
            {

                tiempoCancion = item.duration_ms / 1000;

                if (tiempoRestante>0)
                {
                    cancionesPlaylist.Add(new SpotifyTrack()
                    {
                        nombre = item.name,
                        uri = item.uri,
                        artist = item.artists[0].name,
                        image = item.album.images[0].url,
                        duration = SearchHelper.getTimeTrack(item.duration_ms)
                    });

                    tiempoRestante = tiempoRestante - tiempoCancion;

                }
                else
                {
                    MessageBox.Show("Se ha alcanzado el tiempo necesario para la ruta..");
                    break;
                }
            }
            lst_Playlist.ItemsSource = cancionesPlaylist;
        }

        private void topTracksAlbumes()
        {
            SpotifyAlbumTracksResult resultA = null;
            if (check_idAlbum())
            {
                resultA = SearchHelper.SearchTracksA(listaAlbumes[lst_Albums.SelectedIndex].Id);

            }
            else
            {
                return;
            }

            if (resultA == null)
            {

                return;
            }

            lst_Playlist.ItemsSource = null;
            foreach (var item in resultA.items)
            {
                tiempoCancion = item.duration_ms / 1000;

                if (tiempoRestante>0)
                {
                    cancionesPlaylist.Add(new SpotifyTrack()
                    {
                        nombre = item.name,
                        uri = item.uri,
                        image = listaAlbumes[lst_Albums.SelectedIndex].url,
                        artist = item.artists[0].name,
                        duration = SearchHelper.getTimeTrack(item.duration_ms)
                    });

                    tiempoRestante = tiempoRestante - tiempoCancion;

                }
                else
                {
                    MessageBox.Show("Se ha alcanzado el tiempo necesario para la ruta.");
                    break;
                }
            }
            lst_Playlist.ItemsSource = cancionesPlaylist;
        }

        private bool check_idArtist()
        {
            if (lst_Artistas.SelectedIndex>=0)
            {
                return true;
            }
            else
                return false;
        }

        private bool check_idAlbum()
        {
            if (lst_Albums.SelectedIndex>=0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool check_idTrack()
        {
            if (lst_Canciones.SelectedIndex>=0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void tbx_Artista_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbx_busArtista.Text == string.Empty)
            {
                lst_Artistas.ItemsSource = null;
                return;
            }

            var result = SearchHelper.SearchArtist(tbx_busArtista.Text);

            if (result == null)
            {
                return;
            }

            listaArtistas = new List<SpotifyArtist>();

            foreach (var item in result.artists.items)
            {

                listaArtistas.Add(new SpotifyArtist()
                {
                    ID = item.id,
                    Image = item.images.Any() ? item.images[0].url : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png",
                    Name = item.name,
                    Followers = $"{item.followers.total.ToString("N")} seguidores"
                });
            }

            lst_Artistas.ItemsSource = listaArtistas;
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            win = new MyTravels(token, user);
            win.Show();
            this.Close();
        }



        private void btnCreate_Playlist_Click(object sender, RoutedEventArgs e)
        {
            if (tiempoRestante > 0)
            {
                MessageBox.Show("Necesitas añadir más canciones para tu viaje.");
            }
            else
            {
                if (tbx_Nombre.Text != String.Empty && tbx_Descripcion.Text != String.Empty) { 
                    SearchHelper.createPlaylist(tbx_Nombre.Text, tbx_Descripcion.Text, (bool)cbx_Publico.IsChecked, cancionesPlaylist, token, origen, destino, user);
                MessageBox.Show("Playlist Creada con éxito");
                    lst_Albums.ItemsSource = null;
                    lst_Artistas.ItemsSource = null;
                    lst_Canciones.ItemsSource = null;
                    lst_Playlist.ItemsSource = null;
                    tbx_Nombre.Text = String.Empty;
                    tbx_Descripcion.Text = String.Empty;
                }
                else
                {
                    MessageBox.Show("La playlist necesita tener un nombre y una descripción");
                }
        }

        }

        private void tbx_busAlbum_KeyUp(object sender, KeyEventArgs e)
        {
            List<string> artists = new List<string>();
            if (tbx_busAlbum.Text == string.Empty)
            {
                lst_Albums.ItemsSource = null;
                return;
            }

            var result = SearchHelper.SearchAlbum(tbx_busAlbum.Text);

            if (result == null)
            {
                return;
            }

            listaAlbumes = new List<SpotifyAlbum>();

            foreach (var item in result.albums.items)
            {
                listaAlbumes.Add(new SpotifyAlbum()
                {
                    Id = item.id,
                    url = item.images.Any() ? item.images[0].url : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png",
                    Title = item.name,
                    artist = item.artists[0].name
                });

            }


            lst_Albums.ItemsSource = listaAlbumes;
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            bw = new BrowsingHistory(tsegundos, token, user, origen, destino);
            bw.Show();
            this.Close();
        }

        private void tbx_Cancion_KeyUp(object sender, KeyEventArgs e)
        {
            if (tbx_Cancion.Text == string.Empty)
            {
                lst_Canciones.ItemsSource = null;
                return;
            }

            var result = SearchHelper.SearchTrack(tbx_Cancion.Text);

            if (result == null)
            {
                return;
            }

            listaCanciones = new List<SpotifyTrack>();

            foreach (var item in result.tracks.items)
            {

                listaCanciones.Add(new SpotifyTrack()
                {
                    nombre = item.name,
                    uri = item.uri,
                    image = item.album.images[0].url,
                    artist = item.artists[0].name,
                    duration = SearchHelper.getTimeTrack(item.duration_ms)
                });

            }

            lst_Canciones.ItemsSource = listaCanciones;
        }

        private void lst_Albums_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            topTracksAlbumes();
        }

        private void lst_Artistas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            topTracks();
        }

        private void tracks()
         {
             if(tbx_Cancion.Text == String.Empty)
             {
                 lst_Albums.ItemsSource = null;
                 return;
             }

             if (tiempoRestante>0)
             {      
                 if (check_idTrack())
                 {
                    lst_Playlist.ItemsSource = null;
                    cancionesPlaylist.Add(listaCanciones[lst_Canciones.SelectedIndex]);
                     string[] duracion = listaCanciones[lst_Canciones.SelectedIndex].duration.Split(':');
                     int min = Convert.ToInt32(duracion[0]);
                     int seg = Convert.ToInt32(duracion[1]);
                     int tiempoCancion = min*60 + seg;
                     tiempoRestante = tiempoRestante - tiempoCancion;
                     lst_Playlist.ItemsSource = cancionesPlaylist;
                 } 
             }
             else
             {
                 MessageBox.Show("Se ha alcanzado el tiempo necesario para la ruta..");

             }

         }

        private void lst_Canciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tracks();
        }
    }
}
