using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectAspNETv2.Controllers
{
    public class ProprietaireController : Controller
    {
        // GET: Proprietaire
        public ActionResult Index(int id)
        {
            if (id == 1) { return View("Home"); }
            else if (id == 2) { return View("Products"); }

            else if (id == 3) { return View("Statistic"); }
            else if (id == 4) { return View("Historique"); }
            else if (id == 5) { return View("Support"); }

            else
            {
                return View("Home");
            }


        }
    }
}