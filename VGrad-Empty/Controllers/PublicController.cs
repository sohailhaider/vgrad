using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGrad_Empty.Models;

namespace VGrad_Empty.Controllers
{
    public class PublicController : Controller
    {
        private DataContext db = new DataContext();
        // GET: Public
        public ActionResult Index(string search)
        {
            if(search!=null)
            {
                var students = db.Students.Where(s => s.User.Name.Contains(search) || s.User.Email.Contains(search) || s.BasicInformation.Introduction.Contains(search) || s.Batch.Contains(search)).ToList();
                ViewBag.SearchTerm = search;
                return View(students);
            } else
            {
                var students = db.Students.ToList();
                return View(students);
            }
        }

        public ActionResult CV(int? id)
        {
            if(id==null)
            {
                return RedirectToAction("Index","Public");
            }

            var model = db.Students.Include("Educations").Include("OtherProjects").Include("Project").Where(s => s.StudentId == id).FirstOrDefault();
            if(model == null)
            {
                return RedirectToAction("Index", "Public");
            } else if(model.BasicInformation == null)
            {
                return RedirectToAction("Index", "Public");
            }


            return View(model);
        }
    }
}