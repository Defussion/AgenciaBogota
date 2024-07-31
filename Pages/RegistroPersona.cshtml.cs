using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Identity.Client;
using System.Collections.Generic;

namespace Agencia_Bogota.Pages
{
    public class Ciudad
    {
        public int ID_CIUDAD { get; set; }
        public string CIUDAD { get; set; }
    }

    public class Pais
    {
        public int ID_PAIS { get; set; }
        public string PAIS { get; set; }
    }

    public class RegistroPersonaModel : PageModel
    {
        private readonly string _connectionString = "Data Source=tcp:dbsebas.database.windows.net,1433;Initial Catalog=Prueba2023;User Id=SebasLam@dbsebas;Password=Sebasdb24-";

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Identificacion { get; set; }

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Direccion { get; set; }

        [BindProperty]
        public string Telefono { get; set; }

        //Ciudad
        public List<String> Ciudades = new List<String>();

        //Pais

        public List<String> Paises = new List<String>();

        [BindProperty]
        public string CiudadSeleccionada { get; set; }

        [BindProperty]
        public string PaisSeleccionado { get; set; }


        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Profesion { get; set; }

        [BindProperty]
        public IFormFile PdfFile { get; set; }

        [BindProperty]
        public IFormFile VideoFile { get; set; }

        public void OnGet()
        {
            string queryCiudades = "SELECT CIUDAD FROM CIUDADES";
            string queryPaises = "SELECT PAIS FROM PAISES";

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

                // Cerrar la conexión
                connection.Close();
            }

        }

        public IActionResult OnPost()
        {
            byte[] pdfBytes = null;
            byte[] videoBytes = null;

            var ciudadSeleccionada = Request.Form["ciudad"].ToString();
            var paisSeleccionado = Request.Form["pais"].ToString();


            if (PdfFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    PdfFile.CopyTo(memoryStream);
                    pdfBytes = memoryStream.ToArray();
                }
            }

            if (VideoFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    VideoFile.CopyTo(memoryStream);
                    videoBytes = memoryStream.ToArray();
                }
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO USERS (USERNAME ,PASSWORD, ROL) VALUES (@USERNAME, @PASSWORD, @ROL)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@USERNAME", Email);
                command.Parameters.AddWithValue("@PASSWORD", Password);
                command.Parameters.AddWithValue("@ROL", "DESEMPLEADO");

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

                //Lista de desempleado
                string query2 = "INSERT INTO DESEMPLEADO (ID_USUARIO, IDENTIFICACION, NOMBRE, EMAIL, DIRECCION, TELEFONO, PAIS, CIUDAD, PROFESION, VIDEO) VALUES (@ID_USUARIO, @IDENTIFICACION, @NOMBRE, @EMAIL, @DIRECCION, @TELEFONO, @PAIS, @CIUDAD, @PROFESION, @VIDEO)";
                SqlCommand command2 = new SqlCommand(query2, connection);
                command2.Parameters.AddWithValue("@ID_USUARIO", ID_USUARIO_USERS);
                command2.Parameters.AddWithValue("@IDENTIFICACION", Identificacion);
                command2.Parameters.AddWithValue("@NOMBRE", Nombre);
                command2.Parameters.AddWithValue("@EMAIL", Email);
                command2.Parameters.AddWithValue("@DIRECCION", Direccion);
                command2.Parameters.AddWithValue("@TELEFONO", Telefono);
                command2.Parameters.AddWithValue("@PAIS", paisSeleccionado);
                command2.Parameters.AddWithValue("@CIUDAD", ciudadSeleccionada);
                command2.Parameters.AddWithValue("@PROFESION", Profesion);
                command2.Parameters.AddWithValue("@VIDEO", videoBytes);

                command2.ExecuteNonQuery();

                //Obteniendo ID_DESEMPLEADO FROM TABLA DESEMPLEADO 
                string queryID_D = "SELECT ID_DESEMPLEADO FROM DESEMPLEADO WHERE EMAIL = @Email";
                SqlCommand commandID_D = new SqlCommand(queryID_D, connection);
                commandID_D.Parameters.AddWithValue("@EMAIL", Email);
                SqlDataReader readerD = commandID_D.ExecuteReader();
                int ID_DESEMPLEADO = 0;
                if (readerD.Read())
                {
                    ID_DESEMPLEADO = readerD.GetInt32(0);
                }
                readerD.Close();

                //Lista HV
                string query3 = "INSERT INTO HOJA_DE_VIDA (PdfFile, ID_DESEMPLEADO) VALUES (@PdfFile, @ID_DESEMPLEADO)";
                SqlCommand command3 = new SqlCommand(query3, connection);
                command3.Parameters.AddWithValue("@PdfFile", pdfBytes);
                command3.Parameters.AddWithValue("@ID_DESEMPLEADO", ID_DESEMPLEADO);


                command3.ExecuteNonQuery();
            }

            return RedirectToPage("InicioSesion");
        }
    }
}

