using System;
using System.Collections.Generic;
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
            return View();
        }

        public ActionResult Login()
        {
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
                        return RedirectToAction("Index", "Projects");
                    case UserType.Coordinator:
                        return RedirectToAction("Index", "Coordinator");
                    case UserType.Student:
                        return RedirectToAction("Index", "StudentOperations");
                    default:
                        return RedirectToAction("");
                }
            }
            return View(model);
        }
        public ActionResult SignUp()
        {
            return View();
        }
    }
}