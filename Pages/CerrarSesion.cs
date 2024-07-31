using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agencia_Bogota.Pages
{
    public class CerrarSesion : PageModel
    {
        public IActionResult OnPost()
        {
            //HttpContext.Response.Cookies.Clear();
            HttpContext.Response.Redirect("Index", true);
            int id = HttpContext.Session.GetInt32("ID") ?? 0;
            Console.WriteLine(id);
            return RedirectToPage("Index");

        }
    }
}
