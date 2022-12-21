using MuMaps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MuMaps.Helpers
{
    public class AuxClass
    {
        public static List<Usuario> CargarContenidoXML()
        {
            List<Usuario> listadoUsuario = new List<Usuario>();
            // Cargar contenido de prueba
            XmlDocument docSol = new XmlDocument();
            docSol.Load("listaUsuario.xml");
            foreach (XmlNode node in docSol.DocumentElement.ChildNodes)
            {
                var nuevoUser = new Usuario("", "", "", "", "", "", "", "", false);
                nuevoUser.Nombre = node.Attributes["Nombre"].Value;
                nuevoUser.ape = node.Attributes["Apellidos"].Value;
                nuevoUser.sexo = node.Attributes["Sexo"].Value;
                nuevoUser.pais = node.Attributes["Pais"].Value;
                nuevoUser.username = node.Attributes["Username"].Value;
                nuevoUser.contrasena = node.Attributes["Password"].Value;
                nuevoUser.email = node.Attributes["Email"].Value;
                nuevoUser.metodopago = node.Attributes["Metodopago"].Value;
                nuevoUser.Premium = Convert.ToBoolean(node.Attributes["Premium"].Value);
                listadoUsuario.Add(nuevoUser);
            }
            return listadoUsuario;
        }
    }
}
