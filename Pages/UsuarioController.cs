using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agencia_Bogota.Pages;

namespace Agencia_Bogota.Pages
{
    public class UsuarioController
    {
        [HttpGet]
        public ActionResult Registro ()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        private ActionResult View()
        {
            throw new NotImplementedException();
        }
    }
}
