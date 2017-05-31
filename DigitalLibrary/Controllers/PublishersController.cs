using DigitalLibrary.Attributes;
using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary.Controllers
{
    public class PublishersController : Controller
    {
        
        [HttpGet]
        
        // GET: Publishers
        [Role(new string[] { "admin", "manager", "user" })]
        public ActionResult Index()
        {
            return View(Publisher.GetAll());
        }
        [HttpGet]
        [Role(new string[] { "admin", "manager" })]
        public ActionResult Add()
        {
            return View(new Publisher());
        }
        [HttpPost]
        [Role(new string[] { "admin", "manager" })]
        public ActionResult Add(Publisher publisher)
        {
            if (Publisher.Save(publisher))
            {
                return RedirectToAction("Index","Publishers");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        [Role(new string[] { "admin", "manager"})]
        public ActionResult Edit(int Id)
        {
            Publisher publisher = new Publisher();
            string query = "Select * from Publishers where Id='"+Id+"'";
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
                        publisher = new Publisher();
                        publisher.Id = (int)reader["Id"];
                        publisher.Name = reader["Name"].ToString();
                        publisher.Address = reader["Address"].ToString();
                    }
                }
                return View(publisher);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(publisher);
                throw ex;
            }
               
        }

        [HttpPost]
        [Role(new string[] { "admin", "manager" })]
        public ActionResult Edit(Publisher publisher)
        {
            Database_Helpers db = new Database_Helpers();
            if(db.Update("Publishers",publisher, "where Id='" + publisher.Id + "'"))
            {
                return RedirectToAction("Index","Publishers");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Role("admin")]
        public ActionResult Delete(int Id)
        {
            Publisher publisher = new Publisher();
            string query = "Select * from Publishers where Id='" + Id + "'";
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
                        publisher = new Publisher();
                        publisher.Id = (int)reader["Id"];
                        publisher.Name = reader["Name"].ToString();
                        publisher.Address = reader["Address"].ToString();
                    }
                }
                return View(publisher);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(publisher);
                throw ex;
            }

        }

        [HttpPost]
        [Role("admin")]
        public ActionResult Delete(Publisher publisher)
        {
            Database_Helpers db = new Database_Helpers();
            if (db.delete("Publishers", "where Id='" + publisher.Id + "'"))
            {
                return RedirectToAction("Index", "Publishers");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Role(new string[] { "admin", "manager"})]
        public ActionResult Details(int Id)
        {
            Publisher publisher = new Publisher();
            string query = "Select * from Publishers where Id='" + Id + "'";
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
                        publisher = new Publisher();
                        publisher.Id = (int)reader["Id"];
                        publisher.Name = reader["Name"].ToString();
                        publisher.Address = reader["Address"].ToString();
                    }
                }
                return View(publisher);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(publisher);
                throw ex;
            }

        }
    }
}