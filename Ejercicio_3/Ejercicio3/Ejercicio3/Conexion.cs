using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
namespace Ejercicio3
{
    public class Conexion
    {
        private readonly MySqlConnection cnx = null;
        private readonly MySqlConnectionStringBuilder cadena = new MySqlConnectionStringBuilder();
        public Conexion()
        {
            cadena.Server = @"localhost";
            cadena.UserID = "root";
            cadena.Password = "1235";
            cadena.Database = "ejercicio3";
            cadena.Port = 3306;
            cnx = new MySqlConnection(cadena.ToString());
        }
        public MySqlConnection Cnx()
        {
            return cnx;
        }

    }
}