using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace ProjectAspNETv2.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index(int id)
        {
            var userID = User.Identity.GetUserId();
            if (id == 1) { return View("Home"); }
            else if (id == 2) { return View("Vendeurs"); }
            else if (id == 3) { return View("Products"); }
            else if (id == 4) { return View("Statistic"); }
            else if (id == 5) { return View("Historique"); }

            else
            {
                return View("Home");
            }


        }
    }
}