using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectAspNETv2.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace ProjectAspNETv2.Controllers
{
    public class ProprietairesController : Controller
    {
        private Entities4 db = new Entities4();
        

        // GET: Proprietaires
        public async Task<ActionResult> Index()
        {
            
            var proprietaires = db.Proprietaires.Include(p => p.AspNetUser);
            return View(await proprietaires.ToListAsync());
        }

        // GET: Proprietaires/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proprietaire proprietaire = await db.Proprietaires.FindAsync(id);
            if (proprietaire == null)
            {
                return HttpNotFound();
            }
            return View(proprietaire);
        }

        // GET: Proprietaires/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Proprietaires/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,isCompany,Tel,Adresse,Ville,imageFile")] Proprietaire proprietaire)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(proprietaire.imageFile.FileName);
                string extension = Path.GetExtension(proprietaire.imageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                proprietaire.Logo = "~/PropImages/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/PropImages/"), fileName);
                proprietaire.imageFile.SaveAs(fileName);
                db.Proprietaires.Add(proprietaire);

                await db.SaveChangesAsync();
                ModelState.Clear();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", proprietaire.UserId);
            return View(proprietaire);
        }

        // GET: Proprietaires/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proprietaire proprietaire = await db.Proprietaires.FindAsync(id);
            if (proprietaire == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", proprietaire.UserId);
            return View(proprietaire);
        }

        // POST: Proprietaires/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,UserId,isCompany,Tel,Adresse,Ville,Logo,isHonored,isBlocked")] Proprietaire proprietaire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proprietaire).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", proprietaire.UserId);
            return View(proprietaire);
        }

        // GET: Proprietaires/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proprietaire proprietaire = await db.Proprietaires.FindAsync(id);
            if (proprietaire == null)
            {
                return HttpNotFound();
            }
            return View(proprietaire);
        }

        // POST: Proprietaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Proprietaire proprietaire = await db.Proprietaires.FindAsync(id);
            db.Proprietaires.Remove(proprietaire);
            await db.SaveChangesAsync();
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
