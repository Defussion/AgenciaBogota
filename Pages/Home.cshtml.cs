using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Http;
using System.Reflection.PortableExecutable;


namespace Agencia_Bogota.Pages
{
    public class HomeModel : BasePageModel
    {
        private readonly string _connectionString = "Data Source=tcp:dbsebas.database.windows.net,1433;Initial Catalog=Prueba2023;User Id=SebasLam@dbsebas;Password=Sebasdb24-";

        public byte[] PdfFile { get; set; }
        public string PdfFileName { get; set; }


        public override void OnGet()
        {
            base.OnGet();

            int id = HttpContext.Session.GetInt32("ID") ?? 0;
            int ID_D = 0;
            string queryID_D = "SELECT ID_DESEMPLEADO FROM DESEMPLEADO WHERE ID_USUARIO = @ID";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(queryID_D, connection);
                command.Parameters.AddWithValue("@ID", id); 
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ID_D = reader.GetInt32(0);
                    Console.WriteLine(ID_D);
                }
            }             

            string query = "SELECT PdfFile FROM HOJA_DE_VIDA WHERE ID_DESEMPLEADO = @ID_D";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //HttpContext.Session.GetInt32("ID");
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID_D", ID_D);
                Console.WriteLine(id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    PdfFile = (byte[])reader["PdfFile"];
                    PdfFileName = "archivo.pdf";
                }

                reader.Close();
            }
        }
    }
}
