using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Agencia_Bogota.Pages
{
    public class BasePageModel : PageModel
    {
        public bool IsLoggedIn { get; set; }
        public int LoggedInUserId { get; set; }
        public string LoggedInUserRole { get; set; }

        [HttpGet]
        public virtual void OnGet()
        {
            CheckSession();
        }
        protected void CheckSession()
        {
            IsLoggedIn = HttpContext.Session.GetInt32("ID") != null;
            LoggedInUserId = IsLoggedIn ? HttpContext.Session.GetInt32("ID").Value : 0;
            LoggedInUserRole = IsLoggedIn ? HttpContext.Session.GetString("ROL") : string.Empty;
        }
    }
}
