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
            else if (id == 6) { return View("ProductConfirmation"); }

            else
            {
                return View("Home");
            }


        }



        public ActionResult MyChart()
        {

            string[] xv = { "Lundi", "Mardi", "Mercredi", "jeudi", "Vendredi", "Samedi", "Dimenche" };
            int[] yv = { 7, 2, 2, 4, 18, 14, 20 };

            new System.Web.Helpers.Chart(width: 800, height: 200)
                .AddTitle("les demmandes d'insciption / jours")
                .AddSeries(
                   chartType: "Column",
                   xValue: xv,
                   xField: "Les vues",
                   yValues: yv,
                   yFields: "Les jours"
                ).Write("png");

            return null;



        }
    }
}