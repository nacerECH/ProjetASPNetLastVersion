using Microsoft.AspNet.Identity;
using ProjectAspNETv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjectAspNETv2.Controllers
{
    public class WelcomeController : Controller
    {
        private Entities1 DB = new Entities1();

        // GET: Welcome
        public ActionResult Index()
        {
            

            return View();
        }
        /*public JsonResult GetSearchData()
        {

        }*/
  
        
        public ActionResult Shop()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId(); 
            }


            var data = DB.Proprietaires.ToList();
            var distinctData = new List<Proprietaire>();
            
            var villes = DB.Proprietaires.Select(p=> p.Ville).ToList();

            for (int i = 0; i < data.Count; i++)
            {
                if (villes.Contains(data[i].Ville))
                {
                    distinctData.Add(data[i]);
                    villes.RemoveAll(item => item == data[i].Ville);
                }

            }

            ViewBag.Villes = new SelectList(distinctData, "Id", "Ville");
            ViewBag.Categories = new SelectList(DB.Categories, "CatId", "Name");  
            var produits = DB.Produits;

            return View(produits.ToList());
        }
        public ActionResult Product(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Produit produit = DB.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }

            Vue vue = new Vue();
            vue.created_at = DateTime.Now;
            produit.Vues.Add(vue);

            return View(produit);
        }

        
    }
}