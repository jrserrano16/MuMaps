using MuMaps.Helpers;
using MuMaps.Models;
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
using System.Windows.Shapes;
using System.Xml;

namespace MuMaps
{
    /// <summary>
    /// Lógica de interacción para Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        private XmlDocument doc = new XmlDocument();
        private MainWindow mw;
        private BitmapImage imagCheck = new BitmapImage(new Uri("/src/correct.png", UriKind.Relative));
        private BitmapImage imagCross = new BitmapImage(new Uri("/src/wrong.png", UriKind.Relative));
        public List<Usuario> usuarios;

        public Registration()
        {
            InitializeComponent();
            usuarios = AuxClass.CargarContenidoXML();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (comprobarAllControls() & comprobarUser_Email(tbx_Username, 0, checkUsername) &
                comprobarUser_Email(tbx_Email, 1, checkEmail))
            {
                AñadirXML();

                MessageBox.Show("Usuario añadido con exito");

            }
            else
                MessageBox.Show("No se rellenó alguno de los campos obligatorios o el formato es incorrecto");
     
        }


        public bool comprobarControl(Control c, Image i)
        {

            if (c is TextBox)
            {
                if (((TextBox)c).Text == string.Empty)
                {
                    ((TextBox)c).BorderBrush = Brushes.Red;
                    i.Source = imagCross;
                    return false;
                }
                else
                {
                    ((TextBox)c).BorderBrush = Brushes.Green;
                    i.Source = imagCheck;
                    return true;
                }


            }
            else if (c is ComboBox)
            {
                if (((ComboBox)c).Text == string.Empty)
                {
                    ((ComboBox)c).BorderBrush = Brushes.Red;
                    i.Source = imagCross;
                    return false;
                }
                else
                {
                    ((ComboBox)c).BorderBrush = Brushes.Green;
                    i.Source = imagCheck;
                    return true;
                }
            }
            return true;
        }
        
        private bool comprobarAllControls()
        {
            if (comprobarControl(tbx_Nombre, checkName) & comprobarControl(tbx_Apellidos, checkSurname) &
                comprobarControl(cb_Genero, checkSexo) & comprobarControl(tbx_Pais, checkPais) &
                comprobarControl(tbx_Username, checkUsername) & comprobarControl(tbx_Password, checkPassw) &
                comprobarControl(tbx_Email, checkEmail) & comprobarControl(cb_MetodoPago, checkMetodoPago))
            {
                return true;
            }
            else
                return false;
        }


        private bool comprobarUser_Email(TextBox t, int mode, Image i)
        {
            if (mode == 0)
            {
                foreach (Usuario u in usuarios)
                {
                    if (t.Text.Equals(u.username))
                    {
                        i.Source = imagCross;
                        t.BorderBrush = Brushes.IndianRed;
                        return false;
                    }
                }
            }
            else
            {
                foreach (Usuario u in usuarios)
                {
                    if (t.Text.Equals(u.email))
                    {
                        i.Source = imagCross;
                        t.BorderBrush = Brushes.IndianRed;
                        return false;
                    }
                }
            }
            return true;
        }











        private void AñadirXML()
        {
            doc.Load("listaUsuario.xml");

            XmlNode Users = crearUsuario();
            XmlNode nodoRaiz = doc.DocumentElement;
            nodoRaiz.InsertAfter(Users, nodoRaiz.LastChild);

            doc.Save("listaUsuario.xml");
            
            
        }

        private XmlNode crearUsuario()
        {
            Models.Usuario nuevo = new Models.Usuario(tbx_Nombre.Text, tbx_Apellidos.Text, cb_Genero.Text, tbx_Pais.Text, tbx_Username.Text,
                tbx_Password.Text, tbx_Email.Text, cb_MetodoPago.Text, (bool)cbk_Premium.IsChecked);

            XmlElement nodoUsuario = doc.CreateElement("Usuario");
            XmlAttribute nombre = doc.CreateAttribute("Nombre");
            nombre.Value = nuevo.Nombre;
            nodoUsuario.Attributes.Append(nombre);
            XmlAttribute ape = doc.CreateAttribute("Apellidos");
            ape.Value = nuevo.ape;
            nodoUsuario.Attributes.Append(ape);
            XmlAttribute sexo = doc.CreateAttribute("Sexo");
            sexo.Value = nuevo.sexo;
            nodoUsuario.Attributes.Append(sexo);
            XmlAttribute pais = doc.CreateAttribute("Pais");
            pais.Value = nuevo.pais;
            nodoUsuario.Attributes.Append(pais);
            XmlAttribute username = doc.CreateAttribute("Username");
            username.Value = nuevo.username;
            nodoUsuario.Attributes.Append(username);
            XmlAttribute password = doc.CreateAttribute("Password");
            password.Value = nuevo.contrasena;
            nodoUsuario.Attributes.Append(password);
            XmlAttribute email = doc.CreateAttribute("Email");
            email.Value = nuevo.email ;
            nodoUsuario.Attributes.Append(email);
            XmlAttribute metodopago = doc.CreateAttribute("Metodopago");
            metodopago.Value = nuevo.metodopago;
            nodoUsuario.Attributes.Append(metodopago);
            XmlAttribute premium = doc.CreateAttribute("Premium");
            premium.Value = nuevo.Premium.ToString();
            nodoUsuario.Attributes.Append(premium);

            return nodoUsuario;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            mw = new MainWindow();
            mw.Show();
            this.Close();
        }
    }
}
