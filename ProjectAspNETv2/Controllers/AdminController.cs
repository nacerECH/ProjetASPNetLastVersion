using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProjectAspNETv2.Models;

namespace ProjectAspNETv2.Controllers
{
    public class AdminController : Controller
    {


        private Entities1 db = new Entities1();

        [HttpGet]
        public ActionResult Index() {

            // var userID = User.Identity.GetUserId();

            var data = db.Produits.ToList();
            var vues = new List<int>();


            var data2 = new List<Produit>();
            var data3 = new List<Produit>();
            var ProductsToday = new List<Produit>();
            var ProductsWeek = new List<Produit>();
            var TopProducts = new List<Produit>();

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

            int i = data.Count();
            data3.Add(data[i-1]);
            data3.Add(data[i - 2]);
            data3.Add(data[i - 3]);



            // get top products (based on views)


            // get top products (based on views)
            int max1 = vues.Max();
            vues.RemoveAll(item => item == max1);
            int max2 = vues.Max();
            vues.RemoveAll(item => item == max2);
            int max3 = vues.Max();
            vues.RemoveAll(item => item == max3);

            foreach (Produit p2 in data)
            { 
            
                if(p2.Vues.Count == max1)
                {
                    TopProducts.Add(p2);
                }
                if (p2.Vues.Count == max2)
                {
                    TopProducts.Add(p2);
                }
                if (p2.Vues.Count == max3)
                {
                    TopProducts.Add(p2);
                }

            }

            TopProducts = TopProducts.OrderBy(p => p.Vues.Count).Reverse().ToList();



            ViewBag.ProdMois = data2.Count();
            ViewBag.RecentProducts = data3;
          

            ViewBag.prp = db.Proprietaires.Count();
            ViewBag.produits = db.Produits.Count();

            ViewBag.TopProducts = TopProducts;

            ViewBag.ProductsToday = ProductsToday;
            ViewBag.ProductsWeek = ProductsWeek;
            ViewBag.AllProducts = data;
            

            return View("Home");

            }
        
           
            


    


        


        [HttpGet] 
        public ActionResult GetSellers()
        {
            
                // Return the list of data from the database
            var data = db.Proprietaires.ToList();
            var supportMsg = db.ContactSupports.ToList();




            // Return the list of data from the database
            var data2 = new List<Proprietaire>();
                var data3 = new List<Proprietaire>();

                for (int i = 0; i < data.Count(); i++)
                {
                    if ((bool)  data[i].isHonored)
                    {
                        data2.Add(data[i]);

                    }
                    if ((bool)data[i].isBlocked)
                    {
                        data3.Add(data[i]);

                    }

                }
                ViewBag.MyList = data;
                ViewBag.MyList2 = data2;
                ViewBag.MyList3 = data3;
            ViewBag.ProbList = supportMsg;


            return View("Vendeurs");
            
        }




 


        [HttpPost]

        public ActionResult BD(int id,string etat)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proprietaire p = db.Proprietaires.Find(id);
            if (ModelState.IsValid)
            {
                if(etat == "isBloc") {
                    p.isBlocked = false;
                }
                if(etat== "isDBloc")
                {
                    p.isBlocked = true;
                }
                
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

            }
            return Redirect(Url.Action("GetSellers", "Admin"));
        }


        [HttpPost]
        public ActionResult HD(int id, string etat)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proprietaire p = db.Proprietaires.Find(id);
            if (ModelState.IsValid)
            {
                if (etat == "isHon")
                {
                    p.isHonored = false;
                }
                if (etat == "isNHon")
                {
                    p.isHonored = true;
                }

                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

            }
            return Redirect(Url.Action("GetSellers", "Admin"));
        }










        [HttpGet] 
        public ActionResult GetAllProducts()
        {
                          
                var data = db.Produits.ToList();
                ViewBag.MyList = data;

                return View("Products");
            
        }






        [HttpGet]
        public ActionResult GetProductsNoConfirmed()
        {
                var data = db.Produits.Where(p => p.status=="1").ToList();
                
                ViewBag.MyList = data;
                return View("ProductConfirmation");
            
        }








        [HttpPost]

        public ActionResult Confirm_Reject_Product(int id, string etat)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit p = db.Produits.Find(id);
            if (ModelState.IsValid)
            {
                Historique h = new Historique();
                h.Produit = p;
                h.Proprietaire = p.Proprietaire;

                if (etat == "Confirm")
                {
                    p.status = "2";
                    
                    //------------- Instancier Historique

                    h.operation = "Confirmation";
                    h.operation_date = DateTime.Now;

                    //-------------------------

                }
                else
                {
                    p.status = "3";

                    //------------- Instancier Historique

                    h.operation = "Rejet";
                    h.operation_date = DateTime.Now;
                    //-------------------------
                }
                db.Historiques.Add(h);
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

            }
            return Redirect(Url.Action("GetProductsNoConfirmed", "Admin"));
        }






        [HttpGet] 
        public ActionResult GetStatistics()
        {
           
                var data = db.Proprietaires.ToList();

                ViewBag.e = new SelectList(data, "Id", "Name");

                return View("Statistic");
            
        }


        [HttpGet]

        public ActionResult Get_Seller_Statistics(string e)
        {
            var data = db.Proprietaires.ToList();

            ViewBag.e = new SelectList(data, "Id", "Name");

            int Selected_Value = int.Parse(e);
            Proprietaire p = db.Proprietaires.Find(Selected_Value);
            ViewBag.s = Selected_Value;
            
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




        [HttpGet] 
        public ActionResult GetHistorique()
        {
                               
                var data = db.Historiques.ToList();
                ViewBag.Operations = data;
                return View("Historique");
           
        }


        [HttpPost]

        public ActionResult DeleteOperation(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Historique h = db.Historiques.Find(id);

            db.Historiques.Remove(h);
                db.SaveChanges();

            return Redirect(Url.Action("GetHistorique", "Admin"));
        }



        [HttpPost]

        public ActionResult DeleteMessageSupport(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            db.ContactSupports.Remove(db.ContactSupports.Find(id));
            db.SaveChanges();

            return Redirect(Url.Action("GetSellers", "Admin"));
        }




        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produit produit = db.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }
            ViewBag.Produit = produit;
            return View("Details");
        }


        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {


            Produit produit = db.Produits.Find(id);
            produit.Images.Clear();
            produit.Historiques.Clear();
            produit.Vues.Clear();

            /*
            var images = produit.Images;
            foreach(var image in images)
            {
                //var i = image;
                db.Images.Remove(image);
                db.SaveChanges();

            }

            var vues = produit.Vues;
            foreach (var v in vues)
            {
                //var i = image;
                db.Vues.Remove(v);
                db.SaveChanges();

            }

            var hs = produit.Historiques;
            foreach (var h in hs)
            {
                //var i = image;
                db.Historiques.Remove(h);
                db.SaveChanges();

            }

            */




            db.Produits.Remove(produit);

            db.SaveChanges();
            return Redirect(Url.Action("GetAllProducts", "Admin"));
        }

    }
}