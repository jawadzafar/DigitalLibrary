using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary.Controllers
{
    public class ManagerDashboardController : Controller
    {
        // GET: ManagerDashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}