﻿using System;
using System.Collections.Generic;
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


        private Entities4 db = new Entities4();

  
            public ActionResult Index() {

            // var userID = User.Identity.GetUserId();

            return View("Home");

        }
        
           
            


    


        


        [HttpGet] 
        public ActionResult GetSellers()
        {
            using (var context = new Entities4())
            {

                // Return the list of data from the database
                var data = context.Proprietaires.ToList();


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

                return View("Vendeurs");
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
            using (var context = new Entities4())
            {

               
                var data = context.Produits.ToList();

                Dictionary<int, string> dic = new Dictionary<int, string>();

                int id;
                string NomS;
                foreach (Produit p in data)
                {
                     id = p.Id;
                    int idS = (int)p.propreitaireId;
                    Proprietaire pp = db.Proprietaires.Find(idS);
                     NomS = pp.Name;

                    dic.Add(id, NomS);

                }



                ViewBag.MyList = data;
                ViewBag.Dic = dic;


                return View("Products");
            }
        }






        [HttpGet]
        public ActionResult GetProductsNoConfirmed()
        {
            using (var context = new Entities4())
            {

          
                var data = context.Produits.Where(p => p.status=="2").ToList();
                

                Dictionary<int, string> dic = new Dictionary<int, string>();

                int id;
                string NomS;
                foreach (Produit p in data)
                {
                    id = p.Id;
                    int idS = (int)p.propreitaireId;
                    Proprietaire pp = db.Proprietaires.Find(idS);

                    NomS = pp.Name;

                    dic.Add(id, NomS);

                }



                ViewBag.MyList = data;
                ViewBag.Dic = dic;


                return View("ProductConfirmation");
            }
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
                if (etat == "Confirm")
                {
                    p.status = "1";
                }
                if (etat == "Reject")
                {
                    p.status = "3";
                }

                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();

            }
            return Redirect(Url.Action("GetProductsNoConfirmed", "Admin"));
        }






        [HttpGet] 
        public ActionResult GetStatistics()
        {
            using (var context = new Entities4())
            {

                
                var data = context.Proprietaires.ToList();

                ViewBag.e = new SelectList(data, "Id", "Name");


                return View("Statistic");
            }
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




        [HttpGet] 
        public ActionResult GetHistorique()
        {
            using (var context = new Entities4())
            {
                Dictionary<int, string> dic = new Dictionary<int, string>();
                Dictionary<int, string> dic2 = new Dictionary<int, string>();
                
                var data = context.Historiques.ToList();


                int id;
                string NomS,NomP;
                foreach (Historique h in data)
                {
                    id = h.Id;
                    int idS = (int)h.proprietaireId;
                    int idP = (int)h.produitId;
                    Proprietaire pp = db.Proprietaires.Find(idS);
                    Produit pp2 = db.Produits.Find(idP);


                    NomS = pp.Name;
                    NomP = pp2.name;

                    dic.Add(id, NomS);
                    dic2.Add(id, NomP);

                }




                ViewBag.Operations = data;
                ViewBag.Dic1 = dic;
                ViewBag.Dic2 = dic2;


                return View("Historique");
            }
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
    }
}