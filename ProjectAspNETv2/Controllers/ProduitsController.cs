using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ProjectAspNETv2.Models;

namespace ProjectAspNETv2.Controllers
{
    [Authorize(Roles = "Marchand")]
    public class ProduitsController : Controller
    {
        private Entities1 db = new Entities1();

        // GET: Produits

        public ActionResult Index(int id)
        {
            var idu = User.Identity.GetUserId();
            var prop = db.Proprietaires.Where(p => p.UserId == idu).FirstOrDefault();
            if (prop.Id == id)
            {
                var produits = prop.Produits;

                //var produits = db.Produits.Include(p => p.Category).Include(p => p.Promotion).Include(p => p.Proprietaire).Include(p => p.Images);
                return View(produits.ToList());
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


        }

        // GET: Produits/Details/5
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
            return View(produit);
        }

        // GET: Produits/Create
        public ActionResult Create()
        {
            ViewBag.categoryId = new SelectList(db.Categories, "CatId", "Name");
            ViewBag.promoId = new SelectList(db.Promotions, "Id", "Name");
            ViewBag.propreitaireId = new SelectList(db.Proprietaires, "Id", "Name");
            return View();
        }

        // POST: Produits/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,description_,price,name,livrable,garantee,categoryId,isConfirmed")] Produit produit, HttpPostedFileBase[] files)
        {
            var f = files;
            if (files != null && files.Length > 0)
            {
                var supportedImageType = new[] { ".jpg", ".jpeg", ".png" };

                foreach (var file in files)
                {
                    if (!supportedImageType.Contains(Path.GetExtension(file.FileName)))
                    {
                        ViewBag.FileStatus = "Please upload a supported image format";
                        ViewBag.categoryId = new SelectList(db.Categories, "CatId", "Name", produit.categoryId);
                        ViewBag.promoId = new SelectList(db.Promotions, "Id", "Name", produit.promoId);
                        ViewBag.propreitaireId = new SelectList(db.Proprietaires, "Id", "Name", produit.propreitaireId);
                        return View(produit);

                    }
                }
                if (ModelState.IsValid)
                {
                    var id = User.Identity.GetUserId();
                    var prop = db.Proprietaires.Single(p => p.UserId == id);
                    //Where(p => p.UserId == User.Identity.GetUserId());
                    produit.Proprietaire = prop;
                    produit.createdAt = DateTime.Now;
                    produit.status = "1";
                    

                    //------------- Instancier Historique

                    Historique h = new Historique();
                    h.operation = "Ajouter";
                    h.operation_date = DateTime.Now;
                    h.Produit = produit;
                    h.Proprietaire = prop;


                    //-------------------------

                    //------------- Instancier View

                    Vue v = new Vue();
                    v.Produit = produit;
                    v.created_at = DateTime.Now;


                    //-------------------------



                    db.Produits.Add(produit);
                    db.Historiques.Add(h);
                    db.Vues.Add(v);
                    db.SaveChanges();

                    foreach (var file in files)
                    {
                        string extension = Path.GetExtension(file.FileName);
                        var filename = DateTime.Now.ToString("yymmssfff") + extension;
                        var savePath = Path.Combine(Server.MapPath("~/propimages/Products"), filename);
                        file.SaveAs(savePath);
                        Image image = new Image();
                        image.PathName = "/propimages/Products/" + filename;
                        image.Produit = produit;
                        db.Images.Add(image);

                        //proprietaire.Logo = filename;
                    }
                    db.SaveChanges();
                    ViewBag.FileStatus = "File uploaded successfully.";



                   
                    return Redirect(Url.Action("Index", "Produits", new { id = prop.Id }));
                }

                ViewBag.categoryId = new SelectList(db.Categories, "CatId", "Name", produit.categoryId);
                ViewBag.promoId = new SelectList(db.Promotions, "Id", "Name", produit.promoId);
                ViewBag.propreitaireId = new SelectList(db.Proprietaires, "Id", "Name", produit.propreitaireId);
                return View(produit);

            }
            else
            {
                ViewBag.FileStatus = "Please upload some images";
                ViewBag.categoryId = new SelectList(db.Categories, "CatId", "Name", produit.categoryId);
                ViewBag.promoId = new SelectList(db.Promotions, "Id", "Name", produit.promoId);
                ViewBag.propreitaireId = new SelectList(db.Proprietaires, "Id", "Name", produit.propreitaireId);
                return View(produit);
            }

        }

        // GET: Produits/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.categoryId = new SelectList(db.Categories, "CatId", "Name", produit.categoryId);
            ViewBag.promoId = new SelectList(db.Promotions, "Id", "Name", produit.promoId);
            ViewBag.propreitaireId = new SelectList(db.Proprietaires, "Id", "Name", produit.propreitaireId);
            return View(produit);
        }

        // POST: Produits/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,description_,createdAt,propreitaireId,price,name,livrable,garantee,categoryId")] Produit produit)
        {
            var id = User.Identity.GetUserId();
            var prop = db.Proprietaires.Single(p => p.UserId == id);
            if (ModelState.IsValid)
            {
                //------------- Instancier Historique

                Historique h = new Historique();
                h.operation = "Editer";
                h.operation_date = DateTime.Now;
                h.Produit = produit;
                h.Proprietaire = prop;


                //-------------------------
                
                db.Entry(produit).State = EntityState.Modified;
                db.Historiques.Add(h);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.categoryId = new SelectList(db.Categories, "CatId", "Name", produit.categoryId);
            return View(produit);
        }

        // GET: Produits/Delete/5
        public ActionResult Delete(int? id)
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
            return View(produit);
        }

        // POST: Produits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {


            Produit produit = db.Produits.Find(id);


            produit.Images.ToList().Clear();
            produit.Historiques.ToList().Clear();
            produit.Vues.ToList().Clear();
            db.SaveChanges();

            db.Produits.Remove(produit);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
