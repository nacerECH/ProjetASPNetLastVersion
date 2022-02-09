using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProjectAspNETv2.Models;

namespace ProjectAspNETv2.Controllers
{
    [Authorize(Roles ="Marchand")]
    public class ProprietaireController : Controller
    {
        private Entities1 db = new Entities1();


        [HttpGet]
        public ActionResult Index()
        {

            // get l'utlisateur auth
            var id = User.Identity.GetUserId();
            var prop = db.Proprietaires.Single(p => p.UserId == id);
           
            int propID = prop.Id;

            // get les produit de l'utilisateur auth
            var data = db.Produits.Where(p => p.propreitaireId == propID).ToList();
            
            var data2 = new List<Produit>();  // les produits ajoutes ce mois
            var data3 = new List<Produit>();  // les 3 produits ajoutes recement
            var ProductsToday = new List<Produit>(); // les produits de ce jour
            var ProductsWeek = new List<Produit>();
            //var vues = new List<int>();

            List<int> vues = new List<int>();    // liste des vues des produits
            var TopProducts = new List<Produit>();  // top product basee sur les vues


            foreach (Produit p in data)
            {
                TimeSpan dif = (TimeSpan)(DateTime.Now - p.createdAt);

                double jr = dif.TotalDays;

                if (jr <= 30)
                {
                    data2.Add(p);
                }

                if (jr <= 7)
                {
                    ProductsWeek.Add(p);
                }

                if (jr <= 1)
                {
                    ProductsToday.Add(p);
                }

                vues.Add(p.Vues.Count);

            }
            if(data.Count() >= 3)
            {
                data3 = db.Produits.OrderByDescending(i => i.createdAt).Take(3).ToList();
                TopProducts = data.OrderByDescending(i => i.Vues.Count).Take(3).ToList();
            }
            else
            {
                data3 = db.Produits.OrderByDescending(i => i.createdAt).Take(data.Count()).ToList();
                TopProducts = data.OrderByDescending(i => i.Vues.Count).Take(data.Count()).ToList();
            }

            var VuesToday = db.Vues.Where(v => v.Produit.propreitaireId == propID && ((DateTime)v.created_at).Day == DateTime.Now.Day).ToList().Count();
            var VuesMois = db.Vues.Where(v => v.Produit.propreitaireId == propID && ((DateTime)v.created_at).Month == DateTime.Now.Month).ToList().Count();

            
            ViewBag.VuesToday = VuesToday;
            ViewBag.VuesMois = VuesMois;
            ViewBag.AllProducts = data;
            ViewBag.ProdMois = data2.Count();
            ViewBag.TotalProducts = data.Count();
            ViewBag.RecentProducts = data3;
            ViewBag.TopProducts = TopProducts;
            ViewBag.prp = db.Proprietaires.Count();
            ViewBag.produits = db.Produits.Count();
            ViewBag.ProductsToday = ProductsToday;
            ViewBag.ProductsWeek = ProductsWeek;
            

            return View("Home");

        }


        [HttpGet]
        public ActionResult GetAllProducts()
        {
            var id = User.Identity.GetUserId();
            var prop = db.Proprietaires.Single(p => p.UserId == id);

            int propID = prop.Id;
            return Redirect(Url.Action("Index", "Produits",new { id = propID }));
            
        }



        [HttpGet]
        public ActionResult GetProductsNoConfirmed()
        {
            var id = User.Identity.GetUserId();
            var prop = db.Proprietaires.Single(p => p.UserId == id);

            int propID = prop.Id;

            var data = db.Produits.Where(p => p.propreitaireId == propID && (p.status == "1"  || p.status == "3")).ToList();

                ViewBag.MyList = data;
                return View("ProductsRef");
            
        }






        [HttpGet]
        public ActionResult GetStatistics()
        {
            return View("Statistic");
        }






