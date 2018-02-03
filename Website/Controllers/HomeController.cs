using Constants;
using Managers.Context;
using Scarecrow.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scarecrow.Controllers
{
    [MyLoggingFilter]
    public class HomeController : Controller
    {
        public ActionResult Index() {

            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";
           var smth = Constants.Constants.WebsiteName;
            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //the below method can be replaced with this one, but inclding both calls an abiguity exception
        //[HttpPost]
        //public ActionResult Contact(FormCollection theForm) {
        //    ViewBag.EnteredMessage = theForm["message"] + " to you too";
        //    PartialViewResult pvr = PartialView("ContactResponse");

        //    //ViewBag.htmlMessage = pvr.
        //    return PartialView("ContactResponse");
        //    return View();
        //}

        [HttpPost]
        public ActionResult Contact(string message) {
            ViewBag.Message = message + " to you too";

            return PartialView("ContactResponse");
        }

        [HttpGet]
        public ActionResult GetContactRes(string message) {
            ViewBag.Message = message + " to you too";

            return PartialView("ContactResponse");
        }
    }
}