using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace DigitalLibrary.Models
{
    public class Users
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string UserPhone { get; set; }
        public string Location { get; set; }

        internal static bool Save(Users user)
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

        
        internal static Users Exist(Users user)
        {
            Database_Helpers db = new Database_Helpers();
            Users _user = null;
            string query = "select * from Users where Name='" + user.Name + "' and password='" + user.Password+ "'";
            try
            {
                db.Connection.Open();
                SqlCommand cmd = new SqlCommand(query, db.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _user = new Users()
                        {
                            Id = Convert.ToInt16( reader["Id"].ToString()),
                            Name = reader["Name"].ToString(),
                            Password= reader["Password"].ToString(),
                            Role = reader["Role"].ToString()
                        };

                    }
                }
                return _user;
            }
            catch (Exception ex)
            {

                db.Connection.Close();
            }
            finally
            {
                db.Connection.Close();
            }

            return _user;
        }
    }
    
}