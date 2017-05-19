using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace DigitalLibrary.Models
{
    public class User
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string UserPhone { get; set; }
        public string Location { get; set; }

        public static bool Insert(User user)
        {
            Database_Helpers db = new Database_Helpers();
            if (db.Insert("Users", user))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    
}