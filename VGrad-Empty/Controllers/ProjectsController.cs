using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VGrad_Empty.Models;
using VGrad_Empty.Models.ViewModels;

namespace VGrad_Empty.Controllers
{
    public class ProjectsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Projects
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
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
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
            Project project = db.Projects.Include("GroupMembers").Where(s=>s.ProjectId == id).FirstOrDefault();
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
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
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,Title,ProjectDescription,SupervisorName,Batch")] Project project)
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
                db.Projects.Add(project);
                db.SaveChanges();
                TempData["msg"] = "Project Created Successfully";
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() == UserType.Student.ToString())
            {
                var userId = Convert.ToInt32(Session["UserId"]);
                var user = project.GroupMembers.Where(s => s.User.UserId == userId).FirstOrDefault();
                if (user == null)
                {
                    TempData["msg"] = "Sorry but you can't edit anyone else project!";
                    return RedirectToAction("Login", "Home");
                }
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProjectId,Title,ProjectDescription,SupervisorName,Batch")] Project project)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if(Session["UserType"].ToString() == UserType.Student.ToString())
            {
                var userId = Convert.ToInt32(Session["UserId"]);
                var user = project.GroupMembers.Where(s=>s.User.UserId == userId).FirstOrDefault();
                if(user ==null)
                {
                    TempData["msg"] = "Sorry but you can't edit anyone else project!";
                    return RedirectToAction("Login", "Home");
                }
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Project Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() == UserType.Student.ToString())
            {
                var userId = Convert.ToInt32(Session["UserId"]);
                var user = project.GroupMembers.Where(s => s.User.UserId == userId).FirstOrDefault();
                if (user == null)
                {
                    TempData["msg"] = "Sorry but you can't edit anyone else project!";
                    return RedirectToAction("Login", "Home");
                }
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
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() == UserType.Student.ToString())
            {
                var userId = Convert.ToInt32(Session["UserId"]);
                var user = project.GroupMembers.Where(s => s.User.UserId == userId).FirstOrDefault();
                if (user == null)
                {
                    TempData["msg"] = "Sorry but you can't edit anyone else project!";
                    return RedirectToAction("Login", "Home");
                }
            }
            else if (Session["UserType"].ToString() != UserType.Administrator.ToString() && Session["UserType"].ToString() != UserType.Coordinator.ToString())
            {
                TempData["msg"] = "You don't have enough rights";
                return RedirectToAction("Login", "Home");
            }
            db.Projects.Remove(project);
            TempData["msg"] = "Project Deleted Successfully";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddStudentToProject()
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
            ViewBag.StudentID = new SelectList(db.Students.Include("Project").Where(s => s.Project == null).Select(s => new { StudentId = s.StudentId, FieldName = s.User.Name + "(" + s.User.Email + ")" }), "StudentId", "FieldName");
            ViewBag.ProjectID = new SelectList(db.Projects.Where(s => s.GroupMembers.Count <= 4).Select(s => new { ProjectId = s.ProjectId, FieldName = s.Title + "(Project-ID: " + s.ProjectId + ")" }), "ProjectId", "FieldName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudentToProject(StudentToProjectViewModel model)
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
            ViewBag.StudentID = new SelectList(db.Students.Include("Project").Where(s => s.Project == null).Select(s => new { StudentId = s.StudentId, FieldName = s.User.Name + "(" + s.User.Email + ")" }), "StudentId", "FieldName");
            ViewBag.ProjectID = new SelectList(db.Projects.Where(s => s.GroupMembers.Count <= 4).Select(s => new { ProjectId = s.ProjectId, FieldName = s.Title + "(Project-ID: " + s.ProjectId + ")" }), "ProjectId", "FieldName");
            if (ModelState.IsValid)
            {
                //db.Projects.Add(model);
                var Project = db.Projects.Include("GroupMembers").Where(s => s.ProjectId == model.ProjectID).FirstOrDefault();
                if (Project == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Project Selection");
                    TempData["msg"] = "Invalid Project Selection";
                    return View(model);
                }
                var Student = db.Students.Where(s => s.StudentId == model.StudentID).FirstOrDefault();
                if (Student == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid StudentSelection");
                    TempData["msg"] = "Invalid Student Selection";
                    return View(model);
                }
                var exists = Project.GroupMembers.Where(s => s.StudentId == Student.StudentId).FirstOrDefault();
                if (exists != null)
                {
                    ModelState.AddModelError(string.Empty, "Student is already in this Project");
                    TempData["msg"] = "Student is already in this Project";
                    return View(model);

                }
                Project.GroupMembers.Add(Student);
                TempData["msg"] = "Student assigned to Project";

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult RemoveStudent(StudentToProjectViewModel model)
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
                var Project = db.Projects.Include("GroupMembers").Where(s => s.ProjectId == model.ProjectID).FirstOrDefault();
                if (Project == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Project Selection");
                    TempData["msg"] = "Invalid Project Selection";
                    return RedirectToAction("Index");
                }
                var Student = db.Students.Where(s => s.StudentId == model.StudentID).FirstOrDefault();
                if (Student == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Student Selection");
                    TempData["msg"] = "Invalid Student Selection";
                    return RedirectToAction("Index");
                }
                var exists = Project.GroupMembers.Where(s => s.StudentId == Student.StudentId).FirstOrDefault();
                if (exists == null)
                {
                    ModelState.AddModelError(string.Empty, "Student is already in this Project");
                    TempData["msg"] = "Student is already in this Project";
                    return RedirectToAction("Index");
                }
                Project.GroupMembers.Remove(exists);
                db.SaveChanges();
                TempData["msg"] = "Student Removed From Project";
                return RedirectToAction("Index");
            }
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
