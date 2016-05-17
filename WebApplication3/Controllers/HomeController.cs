using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models.Repository;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        INewsRepository news = new NewsRepository();
        public ActionResult Index()
        {
            
            return View(news.GetNews());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}