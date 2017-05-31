using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalLibrary.ViewModel
{
    public class AdminDashboardViewModel
    {
        public int BookCount { get; set; }
        public int BooksInProgress { get; set; }
        public int UsersCount { get; set; }
        public int ManagersCount { get; set; }

        public AdminDashboardViewModel()
        {
            BookCount = Book.Count();
            UsersCount = Users.UsersAccountCount();
            ManagersCount = Users.ManagersCount(); 
        }
    }
}