using BootstrapIntroduction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BootstrapIntroduction.Controllers
{

    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }
        [Route("About")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Route("Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Basic()
        {

            var person = new Person
            {
                FirstName = "Jasoniel",
                LastName = "Santana"
            };

            return View(person);
        }
    }
}