using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VGrad_Empty.Models;
using VGrad_Empty.Models.ViewModels;

namespace VGrad_Empty.Controllers
{
    public class HomeController : Controller
    {
        DataContext db = new DataContext(); 
        // GET: Home
        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Index", "Public");
            }
            else if (Session["UserType"].ToString() == UserType.Student.ToString())
            {
                return RedirectToAction("BasicInformation", "Home");
            }
            else if (Session["UserType"].ToString() == UserType.Coordinator.ToString())
            {
                return RedirectToAction("Index", "Projects");
            }
            else if (Session["UserType"].ToString() == UserType.Administrator.ToString())
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }

        public ActionResult BasicInformation()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else if (Session["UserType"].ToString() != UserType.Student.ToString())
            {
                return RedirectToAction("Login", "Home");
            }

            var userId = Convert.ToInt32(Session["UserId"]);
            var student = db.Students.Where(s => s.StudentId == userId).FirstOrDefault();
            if(student == null)
            {
                TempData["msg"] = "Ask Co-ordinator to udpate your student information.";
                return RedirectToAction("Logout", "Home");
            }
            if(student.BasicInformation == null)
            {
                return RedirectToAction("AddBasicInformation", "Home");
            }
            else
            {
                return RedirectToAction("EditBasicInformation", "Home");
            }
        }

        public ActionResult AddBasicInformation()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = Convert.ToInt32(Session["UserId"]);
            var student = db.Students.Where(s => s.StudentId == userId).FirstOrDefault();
            if(student.BasicInformation !=null)
            {
                return RedirectToAction("EditBasicInformation", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddBasicInformation(BasicInformation model, HttpPostedFileBase Image)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var m = model;
                DateTime dateTime = DateTime.Now;
                var userId = Convert.ToInt32(Session["UserId"]);
                if (Image != null)
                {
                    string image1 = Image.FileName;
                    string pre = dateTime.Ticks.ToString() + "_" + userId.ToString();
                    image1 = pre + "_" + image1;
                    model.Image = image1;
                    var image1Path = Path.Combine(Server.MapPath("~/Uploads/Images"), image1);
                    Image.SaveAs(image1Path);
                }
                var student = db.Students.Where(s => s.StudentId == userId).FirstOrDefault();
                student.BasicInformation = model;
                db.SaveChanges();
                TempData["msg"] = "Information Updated!";
                return RedirectToAction("EditBasicInformation", "Home");
            }
            return View(model);
        }
        public ActionResult EditBasicInformation()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var userId = Convert.ToInt32(Session["UserId"]);
            var student = db.Students.Where(s => s.StudentId == userId).FirstOrDefault();
            if(student == null)
            {
                TempData["msg"] = "Ask Co-ordinator to udpate your student information.";
                return RedirectToAction("Logout", "Home");
            }
            if(student.BasicInformation == null)
            {
                return RedirectToAction("AddBasicInformation", "Home");
            }

            return View(student.BasicInformation);
        }
        [HttpPost]
        public ActionResult EditBasicInformation(BasicInformation model, HttpPostedFileBase Image)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var m = model;
                DateTime dateTime = DateTime.Now;
                var userId = Convert.ToInt32(Session["UserId"]);
                var basic = db.BasicInformations.Where(s => s.BasicInformationID == model.BasicInformationID).FirstOrDefault();
                if (Image != null)
                {
                    string image1 = Image.FileName;
                    string pre = dateTime.Ticks.ToString() + "_" + userId.ToString();
                    image1 = pre + "_" + image1;
                    basic.Image = image1;
                    var image1Path = Path.Combine(Server.MapPath("~/Uploads/Images"), image1);
                    Image.SaveAs(image1Path);
                }
                basic.FatherName = model.FatherName;
                basic.Introduction = model.Introduction;
                db.SaveChanges();
                TempData["msg"] = "Information Updated!";
                return RedirectToAction("EditBasicInformation", "Home");
            }
            return View(model);
        }

        public ActionResult Login()
        {
            if (Session["UserId"] == null)
            {
            }
            else if (Session["UserType"].ToString() == UserType.Student.ToString())
            {
                return RedirectToAction("BasicInformation", "Home");
            }
            else if (Session["UserType"].ToString() == UserType.Coordinator.ToString())
            {
                return RedirectToAction("Index", "Projects");
            }
            else if (Session["UserType"].ToString() == UserType.Administrator.ToString())
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var password = MD5Hasher.Encrypt(model.password, "vgrad");
                var user = db.Users.Where(s => s.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase) && s.Password == password).FirstOrDefault();
                if(user == null)
                {
                    TempData["msg"] = "Invalid Email/Password!";
                    return View(model);
                }
                Session["UserName"] = user.Name;
                Session["UserType"] = user.Type;
                Session["UserId"] = user.UserId;
                Session["Email"] = user.Email;
                switch(user.Type)
                {
                    case UserType.Administrator:
                        return RedirectToAction("Index", "Users");
                    case UserType.Coordinator:
                        return RedirectToAction("Index", "Projects");
                    case UserType.Student:
                        return RedirectToAction("BasicInformation", "Home");
                    default:
                        return RedirectToAction("");
                }
            }
            return View(model);
        }
        public ActionResult SignUp()
        {
            if (Session["UserId"] == null)
            {
            }
            else if (Session["UserType"].ToString() == UserType.Student.ToString())
            {
                return RedirectToAction("BasicInformation", "Home");
            }
            else if (Session["UserType"].ToString() == UserType.Coordinator.ToString())
            {
                return RedirectToAction("Index", "Projects");
            }
            else if (Session["UserType"].ToString() == UserType.Administrator.ToString())
            {
                return RedirectToAction("Index", "Users");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(User model)
        {
            if (ModelState.IsValid)
            {
                var password = MD5Hasher.Encrypt(model.Password, "vgrad");
                var user = db.Users.Where(s => s.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (user != null)
                {
                    TempData["msg"] = "Email Address already exists, try new.";
                    return View(model);
                }
                model.Password = password;
                model.Type = UserType.Student;
                model.Status = false;
                db.Users.Add(model);
                db.SaveChanges();
                TempData["msg"] = "Signup successfull but needs co-ordinator attention to assign any other role.";
                return RedirectToAction("Login", "Home");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            Session.Remove("UserId");
            Session.Remove("UserType");
            Session.RemoveAll();
            Session.Clear();
            return RedirectToAction("Login","Home");
        }
    }
}