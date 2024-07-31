using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Reflection.PortableExecutable;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace Agencia_Bogota.Pages
{
    public class InicioSesionModel : PageModel
    {
        private readonly string _connectionString = "Data Source=tcp:dbsebas.database.windows.net,1433;Initial Catalog=Prueba2023;User Id=SebasLam@dbsebas;Password=Sebasdb24-";

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Rol { get; set; }

        //Lista tabal USERS
        public List<String> Datos = new List<String>();

        [TempData]
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            ErrorMessage = string.Empty;
        }

        public IActionResult OnPost()
        {
            // Validar el inicio de sesión y redirigir según el resultado
            if (ValidarCredenciales(Email, Password, Id, Rol))
            {

                // Inicio de sesión exitoso, redirigir a la página principal
                return RedirectToPage("Home");
            }
            else
            {
                // Credenciales inválidas, mostrar mensaje de error
                ErrorMessage = "Credenciales inválidas: Usuario o Contraseña incorrectos";
                return Page();
            }
        }

        private bool ValidarCredenciales(string email, string password, int id, string rol)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM USERS WHERE USERNAME = @Email AND PASSWORD = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string query2 = "SELECT ID_USUARIO, ROL FROM USERS WHERE USERNAME = @Email";
                        SqlCommand command2 = new SqlCommand(query2, connection);
                        command2.Parameters.AddWithValue("@Email", email);
                        reader.Close();
                        int result = (int)command2.ExecuteScalar();
                        if (result > 0)
                        {
                            SqlDataReader reader2 = command2.ExecuteReader();
                            if (reader2.Read())
                            {
                                int idUsuario = reader2.GetInt32(0);
                                string rolUsuario = reader2.GetString(1);

                                // Asignar los valores a la sesión
                                HttpContext.Session.SetInt32("ID", idUsuario);
                                HttpContext.Session.SetString("ROL", rolUsuario);
                                Console.WriteLine(rolUsuario);
                            }
                            reader2.Close();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
    }
}
