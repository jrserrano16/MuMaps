using MuMaps.Helpers;
using MuMaps.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace MuMaps
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MyTravels : Window
    {
        private List<LocationG> loc1 = new List<LocationG>();
        private List<LocationG> loc2 = new List<LocationG>();
        private string origen;
        private string destino;
        private Int32 tsegundos = 0;
        private MyPlaylist mp;
        private MainWindow mw;
        private string token;
        private string user;
        public MyTravels(string token, string user)
        {
            InitializeComponent();
            this.token = token;
            this.user = user;

        }

        private void tbx_LocInit_KeyUp(object sender, KeyEventArgs e)
        {
            txbLocs(tbx_LocInit, lst_LocInit);
        }
        private void tbx_LocFin_KeyUp(object sender, KeyEventArgs e)
        {
            txbLocs(tbx_LocFin, lst_LocFin);
        }
        private void txbLocs(TextBox t, ListBox l)
        {
            if (t.Text == string.Empty)
            {
                l.ItemsSource = null;
                return;
            }

            var result = SearchHelper.SearchCity(t.Text);

            if (result == null)
            {
                return;
            }

            var listaLocations = new List<LocationG>();


            foreach (var item in result.items)
            {
                listaLocations.Add(new LocationG()
                {
                    title = item.title,
                    lat = item.position.lat,
                    lng = item.position.lng


                });
            }


            l.ItemsSource = listaLocations;
            if (l.Name.Equals("lst_LocFin"))
            {
                loc2 = listaLocations;
            }
            else
            {
                loc1 = listaLocations;
            }

        }
        private void calcularDistancia(String transport)
        {

            double lat1 = loc1[lst_LocInit.SelectedIndex].lat;
            double lng1 = loc1[lst_LocInit.SelectedIndex].lng;
            double lat2 = loc2[lst_LocFin.SelectedIndex].lat;
            double lng2 = loc2[lst_LocFin.SelectedIndex].lng;
            var result = SearchHelper.generateRoute(lat1, lng1, lat2, lng2, transport);


            if (result == null)
            {
                return;
            }


            var listaRoutes = new List<RouteG>();
            List<Instruction> listainstrucion = new List<Instruction>();
            foreach (var item in result.routes)
            {
                foreach (var item2 in item.sections)
                {
                    foreach (var item3 in item2.actions)
                    {

                        listainstrucion.Add(new Instruction()
                        {
                            desc = item3.instruction
                        });
                    }
                    listaRoutes.Add(new RouteG()
                    {
                        distance = item2.summary.length,
                        time = item2.summary.duration
                    });


                    lst_Indicaciones.ItemsSource = listainstrucion;

                    tsegundos = item2.summary.duration;

                    duracion.Text = SearchHelper.getTravelPlaylistTime(tsegundos);
                    distancia.Text = (item2.summary.length/1000).ToString()+" kilómetro(s)";


                    if (distancia.Text != null)
                    {
                        btnPlayList.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void calcularc_Click(object sender, RoutedEventArgs e)
        {
            checkRoute();
      
        }
        private void checkRoute()
        {
            String transport = checkTransport();
            if (lst_LocFin.SelectedValue != null && lst_LocFin.SelectedValue != null && cbTransporte.SelectedIndex>0)

            {
                calcularDistancia(transport);
                origen = loc1[lst_LocInit.SelectedIndex].title;
                destino = loc2[lst_LocFin.SelectedIndex].title;
            }
            else
            {
                lst_Indicaciones.ItemsSource=null;
            }
        }

        private string checkTransport()
        {
            if (cbTransporte.SelectedIndex == 1)
            {
                return "pedestrian";
            }
            else if (cbTransporte.SelectedIndex == 2)
            {
                return "car";
            }
            if (cbTransporte.SelectedIndex == 3)
            {
                return "truck";
            }
            return "";
        }
        private void tbx_LocInit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tbx_LocInit.Text = String.Empty;

        }

        private void btn_Playlist_Click(object sender, RoutedEventArgs e)
        {
            
            if (tsegundos != 0)
            {
                mp = new MyPlaylist(tsegundos, token, user, origen, destino);
                mp.Show();
                this.Close();
            } else
            {
                MessageBox.Show("No se ha especificado ruta de viaje.");
            }
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }

}

