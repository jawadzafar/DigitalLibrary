using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalLibrary.ViewModel
{
    public class UserDashboardViewModel
    {
        public int BookCount { get; set; }
        public UserDashboardViewModel()
        {
            BookCount = Book.Count();
        }
    }
}