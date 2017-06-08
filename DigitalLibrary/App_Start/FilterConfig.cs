using DigitalLibrary.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace DigitalLibrary
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            AddSeeds();
        }

        public static void AddSeeds()
        {
            Database_Helpers db = new Database_Helpers();
            if (db.get_scalar("select Count(*) from users WHERE Name='Admin'") == 0)
            {
                Users _user = new Users();
                _user.Name = "Admin";
                _user.Password = "Admin1234%";
                _user.Role = "admin";
                _user.UserPhone = "03344037449";
                _user.Location = "Jamia Ashrafia Lahore, Pakistan";
                db.Insert("Users", _user);
            }


        }
    }
}
