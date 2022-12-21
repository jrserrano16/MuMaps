using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuMaps.Models
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string ape { get; set; }
        public string sexo { get; set; }
        public string pais { get; set; }
        public string username { get; set; }
        public string contrasena { get; set; }
        public string email { get; set; }
        public string metodopago { get; set; }
        public Boolean Premium { get; set; }

        public Usuario(string nombre, string ape, string sexo, string pais, string username,
            string contrasena, string email, string metodopago, bool premium)
        {
            Nombre = nombre;
            this.ape = ape;
            this.sexo = sexo;
            this.pais = pais;
            this.username = username;
            this.contrasena = contrasena;
            this.email = email;
            this.metodopago = metodopago;
            Premium = premium;
        }
    }
}
