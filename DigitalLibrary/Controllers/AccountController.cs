using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace DigitalLibrary.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpGet]
        public ActionResult Register()
        {
            return View(new Users());
        }

        [HttpPost]
        public ActionResult Register(Users user)
        {
            if (Users.Save(user))
            {
                if(user.Role == "manager")
                {
                    return RedirectToAction("Index","ManagerDashboard");
                }
                else
                {
                    return RedirectToAction("Index", "UserDashboard");
                }
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult Login(Users user)
        {
            if (Users.Exist(user)!=null)
            {

                Session["User"] = user;

                if (user.Role == "admin")
                {
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else if (user.Role == "manager")
                {
                    return RedirectToAction("Index", "ManagerDashboard");
                }
                else if (user.Role == "user")
                {
                    return RedirectToAction("Index", "UserDashboard");
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
    }
}