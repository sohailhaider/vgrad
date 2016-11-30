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
    public class StudentsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Students
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            var students = db.Students.Include(s => s.User);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            ViewBag.StudentId = new SelectList(db.Users.Where(s=>s.Type == UserType.Student).Select(s=>new { UserId = s.UserId, FieldName = s.Name + "(" + s.Email + ")"}), "UserId", "FieldName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,Batch,GraduationDate,ContactNumber,Employeement")] Student student)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var existing = db.Students.Where(s => s.StudentId == student.StudentId).FirstOrDefault();
                if(existing!=null)
                {
                    TempData["msg"] = "Student Record exists coresponding User";
                    return RedirectToAction("Edit", "Students", new { id = student.StudentId });
                }
                db.Students.Add(student);
                db.SaveChanges();
                TempData["msg"] = "Student Record Created Successfully";
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Users.Where(s => s.Type == UserType.Student).Select(s => new { UserId = s.UserId, FieldName = s.Name + "(" + s.Email + ")" }), "UserId", "FieldName", student.StudentId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Users, "UserId", "Name", student.StudentId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,Batch,GraduationDate,ContactNumber,Employeement")] Student student)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Student Information Updated";
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Users, "UserId", "Name", student.StudentId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            TempData["msg"] = "Student Record Deleted, Now you can delete User account!";
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
