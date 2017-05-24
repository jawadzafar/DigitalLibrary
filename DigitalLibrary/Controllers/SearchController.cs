using DigitalLibrary.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        [HttpGet]
        //[Role(new string[] { "admin", "manager", "user" })]
        public ActionResult IndexSearch()
        {

            return View();
        }

        [HttpPost]
        //[Role(new string[] { "admin", "manager", "user" })]
        public ActionResult IndexSearch(FormCollection form)
        {
            var word = form["SearchText"].ToString();
            var list = Database_Helpers.QueryList("SELECT Abwaabs.Name as BaabName, Pages.PageNumberDisplay, Books.Name as BookName, Pages.PageDetails, Pages.PageTag FROM Pages Join Books ON Pages.BookId = Books.Id Join Abwaabs ON Pages.BaabId = Abwaabs.Id Where Pages.PageDetails Like '%"+word+"%'");
            return View(list);
        }
    }
}