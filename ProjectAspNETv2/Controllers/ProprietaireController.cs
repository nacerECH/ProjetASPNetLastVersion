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
        private Entities db = new Entities();


        [HttpGet]
        public ActionResult Index()
        {

            var userID = User.Identity.GetUserId();

            Proprietaire prop = db.Proprietaires.Where(pr => pr.UserId == userID).FirstOrDefault();

            int propID = prop.Id;

            var data = db.Produits.Where(p => p.propreitaireId== propID).ToList();
            var vues = db.Vues.ToList();
            var data2 = new List<Produit>();
            var data3 = new List<Produit>();
            var ProductsToday = new List<Produit>();
            var ProductsWeek = new List<Produit>();
            var VuesProducts = new List<Vue>();

            Dictionary<int, int> dicVP = new Dictionary<int, int>();

            int idd;
            int val;
            foreach (Vue v in vues)
            {
                idd = v.product_id;

                Vue pp = db.Vues.Find((int)v.id);
                val = (int)pp.value;

                dicVP.Add(idd, val);

            }

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

                //  VuesProducts.Add(dicVP[p.Id]);

                Vue vv = db.Vues.Where(v => v.product_id == p.Id).FirstOrDefault();

                VuesProducts.Add(vv);



            }

            int i = data.Count();
            data3.Add(data[i - 1]);
            data3.Add(data[i - 2]);
            data3.Add(data[i - 3]);

            
            

            // get top products (based on views)

            // get top products (based on views)

            

            var items0 = VuesProducts.OrderByDescending(u => u.value).Take(3).ToList();

            int id1 = items0[0].product_id;
            int id2 = items0[1].product_id;
            int id3 = items0[2].product_id;

            Produit p1 = db.Produits.Find(id1);
            Produit p2 = db.Produits.Find(id2);
            Produit p3 = db.Produits.Find(id3);

            var items = new List<Produit>();
            items.Add(p1);
            items.Add(p2);
            items.Add(p3);



            ViewBag.DicVP = dicVP;
            ViewBag.ProdMois = data2.Count();
            ViewBag.TotalProducts = data.Count();
            ViewBag.TopProducts = items;
            ViewBag.RecentProducts = data3;
            
          //  ViewBag.TopProducts = items;

            ViewBag.prp = db.Proprietaires.Count();
            ViewBag.produits = db.Produits.Count();

            ViewBag.ProductsToday = ProductsToday;
            ViewBag.ProductsWeek = ProductsWeek;
            ViewBag.AllProducts = data;

            return View("Home");

        }


        [HttpGet]
        public ActionResult GetAllProducts()
        {
            Proprietaire prop = db.Proprietaires.Where(pr => pr.UserId == User.Identity.GetUserId()).FirstOrDefault();

            int propID = prop.Id;

            var data = db.Produits.Where(p => p.propreitaireId == prop.Id).ToList();


                ViewBag.MyList = data;

                return View("Products");
            
        }



        [HttpGet]
        public ActionResult GetProductsNoConfirmed()
        {

            Proprietaire prop = db.Proprietaires.Where(pr => pr.UserId == User.Identity.GetUserId()).FirstOrDefault();

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
            DateTime dateAuj0 = DateTime.Now;
            DateTime dateAuj1 = dateAuj0.AddDays(-1);
            DateTime dateAuj2 = dateAuj0.AddDays(-2);
            DateTime dateAuj3 = dateAuj0.AddDays(-3);
            DateTime dateAuj4 = dateAuj0.AddDays(-4);
            DateTime dateAuj5 = dateAuj0.AddDays(-5);
            DateTime dateAuj6 = dateAuj0.AddDays(-6);

            string[] xv = { dateAuj6.ToString("dd/MM"), dateAuj5.ToString("dd/MM"), dateAuj4.ToString("dd/MM"), dateAuj3.ToString("dd/MM"), dateAuj2.ToString("dd/MM"), dateAuj1.ToString("dd/MM"), dateAuj0.ToString("dd/MM") };
            int[] yv = { 150, 140, 160, 95, 50, 76, 250 };

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





        [HttpGet]
        public ActionResult GetHistorique()
        {

            Proprietaire prop = db.Proprietaires.Where(pr => pr.UserId == User.Identity.GetUserId()).FirstOrDefault();

            int propID = prop.Id;

            Dictionary<int, string> dic2 = new Dictionary<int, string>();

                var data = db.Historiques.Where(h => h.proprietaireId == propID).ToList();


                int id;
                string NomS, NomP;
                foreach (Historique h in data)
                {
                    id = h.Id;
                    
                    int idP = (int)h.produitId;
                   
                    Produit pp2 = db.Produits.Find(idP);


                   
                    NomP = pp2.name;

                    
                    dic2.Add(id, NomP);

                }




                ViewBag.Operations = data;
                
                ViewBag.Dic2 = dic2;


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
            Proprietaire prop = db.Proprietaires.Where(pr => pr.UserId == User.Identity.GetUserId()).FirstOrDefault();

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



    }
}