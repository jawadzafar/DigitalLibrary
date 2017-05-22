using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary.Controllers
{
    public class AuthorsController : Controller
    {
        [HttpGet]
        // GET: Authors
        public ActionResult Index()
        {
            return View(Authors.GetAll() );
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View(new Authors());
        }
        [HttpPost]
        public ActionResult Add(Authors author)
        {
            if (Authors.Save(author))
            {
                return RedirectToAction("index", "Authors");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Authors author = new Authors();
            string query = "Select * from Authors where id='" + id + "'";
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
                        author = new Authors();
                        author.Id = (int)reader["Id"];
                        author.Name = reader["Name"].ToString();
                        author.Desigination= reader["Desigination"].ToString();
                        author.UserId= (int)reader["UserId"];
                    }
                }
                return View(author);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(author);
                throw ex;
            }


        }
        [HttpPost]
        public ActionResult Edit(Authors author)
        {
            Database_Helpers db = new Database_Helpers();
            
                if (db.Update("Authors", author, "where Id='" + author.Id + "'"))
                {
                    return RedirectToAction("Index", "Authors");
                }
                else
                {
                    return View();
                }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Authors author = new Authors();
            string query = "Select * from Authors where id='" + id + "'";
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
                        author = new Authors();
                        author.Id = (int)reader["Id"];
                        author.Name = reader["Name"].ToString();
                        author.Desigination = reader["Desigination"].ToString();
                        author.UserId = (int)reader["UserId"];
                    }
                }
                return View(author);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(author);
                throw ex;
            }


        }
        [HttpPost]
        public ActionResult Delete(Authors author)
        {
            Database_Helpers db = new Database_Helpers();

            if (db.delete("Authors", "where Id='" + author.Id + "'"))
            {
                return RedirectToAction("Index", "Authors");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Authors author = new Authors();
            string query = "Select * from Authors where id='" + id + "'";
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
                        author = new Authors();
                        author.Id = (int)reader["Id"];
                        author.Name = reader["Name"].ToString();
                        author.Desigination = reader["Desigination"].ToString();
                        author.UserId = (int)reader["UserId"];
                    }
                }
                return View(author);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(author);
                throw ex;
            }


        }

    }


}