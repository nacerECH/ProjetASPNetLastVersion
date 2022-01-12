using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectAspNETv2.Controllers
{
    public class WelcomeController : Controller
    {
        // GET: Welcome
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }
        public ActionResult Signin()
        {
            return View();
        }
        public ActionResult Shop()
        {
            return View();
        }
        public ActionResult Product()
        {
            return View();
        }
    }
}