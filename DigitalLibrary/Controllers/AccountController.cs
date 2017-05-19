﻿using DigitalLibrary.Models;
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
        public ActionResult Login()
        {
            return View();
        }
    }
}