        public ActionResult MyChart()
        {
            var id = User.Identity.GetUserId();
            var prop = db.Proprietaires.Single(p2 => p2.UserId == id);

            int propID = prop.Id;

            DateTime dateAuj0 = DateTime.Now;
            DateTime dateAuj1 = dateAuj0.AddDays(-1);
            DateTime dateAuj2 = dateAuj0.AddDays(-2);
            DateTime dateAuj3 = dateAuj0.AddDays(-3);
            DateTime dateAuj4 = dateAuj0.AddDays(-4);
            DateTime dateAuj5 = dateAuj0.AddDays(-5);
            DateTime dateAuj6 = dateAuj0.AddDays(-6);

            var Dates = new List<DateTime>();
            Dates.Add(dateAuj0);
            Dates.Add(dateAuj1);
            Dates.Add(dateAuj2);
            Dates.Add(dateAuj3);
            Dates.Add(dateAuj4);
            Dates.Add(dateAuj5);
            Dates.Add(dateAuj6);

            var data = new List<Vue>();

            // initialiser les vues 
            var vues2 = new List<int>();
            
            int p = 0;


            foreach (DateTime dt in Dates)
            {
              p =  db.Vues.Where(v => ((DateTime)v.created_at).Day == dt.Day  && v.Produit.propreitaireId == propID).Count();
                vues2.Add(p);

            }
           

            string[] xv = { dateAuj6.ToString("dd/MM"), dateAuj5.ToString("dd/MM"), dateAuj4.ToString("dd/MM"), dateAuj3.ToString("dd/MM"), dateAuj2.ToString("dd/MM"), dateAuj1.ToString("dd/MM"), dateAuj0.ToString("dd/MM") };
            int[] yv = { vues2[6], vues2[5], vues2[4], vues2[3], vues2[2], vues2[1], vues2[0] };

            new System.Web.Helpers.Chart(width: 800, height: 200)
                .AddTitle("Les vues dans les 7 derniers jours")
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
            var id = User.Identity.GetUserId();
            var prop = db.Proprietaires.Single(p => p.UserId == id);

            int propID = prop.Id;

            // initialiser les vues 

            var vues = db.Vues.ToList();
            var vues2 = new List<int>();
            var cats = db.Categories.ToList();

            



            foreach (Category cat in cats)
            {
                int s = 0;
                foreach (Produit pr in cat.Produits)
                {
                     if(pr.propreitaireId == propID)
                     {
                         s = s + pr.Vues.Count;
                     }

                }
                vues2.Add(s);

            }


            List<string> xv = new List<string>();
            List<int> yv = new List<int>();

            foreach(Category cat in cats)
            {
                xv.Add(cat.Name);
            
            }
            foreach (int vu in vues2)
            {
                yv.Add(vu);
            }



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





        [HttpGet]
        public ActionResult GetHistorique()
        {
            var id2 = User.Identity.GetUserId();
            var prop = db.Proprietaires.Single(p => p.UserId == id2);
            int propID = prop.Id;
      
                var data = db.Historiques.Where(h => h.proprietaireId == propID).ToList();


                ViewBag.Operations = data;
              

                return View("Historique");
            

        }



        [HttpPost]

        public ActionResult DeleteOperation(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Historique h = db.Historiques.Find(id);

            db.Historiques.Remove(h);
            db.SaveChanges();

            return Redirect(Url.Action("GetHistorique", "Proprietaire"));
        }



        [HttpGet]
        public ActionResult Support()
        {
            return View("Support");
        }

        [HttpPost]

        public ActionResult SendMessageToSupport(string email, string subject, string message)
        {
            var id = User.Identity.GetUserId();
            var prop = db.Proprietaires.Single(p => p.UserId == id);
            int propID = prop.Id;


            ContactSupport cs = new ContactSupport();
            cs.Email = email;
            cs.subject = subject;
            cs.message = message;
            cs.marchandId = propID;
            int ids = db.ContactSupports.Count(); ids++;
            cs.Id = ids;
            if (ModelState.IsValid)
            {
                db.ContactSupports.Add(cs);
                db.SaveChanges();

            }
            return Redirect(Url.Action("Support", "Proprietaire"));
        }

        public ActionResult Profile()
        {
            var idu = User.Identity.GetUserId();
            var prop = db.Proprietaires.Where(p => p.UserId == idu).FirstOrDefault();
            return View(prop);
        }
    }
}