using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;
using MuMaps.Models;
using System.Xml;
using MuMaps.Helpers;

namespace MuMaps
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyTravels mt;
        private Registration r;
        private BitmapImage imagCheck = new BitmapImage(new Uri("/src/correct.png", UriKind.Relative));
        private BitmapImage imagCross = new BitmapImage(new Uri("/src/wrong.png", UriKind.Relative));
        private string token;
        public List<Usuario> usuarios;

        
        public MainWindow()
        {
            InitializeComponent();
            usuarios = AuxClass.CargarContenidoXML();
            
        

        }

       

        private void txtUsuario_KeyUp(object sender, KeyEventArgs e)
        {
            passContrasena.IsEnabled = false;
            txtUsuario.Background = Brushes.White;
            if (e.Key == Key.Enter)
            {
                if (ComprobarEntrada(txtUsuario.Text, usuarios, txtUsuario, checkUser,true))
                {
                    passContrasena.IsEnabled = true;
                    passContrasena.Focus();
                }

            }

        }

        private Boolean ComprobarEntrada(string valorIntroducido, List<Usuario> usuarios,
           Control componenteEntrada, Image imagenFeedBack, Boolean dato)
        {
            Boolean valido = false;
            String valorReal;
            componenteEntrada.BorderThickness = new Thickness(2);
            Usuario aux;

            foreach (Usuario usuario in usuarios)
            {
                if (dato)
                {
                    valorReal = usuario.username;
                }
                else
                {
                    if (usuario.username.Equals(txtUsuario.Text))
                    {
                        aux = usuario;
                        valorReal = aux.contrasena;
                    }
                    else { valorReal = ""; }
                }
                if (valorReal == valorIntroducido)
                {
                    // borde y background en verde
                    componenteEntrada.BorderBrush = Brushes.Green;
                    componenteEntrada.Background = Brushes.LightGreen;
                    // imagen al lado de la entrada de usuario --> check
                    imagenFeedBack.Source = imagCheck;
                    imagenFeedBack.ToolTip = "Credencial Correcta";
                    valido = true;
                    break;
                }
                else
                {
                    // marcamos borde en rojo
                    componenteEntrada.BorderBrush = Brushes.Red;
                    componenteEntrada.Background = Brushes.IndianRed;
                    // imagen al lado de la entrada de usuario --> cross
                    imagenFeedBack.Source = imagCross;
                    imagenFeedBack.ToolTip = "Credencial No encontrada";
                    valido = false;
                }
            }
            return valido;
        }



        private void passContrasena_KeyUp(object sender, KeyEventArgs e)
        {
            btnLogin.IsEnabled = false;
            passContrasena.Background = Brushes.White;

            {
                if (e.Key == Key.Enter)
                {
                    if (ComprobarEntrada(passContrasena.Password, usuarios, passContrasena, checkPassword,false) && checkUrl())
                    {
                        btnLogin.IsEnabled = true;
                    }
                }

            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {        
            
                mt = new MyTravels(token, txtUsuario.Text);
                mt.Show();
                this.Close();
                       
        }

        private bool checkUrl()
        {
            if (!txtURL.Text.Contains("access_token"))
            {
                MessageBox.Show("No se introdujo URL o la introducida no es correcta...");
                return false;
            }
            else
            {
                MessageBox.Show("URL correcta");
                string[] sep = txtURL.Text.Split('#');
                string [] sep2 = sep[1].Split('&');
                string [] access_token = sep2[0].Split('=');
                token = access_token[1]; 
                return true;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            r = new Registration();
            r.Show();
            this.Close();
        }

        private void btnSol_Token_Click(object sender, RoutedEventArgs e)
        {
            SearchHelper.getAuthToken();
        }

    }
}
