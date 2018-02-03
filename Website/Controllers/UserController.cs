using DataManager.Auth;
using Managers.Context;
using Managers.Managers;
using Managers.Models;
using Scarecrow.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utils.Time;

namespace Scarecrow.Controllers
{
    public class UserController : Controller
    {
        MainDataContext db = new MainDataContext();
        private const string ViewPath = "/Views/User/";

        private bool SetLogin() {
            AuthUser au = new AuthUser(db);
            ViewBag.User = au.Authorise();
            return ViewBag.User != null;
        }

        // GET: CheckingAccount
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckingAccount/feed
        public ActionResult Feed()
        {
            return View();
        }

        // GET: CheckingAccount/Create
        public ActionResult Signup()
        {
            return View();
        }

        // POST: CheckingAccount/Create
        [HttpPost]
        public ActionResult Signup(UserViewModels.Signup model)
        {
            UserManager userM = new UserManager(db);

            //check the username for uniqueness
            bool usernameExists = userM.GetUserByUsername(model.Username) != null;
            if (usernameExists) {
                ModelState.AddModelError("Username", "Username is not unique");
            }

            //check the email address for uniqueness
            bool emailExists = userM.GetUserByEmail(model.EmailAddress) != null;
            if (emailExists) {
                ModelState.AddModelError("EmailAddress", "Email is not unique");
            }

            if (ModelState.IsValid)
            {
                User newUser = userM.CreateNewUser(model.Username, model.EmailAddress, model.Password);

                db.Users.Add(newUser);
                db.SaveChanges();

                EmailManager em = new EmailManager();
                em.SendVerificationEmail(newUser);

                return RedirectToAction("VerifyEmail");
            }
            else { return View(); }

        }

        public ActionResult Login() { return View(); }

        public ActionResult Details() { if (SetLogin()) { return View(); } else { return RedirectToAction("Index", "Home"); } }

        [HttpPost]
        public ActionResult Login(UserViewModels.Login model) {
            AuthUser aa = new AuthUser(db);

            bool success = aa.Login(model.EmailAddress, model.Password);

            if (success) { return RedirectToAction("Details", aa.Authorise()); }
            else {
                ModelState.AddModelError("EmailAddress", "That email and password combination was not recognised");
                return View();
            }

        }

        public ActionResult VerfyEmail(string hash) {

            UserManager userM = new UserManager(db);

            Managers.Models.User user = userM.GetAllData().FirstOrDefault(u => u.EmailVerifyHash == hash);

            if (user != null)
            {
                user.EmailVerified = true;
                db.SaveChanges();

                return View(ViewPath + "VerifyThanks.cshtml"); 
            }
            else { return RedirectToAction("Index", "Home", null); }

        }

        // GET: CheckingAccount/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckingAccount/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckingAccount/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckingAccount/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
