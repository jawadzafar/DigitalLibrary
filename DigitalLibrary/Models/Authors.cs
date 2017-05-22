using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DigitalLibrary.Models
{
    public class Authors
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desigination { get; set; }
        public int UserId { get; set; }

        internal static List<Authors> GetAll()
        {
            Authors author = new Authors();
            List<Authors> AuthorList = new List<Authors>();
            string query = "Select * from Authors";
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
                        author = new Authors();
                        author.Id = (int)reader["Id"];
                        author.Name = reader["Name"].ToString();
                        author.UserId = (int)reader["UserId"];
                        author.Desigination = reader["Desigination"].ToString();
                        AuthorList.Add(author);
                    }
                }
                return AuthorList;
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return AuthorList;
                throw ex;
            }
            
        }

        internal static bool Save(Authors author)
        {
            Database_Helpers db = new Database_Helpers();
            if(db.Insert("Authors", author))
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