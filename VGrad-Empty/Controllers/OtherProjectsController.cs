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
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = Convert.ToInt32(Session["UserId"]);
            return View(db.OtherProjects.Include("Student").Where(s => s.Student.User.UserId == userId).ToList());
        }

        // GET: OtherProjects/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
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
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: OtherProjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OtherProjectId,Title,Description,Organization")] OtherProject otherProject)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var student = db.Students.Where(s => s.User.UserId == userId).FirstOrDefault();
                if (student == null)
                {
                    TempData["msg"] = "No student information found!";
                    return RedirectToAction("Login", "Home");
                }
                student.OtherProjects.Add(otherProject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(otherProject);
        }

        // GET: OtherProjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int userId = Convert.ToInt32(Session["UserId"]);
            OtherProject otherProject = db.OtherProjects.Include("Student").Where(s => s.OtherProjectId == id).FirstOrDefault();

            if (otherProject.Student.User.UserId != userId)
            {
                TempData["msg"] = "Kindly login with connected account";
                RedirectToAction("Login", "Home");
            }
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
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
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
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int userId = Convert.ToInt32(Session["UserId"]);
            OtherProject otherProject = db.OtherProjects.Include("Student").Where(s => s.OtherProjectId == id).FirstOrDefault();

            if (otherProject.Student.User.UserId != userId)
            {
                TempData["msg"] = "Kindly login with connected account";
                RedirectToAction("Login", "Home");
            }
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
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = Convert.ToInt32(Session["UserId"]);
            OtherProject otherProject = db.OtherProjects.Include("Student").Where(s => s.OtherProjectId == id).FirstOrDefault();

            if (otherProject.Student.User.UserId != userId)
            {
                TempData["msg"] = "Kindly login with connected account";
                RedirectToAction("Login", "Home");
            }
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
