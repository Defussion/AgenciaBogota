using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Client;
using System.Collections.Generic;


namespace Agencia_Bogota.Pages
{
   
    public class Sede
    {
        public int ID_SEDE { get; set; }
        public string SEDE { get; set; }
    }

    public class RegistroEmpresaModel : PageModel
    {
        private readonly string _connectionString = "Data Source=tcp:dbsebas.database.windows.net,1433;Initial Catalog=Prueba2023;User Id=SebasLam@dbsebas;Password=Sebasdb24-";

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string NIT { get; set; }

        [BindProperty]
        public string Razon { get; set; }

        [BindProperty]
        public string Representante { get; set; }

        [BindProperty]
        public string Direcciones { get; set; }

        [BindProperty]
        public string Telefonos { get; set; }

        //Ciudad
        public List<String> Ciudades = new List<String>();

        //Pais

        public List<String> Paises = new List<String>();

        //Sedes
        public List<String> Sedes = new List<String>();

        [BindProperty]
        public string CiudadSeleccionada { get; set; }

        [BindProperty]
        public string PaisSeleccionado { get; set; }

        [BindProperty]
        public string SedeSeleccionada { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Email { get; set; }


        public void OnGet()
        {
            string queryCiudades = "SELECT CIUDAD FROM CIUDADES";
            string queryPaises = "SELECT PAIS FROM PAISES";
            string querySedes = "SELECT SEDE FROM SEDES";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // Abrir la conexión
                connection.Open();

                // Obtener las ciudades
                using (SqlCommand command = new SqlCommand(queryCiudades, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Console.WriteLine(reader.GetString(0));

                            Ciudades.Add(reader.GetString(0));
                        }
                    }
                }

                // Obtener los países
                using (SqlCommand command = new SqlCommand(queryPaises, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Paises.Add(reader.GetString(0));
                        }
                    }
                }

                // Obtener las sedes
                using (SqlCommand command = new SqlCommand(querySedes, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Sedes.Add(reader.GetString(0));
                        }
                    }
                }

                // Cerrar la conexión
                connection.Close();
            }
        }

        public IActionResult OnPost()
        {
            var ciudadSeleccionada = Request.Form["ciudad"].ToString();
            var paisSeleccionado = Request.Form["pais"].ToString();
            var sedeSeleccionada = Request.Form["sede"].ToString();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO USERS (USERNAME ,PASSWORD, ROL) VALUES (@USERNAME, @PASSWORD, @ROL)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@USERNAME", Email);
                command.Parameters.AddWithValue("@PASSWORD", Password);
                command.Parameters.AddWithValue("@ROL", "EMPRESA");

                command.ExecuteNonQuery();

                //Obteniendo ID_USUARIO FROM TABLA USERS 
                string queryID = "SELECT ID_USUARIO FROM USERS WHERE USERNAME = @Email";
                SqlCommand commandID = new SqlCommand(queryID, connection);
                commandID.Parameters.AddWithValue("@EMAIL", Email);
                SqlDataReader reader = commandID.ExecuteReader();
                int ID_USUARIO_USERS = 0;
                if (reader.Read())
                {
                    ID_USUARIO_USERS = reader.GetInt32(0);
                }
                reader.Close();

                //Lista de EMPRESA
                string query2 = "INSERT INTO EMPRESA (ID_USUARIO, NIT, RAZON_SOCIAL, EMAIL, DIRECCIONES, TELEFONOS, PAIS, CIUDAD, SEDE) VALUES (@ID_USUARIO, @NIT, @RAZON_SOCIAL, @EMAIL, @DIRECCIONES, @TELEFONOS, @PAIS, @CIUDAD, @SEDE )";
                SqlCommand command2 = new SqlCommand(query2, connection);
                command2.Parameters.AddWithValue("@ID_USUARIO", ID_USUARIO_USERS);
                command2.Parameters.AddWithValue("@NIT", NIT);
                command2.Parameters.AddWithValue("@RAZON_SOCIAL", Razon);
                command2.Parameters.AddWithValue("@EMAIL", Email);
                command2.Parameters.AddWithValue("@DIRECCIONES", Direcciones);
                command2.Parameters.AddWithValue("@TELEFONOS", Telefonos);
                command2.Parameters.AddWithValue("@PAIS", paisSeleccionado);
                command2.Parameters.AddWithValue("@CIUDAD", ciudadSeleccionada);
                command2.Parameters.AddWithValue("@SEDE", sedeSeleccionada);
            

                command2.ExecuteNonQuery();
            }
                return RedirectToPage("InicioSesion");
        }
    }
}
