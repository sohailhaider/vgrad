using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VGrad_Empty.Models;

namespace VGrad_Empty.Controllers
{
    public class OtherProjectsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: OtherProjects
        public ActionResult Index()
        {
            return View(db.OtherProjects.ToList());
        }

        // GET: OtherProjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherProject otherProject = db.OtherProjects.Find(id);
            if (otherProject == null)
            {
                return HttpNotFound();
            }
            return View(otherProject);
        }

        // GET: OtherProjects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OtherProjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OtherProjectId,Title,Description,Organization")] OtherProject otherProject)
        {
            if (ModelState.IsValid)
            {
                db.OtherProjects.Add(otherProject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(otherProject);
        }

        // GET: OtherProjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherProject otherProject = db.OtherProjects.Find(id);
            if (otherProject == null)
            {
                return HttpNotFound();
            }
            return View(otherProject);
        }

        // POST: OtherProjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OtherProjectId,Title,Description,Organization")] OtherProject otherProject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(otherProject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(otherProject);
        }

        // GET: OtherProjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OtherProject otherProject = db.OtherProjects.Find(id);
            if (otherProject == null)
            {
                return HttpNotFound();
            }
            return View(otherProject);
        }

        // POST: OtherProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OtherProject otherProject = db.OtherProjects.Find(id);
            db.OtherProjects.Remove(otherProject);
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
