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
        private Entities1 db = new Entities1();

        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }
        public ActionResult Signin()
        {
            return View();
        }
        public ActionResult Shop()
        {
            return View();
        }
        public ActionResult Product(int? id)
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

            Vue vue = new Vue();
            vue.created_at = DateTime.Now;
            produit.Vues.Add(vue);

            return View(produit);
        }
    }
}