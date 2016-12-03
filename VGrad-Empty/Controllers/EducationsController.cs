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
    public class EducationsController : Controller
    {
        private DataContext db = new DataContext();
        // GET: Educations
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            int userId = Convert.ToInt32(Session["UserId"]);
            return View(db.Educations.Include("Student").Where(s=>s.Student.User.UserId == userId).ToList());
        }

        // GET: Educations/Details/5
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
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // GET: Educations/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        // POST: Educations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EducationId,Title,College,StartDate,EndDate")] Education education)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                var student = db.Students.Where(s => s.User.UserId == userId).FirstOrDefault();
                if(student == null)
                {
                    TempData["msg"] = "No student information found!";
                    return RedirectToAction("Login", "Home");
                }
                student.Educations.Add(education);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(education);
        }

        // GET: Educations/Edit/5
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
            Education education = db.Educations.Include("Student").Where(s=>s.EducationId == id).FirstOrDefault();

            if (education.Student.User.UserId != userId)
            {
                TempData["msg"] = "Kindly login with connected account";
                RedirectToAction("Login", "Home");
            }
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // POST: Educations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EducationId,Title,College,StartDate,EndDate")] Education education)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(education).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(education);
        }

        // GET: Educations/Delete/5
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
            Education education = db.Educations.Find(id);
            if (education == null)
            {
                return HttpNotFound();
            }
            return View(education);
        }

        // POST: Educations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Education education = db.Educations.Find(id);
            db.Educations.Remove(education);
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
