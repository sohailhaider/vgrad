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
    public class UsersController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,Name,Email,Password,Type")] User user)
        {
            if (ModelState.IsValid)
            {
                var ExistingUser = db.Users.Where(s => s.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if(ExistingUser!=null)
                {
                    TempData["msg"] = "Email already registered!";
                    return View(user);
                }
                
                TempData["msg"] = "New User Added";
                user.Password = MD5Hasher.Encrypt(user.Password, "vgrad");
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.Password = MD5Hasher.Decrypt(user.Password, "vgrad");
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Name,Email,Password,Type")] User user)
        {
            if (ModelState.IsValid)
            {
                var userWithEmail = db.Users.Where(s => s.Email.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if(userWithEmail.UserId != user.UserId)
                {
                    TempData["msg"] = "Email already registered to another user";
                    return View(user);
                }
                
                user.Password = MD5Hasher.Encrypt(user.Password, "vgrad");
                var usr = db.Users.Where(s => s.UserId == user.UserId).FirstOrDefault();
                var student = db.Students.Where(s => s.StudentId == user.UserId).FirstOrDefault();
                if (student != null)
                {
                    if (usr.Type==UserType.Student && user.Type!=UserType.Student)
                    {
                        TempData["msg"] = "Please delete corresponding student details first from student tab";
                        return RedirectToAction("Index", "Users");
                    }                        
                }



                usr.Name = user.Name;
                usr.Email = user.Email;
                usr.Password = user.Password;
                usr.Type = user.Type;

                TempData["msg"] = "User Updated Successfully";
                //db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var student = db.Students.Where(s => s.StudentId == id).FirstOrDefault();
            if(student!=null)
            {
                TempData["msg"] = "Please delete corresponding student details first from student tab";
                return RedirectToAction("Index", "Users");
            }
            TempData["msg"] = "User Deleted Successfully";
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
