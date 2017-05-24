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

        internal static List<Users> GetAll()
        {
            Users user = new Users();
            List<Users> UserList = new List<Users>();

            string query = "Select * from Users";

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
                        UserList.Add(user);
                    }
                }
                return UserList;
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return UserList;
                throw ex;
            }
        }
        internal static List<Users> GetAll(string role)
        {
            Users user = new Users();
            List<Users> UserList = new List<Users>();
            
                string query = "Select * from Users where role='"+role+"'";

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
                            UserList.Add(user);
                        }
                    }
                    return UserList;
                }
                catch (Exception ex)
                {
                    db.Connection.Close();
                    return UserList;
                    throw ex;
                }
        }
    }
    
}