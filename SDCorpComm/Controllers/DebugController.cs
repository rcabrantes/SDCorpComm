using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SDCorpComm.Controllers
{
    public class DebugController : Controller
    {
        // GET api/values
        public ActionResult Usuarios()
        {
            return View(HomeController.usuarios);
        }


    }
}
