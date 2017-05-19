using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary.Controllers
{
    public class BookController : Controller
    {
        [HttpGet]
        // GET: Book
        public ActionResult Index()
        {
            
            return View(Book.GetAll());
        }
        [HttpGet]
        public ActionResult Add()
       {
            
            return View(new Book());
        }
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
                    return RedirectToAction("Index", "Book");
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
    }
}