using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        [HttpGet]
        public ActionResult Index()
        {
            return View(Page.GetAll());
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View(new Page());
        }
        [HttpPost]
        public ActionResult Add(Page page)
        {
            if (Page.Save(page))
            {
                return RedirectToAction("index", "Pages");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            Page page = new Page();
            string query = "Select * from Pages where Id='"+Id+"'";
            Database_Helpers db = new Database_Helpers();
            try
            {
                db.Connection.Open();
                SqlCommand cmd = new SqlCommand(query, db.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        page = new Page();
                        page.Id = (int)reader["Id"];
                        page.BookId = (int)reader["BookId"];
                        page.BaabId = (int)reader["BaabId"];
                        page.PageDetails = reader["PageDetails"].ToString();
                        page.PageNumberDisplay = reader["PageNumberDisplay"].ToString();
                        page.PageTag = reader["PageTag"].ToString();
                    }
                }
                return View(page);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(page);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(Page page)
        {
            Database_Helpers db = new Database_Helpers();
            if (db.Update("Pages", page, "where Id='"+page.Id+"'"))
            {
                return RedirectToAction("Index","Pages");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            Page page = new Page();
            string query = "Select * from Pages where Id='" + Id + "'";
            Database_Helpers db = new Database_Helpers();
            try
            {
                db.Connection.Open();
                SqlCommand cmd = new SqlCommand(query, db.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        page = new Page();
                        page.Id = (int)reader["Id"];
                        page.BookId = (int)reader["BookId"];
                        page.BaabId = (int)reader["BaabId"];
                        page.PageDetails = reader["PageDetails"].ToString();
                        page.PageNumberDisplay = reader["PageNumberDisplay"].ToString();
                        page.PageTag = reader["PageTag"].ToString();
                    }
                }
                return View(page);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(page);
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Delete(Page page)
        {
            Database_Helpers db = new Database_Helpers();
            if (db.delete("Pages", "where Id='" + page.Id + "'"))
            {
                return RedirectToAction("Index", "Pages");
            }
            else
            {
                return View();
            }
        }
    }
}