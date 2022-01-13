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
            else if (id == 6) { return View("ProductsRef"); }

            else
            {
                return View("Home");
            }


        }



        public ActionResult MyChart()
        {

            string[] xv = { "Lundi", "Mardi", "Mercredi", "jeudi", "Vendredi", "Samedi", "Dimenche" };
            int[] yv = { 150, 140, 160, 95, 50, 76, 250 };

            new System.Web.Helpers.Chart(width: 800, height: 200)
                .AddTitle("Total des vues selon les jours")
                .AddSeries(
                   chartType: "Column",
                   xValue: xv,
                   xField:"Les vues",
                   yValues: yv,
                   yFields:"Les jours"
                ).Write("png");

            return null;



        }


        public ActionResult MyChart2()
        {

            string[] xv = { "Cat1", "Cat2", "Cat3", "Cat4", "Cat5", "Cat6", "Cat7" };
            int[] yv = { 250, 100, 12, 75, 5, 200, 850 };

            new System.Web.Helpers.Chart(width: 800, height: 200)
                .AddTitle("Total des vues selon les categories")
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