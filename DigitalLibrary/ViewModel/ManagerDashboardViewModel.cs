using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalLibrary.ViewModel
{
    public class ManagerDashboardViewModel
    {
        public int BookAssignCount { get; set; }
        public int NoOfBooksCompleted { get; set; }
        public int UsersCount { get; set; }

        public ManagerDashboardViewModel()
        {
            BookAssignCount = Book.AssignBookCount();
            UsersCount = Users.UsersAccountCount();
        }
    }
}