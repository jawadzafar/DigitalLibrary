using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary.Controllers
{
    public class AbwaabsController : Controller
    {
        [HttpGet]
        // GET: Abwaabs
        public ActionResult Index()
        {
            return View(  Abwaab.GetAll() );
        }
        [HttpGet]
        public  ActionResult Add()
        {
            return View(new Abwaab());
        }
        [HttpPost]
        public ActionResult Add(Abwaab abwaab)
        {
            if (Abwaab.Save(abwaab))
            {
                return RedirectToAction("Index", "Abwaabs");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Abwaab abwaab = new Abwaab();
            string query = "Select * from Abwaabs where id='" + id + "'";
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
                        abwaab = new Abwaab();
                        abwaab.Id = (int)reader["Id"];
                        abwaab.Name = reader["Name"].ToString();
                        abwaab.BaabNo= (int)reader["BaabNo"];
                        abwaab.NoOfPages = (int)reader["NoOfPages"];
                        abwaab.BookId = (int)reader["BookId"];
                    }
                }
                return View(abwaab);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(abwaab);
            }
        }

        [HttpPost]
        public ActionResult Edit(Abwaab abwaab)
        {
            Database_Helpers db = new Database_Helpers();

            if (db.Update("Abwaabs", abwaab, "where Id='" + abwaab.Id + "'"))
            {
                return RedirectToAction("Index", "Abwaabs");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            Abwaab abwaab = new Abwaab();
            string query = "Select * from Abwaabs where id='" + id + "'";
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
                        abwaab = new Abwaab();
                        abwaab.Id = (int)reader["Id"];
                        abwaab.Name = reader["Name"].ToString();
                        abwaab.BaabNo = (int)reader["BaabNo"];
                        abwaab.NoOfPages = (int)reader["NoOfPages"];
                        abwaab.BookId = (int)reader["BookId"];
                    }
                }
                return View(abwaab);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(abwaab);
            }
        }

        [HttpPost]
        public ActionResult Delte(Abwaab abwaab)
        {
            Database_Helpers db = new Database_Helpers();

            if (db.delete("Abwaabs", "where Id='" + abwaab.Id + "'"))
            {
                return RedirectToAction("Index", "Abwaabs");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Abwaab abwaab = new Abwaab();
            string query = "Select * from Abwaabs where id='" + id + "'";
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
                        abwaab = new Abwaab();
                        abwaab.Id = (int)reader["Id"];
                        abwaab.Name = reader["Name"].ToString();
                        abwaab.BaabNo = (int)reader["BaabNo"];
                        abwaab.NoOfPages = (int)reader["NoOfPages"];
                        abwaab.BookId = (int)reader["BookId"];
                    }
                }
                return View(abwaab);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(abwaab);
            }
        }

    }
}