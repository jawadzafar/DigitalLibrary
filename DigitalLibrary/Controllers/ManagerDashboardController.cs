using DigitalLibrary.Attributes;
using DigitalLibrary.Models;
using DigitalLibrary.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary.Controllers
{
    public class ManagerDashboardController : Controller
    {
        [HttpGet]
        [Role ("manager")]
        // GET: ManagerDashboard
        public ActionResult Index()
        {
            return View(new ManagerDashboardViewModel());
        }
        [HttpGet]
        [Role("manager")]
        public ActionResult Add()
        {
            return View(new Users());
        }
        [HttpPost]
        [Role("manager")]
        public ActionResult Add(Users user)
        {
            if (Users.Save(user))
            {
                return RedirectToAction("ViewList", "ManagerDashboard");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        [Role("manager")]
        public ActionResult ViewList()
        {
            return View(Users.GetAll());
        }
        [HttpPost]
        [Role("manager")]
        public ActionResult ViewList(FormCollection form)
        {
            string role = form["Role"].ToString();
            return View(Users.GetAll(role));

        }

        [HttpGet]
        [Role("manager")]
        public ActionResult Edit(int Id)
        {
            Users user = new Users();
            string query = "Select * from Users where Id='" + Id + "'";
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
                        user = new Users();
                        user.Id = (int)reader["Id"];
                        user.Name = reader["Name"].ToString();
                        user.EmailAddress = reader["EmailAddress"].ToString();
                        user.Location = reader["Location"].ToString();
                        user.Role = reader["Role"].ToString();
                        user.UserPhone = reader["UserPhone"].ToString();
                    }
                }
                return View(user);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(user);
                throw ex;
            }

        }
        [HttpPost]
        [Role("manager")]
        public ActionResult Edit(Users user)
        {
            Database_Helpers db = new Database_Helpers();

            if (db.Update("Users", user, "where Id='" + user.Id + "'"))
            {
                return RedirectToAction("ViewList", "ManagerDashboard");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        [Role("manager")]
        public ActionResult Delete(int Id)
        {
            Users user = new Users();
            string query = "Select * from Users where Id='" + Id + "'";
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
                        user = new Users();
                        user.Id = (int)reader["Id"];
                        user.Name = reader["Name"].ToString();
                        user.EmailAddress = reader["EmailAddress"].ToString();
                        user.Location = reader["Location"].ToString();
                        user.Role = reader["Role"].ToString();
                        user.UserPhone = reader["UserPhone"].ToString();
                    }
                }
                return View(user);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(user);
                throw ex;
            }
        }
        [HttpPost]
        [Role("manager")]
        public ActionResult Delete(Users user)
        {
            Database_Helpers db = new Database_Helpers();

            if (db.delete("Users", "where Id='" + user.Id + "'"))
            {
                return RedirectToAction("ViewList", "ManagerDashboard");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Role("manager")]
        public ActionResult AssignBook()
        {
            return View(new BookAsignmentViewModel());
        }
        [HttpPost]
        [Role("manager")]
        public ActionResult AssignBook(string UserId, string BookId)
        {

            BookAsignmentViewModel.Save(BookId, UserId);
            return View();

        }
        [HttpGet]
        [Role("manager")]
        public ActionResult ViewAssignBook()
        {
            var list = Database_Helpers.QueryList("select Books.Name as BookName, Books.NoOfAbwaabs, Books.EditionNumber, Users.Name as ManagerName from books join Users on Users.Id = Books.UserId ");
            return View(list);
        }

    }
}