using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DigitalLibrary.Models
{
    public class Abwaab
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public string Name { get; set; }
        public int BaabNo { get; set; }
        public int NoOfPages { get; set; }    
        public int BookId { get; set; }

        internal static List<Abwaab> GetAll()
        {
            Abwaab abwaab = new Abwaab();
            List<Abwaab> ListOfAbwaab = new List<Abwaab>();
            string query = "Select * from Abwaabs";
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
                        abwaab = new Abwaab();
                        abwaab.Id = (int)reader["Id"];
                        abwaab.Name = reader["Name"].ToString();
                        abwaab.BookId = (int)reader["BookId"];
                        abwaab.BaabNo = (int)reader["BaabNo"];
                        abwaab.NoOfPages = (int)reader["NoOfPages"];
                        ListOfAbwaab.Add(abwaab);
                    }
                }
                return ListOfAbwaab;
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return ListOfAbwaab;
                throw ex;
            }

        }
      
        internal static bool Save(Abwaab abwaab)
        {
            Database_Helpers db = new Database_Helpers();
            if (db.Insert("Abwaabs", abwaab))
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