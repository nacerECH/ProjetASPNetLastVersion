using Microsoft.AspNet.Identity;
using PagedList;
using ProjectAspNETv2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ProjectAspNETv2.Controllers
{
    public class WelcomeController : Controller
    {
        private Entities1 DB = new Entities1();

        // GET: Welcome
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();
            }


            var data = DB.Proprietaires.ToList();
            var distinctData = new List<Proprietaire>();

            var villes = DB.Proprietaires.Select(p => p.Ville).ToList();

            for (int i = 0; i < data.Count; i++)
            {
                if (villes.Contains(data[i].Ville))
                {
                    distinctData.Add(data[i]);
                    villes.RemoveAll(item => item == data[i].Ville);
                }

            }

            var Villes = new SelectList(distinctData, "Id", "Ville"); 
            



            ViewBag.Villes = new SelectList(distinctData, "Id", "Ville");
            ViewBag.Categories = new SelectList(DB.Categories, "CatId", "Name");
           
            ViewBag.NewProducts = DB.Produits.OrderByDescending(p => p.createdAt).ToList();
            ViewBag.hotProducts = DB.Produits.OrderByDescending(p =>p.Vues.Count).ToList();
            var sProducts= DB.Produits.Where(p => p.promoId != null).OrderByDescending(p => p.Promotion.Pourcentage).ToList();
            ViewBag.specialProducts = sProducts;

            DateTime now = DateTime.Now;
            /*------- getting the newest products section----------*/

            ViewBag.newestProducts = DB.Produits.Where(p => DbFunctions.DiffHours((DateTime)p.createdAt,now) < 24).OrderByDescending(p=>(DateTime)p.createdAt).ToList();

            /*-------end - getting the newest products section----------*/

            /**/

            /* var couplecatproducts = DB.Categories.Select(c => new { C=c, P=c.Produits.OrderByDescending( p =>p.Vues.Count) }).ToArray();
             var category = couplecatproducts.Select(c => c.C).ToList();*/

            var category = DB.Produits.OrderByDescending(p => p.Vues.Count).Select(p => p.Category).ToList();

            List<Category> TopCategories = new List<Category>();
            TopCategories.Add(category.ElementAt(0));
            foreach(var cat in category)
            {
                if (!TopCategories.Contains(cat))
                {
                    TopCategories.Add(cat);
                }
            }

            //all categories //
            ViewBag.AllCategories = DB.Categories.ToList();
            
            
            /*var query = (from c in DB.Categories join p in DB.Produits on c.CatId equals p.categoryId orderby p.Vues.Count descending select c ).ToList();*/

            return View(TopCategories.Take(4));
        }
        /*public JsonResult GetSearchData()
        {

        }*/
  
        
        public ActionResult Shop(int page = 1, int pageSize = 8)
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Identity.GetUserId();
                var currentSeller = DB.Proprietaires.Where(s => s.UserId == id).First();
                ViewBag.CurrentSeller = currentSeller;
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


           
            var produits1 = DB.Produits.Where(p => p.status == "2").OrderByDescending(p =>p.createdAt).ToList();
            PagedList<Produit> produits = new PagedList<Produit>(produits1, page, pageSize); 
            return View(produits);
        }



        public ActionResult Product(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (User.Identity.IsAuthenticated)
            {
                var id2 = User.Identity.GetUserId();
                var currentSeller = DB.Proprietaires.Where(s => s.UserId == id2).First();
                ViewBag.CurrentSeller = currentSeller;
            }

            Produit produit = DB.Produits.Find(id);
            if (produit == null)
            {
                return HttpNotFound();
            }

            Vue vue = new Vue();
            vue.created_at = DateTime.Now;
            produit.Vues.Add(vue);
            DB.SaveChanges();


            //"YOU MAY ALSO LIKE" content in the product view 
            if (produit.Category.Produits.Count >= 4)
            {
                ViewBag.RelatedProducts = DB.Produits.Where(p => p.Category.Name == produit.Category.Name && p.Id != produit.Id).Take(4).ToList();
            }
            else
            {
                int count = produit.Category.Produits.Count;
                ViewBag.RelatedProducts = DB.Produits.Where(p => p.Category.Name == produit.Category.Name && p.Id != produit.Id).Take(count).ToList();
                

            }
            

            return View(produit);
        }

        
    }
}