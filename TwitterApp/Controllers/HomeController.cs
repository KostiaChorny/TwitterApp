using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TwitterApp.Models.Data;
using TwitterApp.Models.TwitterHelpers;

namespace TwitterApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AppContext db = new AppContext();

            return View(db.Users.ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}