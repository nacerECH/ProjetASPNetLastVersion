using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
            var ProductsToday = new List<Produit>();
            var ProductsWeek = new List<Produit>();
            var  TopProducts = new List<Produit>();



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


            // get top sellers (based on views)

            if (data.Count() >= 3)
            {
              
              TopProducts = data.OrderByDescending(i => i.Vues.Count).Take(3).ToList();

            }
            else
            {
               
                TopProducts = data.OrderByDescending(i => i.Vues.Count).Take(data.Count()).ToList();

            }

            
            var nv = db.Proprietaires.OrderByDescending(i => i.created_at).Take(3).ToList();
            var par = db.Proprietaires.Where(p => p.isCompany == false).Count();
            var soc = db.Proprietaires.Where(p => p.isCompany == true).Count();


            ViewBag.ProdMois = data2.Count();
            ViewBag.prp = db.Proprietaires.Count();
            ViewBag.par = par;
            ViewBag.soc = soc;
            ViewBag.produits = data.Count();
            ViewBag.TopProducts = TopProducts;
            ViewBag.NV = nv;
            ViewBag.ProductsToday = ProductsToday;
            ViewBag.ProductsWeek = ProductsWeek;
            ViewBag.AllProducts = data;
            ViewBag.Ref = data.Where(p => p.status == "3").Count();
            ViewBag.Acc = data.Where(p => p.status == "2").Count();
            ViewBag.Att = data.Where(p => p.status == "1").Count();


            return View("Home");

            }
        
           
            


    


        


        [HttpGet] 
        public ActionResult GetSellers()
        {
            
                // Return the list of sellers from the database
            var data = db.Proprietaires.ToList();
            var supportMsg = db.ContactSupports.ToList();
            var PrBloq = db.Proprietaires.Where(p => p.isBlocked == true).ToList();
            var PrHon = db.Proprietaires.Where(p => p.isHonored == true).ToList();
                ViewBag.MyList = data;
                ViewBag.MyList2 = PrBloq;
                ViewBag.MyList3 = PrHon;
            ViewBag.ProbList = supportMsg;

            return View("Vendeurs");
            
        }

        [HttpGet]
        public ActionResult Message(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var h = db.ContactSupports.Find(id);

                ViewBag.h = h;
                return View("Message");
            }
            

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




        public ActionResult Get_Seller_Statistics(string e)
        {
            var data = db.Proprietaires.ToList();

            ViewBag.e = new SelectList(data, "Id", "Name");


            if (e == "0" || e == null || e == "Select Seller" || e == "" )
            {
                return Redirect(Url.Action("GetStatistics", "Admin"));
            }
            else
            {
                int Selected_Value = int.Parse(e);

                Proprietaire p = db.Proprietaires.Find(Selected_Value);
                ViewBag.s = Selected_Value;

                // ------------------------ STATS1 CAT/VUES

                var vues = db.Vues.ToList();
                var vues2 = new List<int>();
                var cats = db.Categories.ToList();

                List<DataPoint> dataPoints = new List<DataPoint>();


                foreach (Category cat in cats)
                {
                    int s = 0;
                    foreach (Produit pr in cat.Produits)
                    {
                        if (pr.propreitaireId == p.Id)
                        {
                            s = s + pr.Vues.Count;
                        }

                    }
                    vues2.Add(s);
                    dataPoints.Add(new DataPoint(cat.Name, s));

                }


                //-------------------------



                //----------------------Jours/vues

                List<DataPoint> dataPoints2 = new List<DataPoint>();

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

                var data2 = new List<Vue>();

                // initialiser les vues 

                foreach (Vue v in vues)
                {
                    if (v.Produit.propreitaireId == p.Id)
                    {
                        data2.Add(v);
                    }
                }


                int p2 = 0;


                foreach (DateTime dt in Dates)
                {
                    foreach (var v in vues)
                    {

                        DateTime d = (DateTime)v.created_at;
                        if (d.Day == dt.Day)
                        {
                            p2++;
                        }

                    }

                    dataPoints2.Add(new DataPoint(dt.ToString("dd/MM"), p2));


                }



                //----------------------------




                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
                ViewBag.DataPoints2 = JsonConvert.SerializeObject(dataPoints2);
                return View("Statistic");


            }
            
        }


        





        public ActionResult MyChart()
        {
            var data = db.Proprietaires.ToList();
            DateTime dateAuj0 = DateTime.Now;
            DateTime dateAuj1 = dateAuj0.AddDays(-1);
            DateTime dateAuj2 = dateAuj0.AddDays(-2);
            DateTime dateAuj3 = dateAuj0.AddDays(-3);
            DateTime dateAuj4 = dateAuj0.AddDays(-4);
            DateTime dateAuj5 = dateAuj0.AddDays(-5);
            DateTime dateAuj6 = dateAuj0.AddDays(-6);

            var Dates = new List<DateTime>();
            Dates.Add(dateAuj6); Dates.Add(dateAuj5); Dates.Add(dateAuj4); Dates.Add(dateAuj3); Dates.Add(dateAuj2); Dates.Add(dateAuj1); Dates.Add(dateAuj0);

            string[] xv = { dateAuj6.ToString("dd/MM"), dateAuj5.ToString("dd/MM"), dateAuj4.ToString("dd/MM"), dateAuj3.ToString("dd/MM"), dateAuj2.ToString("dd/MM"), dateAuj1.ToString("dd/MM"), dateAuj0.ToString("dd/MM") };
            var yv = new List<int>();
            int c;

            foreach(Proprietaire p in data)
            {

            }

            foreach(DateTime dt in Dates)
            {
                c = 0;
                foreach (Proprietaire p in data)
                {
                    DateTime ddt = (DateTime)p.created_at;
                    if (dt.Month == ddt.Month || dt.Day == ddt.Day || dt.Day == ddt.Day)
                    {
                        c++;
                    }
                }

                yv.Add(c);
                
            }

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
            produit.Images.ToList().Clear();
            produit.Historiques.ToList().Clear();
            produit.Vues.ToList().Clear();
            db.SaveChanges();

            db.Produits.Remove(produit);

            db.SaveChanges();
            return Redirect(Url.Action("GetAllProducts", "Admin"));
        }





    }
    [DataContract]
    internal class DataPoint
    {


        public DataPoint(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;
    }
}