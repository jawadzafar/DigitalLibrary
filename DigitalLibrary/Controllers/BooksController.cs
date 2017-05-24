using DigitalLibrary.Attributes;
using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary.Controllers
{
    public class BooksController : Controller
    {
        [HttpGet]
        [Role(new string[] { "Admin", "Manager","User" })]
        // GET: Book
        public ActionResult Index()
        {
            
            return View(Book.GetAll());
        }
        [HttpGet]
        [Role("Admin")]
        public ActionResult Add()
       {
            
            return View(new Book());
        }
        [Role("Admin")]
        [HttpPost]
        public ActionResult Add(Book book, HttpPostedFileBase file)
        {
            if (file != null)
            {
                var allowedExtensions = new[] { ".jpeg", ".jpg", ".JPG", ".png" };
                var filename = file.FileName.ToString();
                var ext = Path.GetExtension(file.FileName);

                if (allowedExtensions.Contains(ext))
                {
                    var name = Path.GetFileNameWithoutExtension(filename);
                    var myfile = name + ext;
                    var path = Path.Combine(Server.MapPath(@"~\Asserts\Book_Images"), myfile);
                    book.BookCover = myfile;
                    file.SaveAs(path);
                }
                if (Book.Save(book))
                {
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [Role(new string[] { "Admin", "Manager" })]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Book books = new Book();
            string query = "Select * from books where id='" + id + "'";
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
                        books = new Book();
                        books.Id = (int)reader["Id"];
                        books.Name = reader["Name"].ToString();
                        books.AuthorId = (int)reader["AuthorId"];
                        books.PublisherId = (int)reader["PublisherId"];
                        books.EditionNumber = (int)reader["EditionNumber"];
                        books.PublishingYear = Convert.ToDateTime(reader["PublishingYear"].ToString());
                        books.BookCompleted = Convert.ToByte(reader["BookCompleted"]);
                        books.NoOfPages = Convert.ToInt16(reader["NoOfPages"]);
                        books.NoOfAbwaabs = Convert.ToInt16(reader["NoOfAbwaabs"]);
                        books.BookCover = reader["BookCover"].ToString();
                    }
                }
                return View(books);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(books);
            }

         
        }
        [Role(new string[] { "Admin", "Manager" })]
        [HttpPost]
        public ActionResult Edit(Book book, HttpPostedFileBase file)
        {
            Database_Helpers db = new Database_Helpers();
            if (file != null)
            {
                var allowedExtensions = new[] { ".jpeg", ".jpg", ".JPG", ".png" };
                var filename = file.FileName.ToString();
                var ext = Path.GetExtension(file.FileName);

                if (allowedExtensions.Contains(ext))
                {
                    var name = Path.GetFileNameWithoutExtension(filename);
                    var myfile = name + ext;
                    var path = Path.Combine(Server.MapPath(@"~\Asserts\Book_Images"), myfile);
                    book.BookCover = myfile;
                    file.SaveAs(path);
                }

                if (db.Update("Books", book, "where Id='" + book.Id + "'"))
                {
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }


        [Role("Admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Book books = new Book();
            string query = "Select * from books where id='" + id + "'";
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
                        books = new Book();
                        books.Id = (int)reader["Id"];
                        books.Name = reader["Name"].ToString();
                        books.AuthorId = (int)reader["AuthorId"];
                        books.PublisherId = (int)reader["PublisherId"];
                        books.EditionNumber = (int)reader["EditionNumber"];
                        books.PublishingYear = Convert.ToDateTime(reader["PublishingYear"].ToString());
                        books.BookCompleted = Convert.ToByte(reader["BookCompleted"]);
                        books.NoOfPages = Convert.ToInt16(reader["NoOfPages"]);
                        books.NoOfAbwaabs = Convert.ToInt16(reader["NoOfAbwaabs"]);
                        books.BookCover = reader["BookCover"].ToString();
                    }
                }
                return View(books);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(books);
            }


        }
        [Role("Admin")]
        [HttpPost]
        public ActionResult Delete(Book book)
        {
            Database_Helpers db = new Database_Helpers();
                if (db.delete("Books",  "where Id='" + book.Id + "'"))
                {
                    return RedirectToAction("Index", "Books");
                }
                else
                {
                    return View();
                }
            
        }
        [HttpGet]
        [Role(new string[] { "Admin", "Manager" })]
        public ActionResult Details(int id)
        {
            Book books = new Book();
            string query = "Select * from books where id='" + id + "'";
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
                        books = new Book();
                        books.Id = (int)reader["Id"];
                        books.Name = reader["Name"].ToString();
                        books.AuthorId = (int)reader["AuthorId"];
                        books.PublisherId = (int)reader["PublisherId"];
                        books.EditionNumber = (int)reader["EditionNumber"];
                        books.PublishingYear = Convert.ToDateTime(reader["PublishingYear"].ToString());
                        books.BookCompleted = Convert.ToByte(reader["BookCompleted"]);
                        books.NoOfPages = Convert.ToInt16(reader["NoOfPages"]);
                        books.NoOfAbwaabs = Convert.ToInt16(reader["NoOfAbwaabs"]);
                        books.BookCover = reader["BookCover"].ToString();
                    }
                }
                return View(books);
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return View(books);
            }


        }

    }
}