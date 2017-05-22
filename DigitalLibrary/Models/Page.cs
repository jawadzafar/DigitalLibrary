using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DigitalLibrary.Models
{
    public class Page
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public int BaabId { get; set; }
        public string PageDetails { get; set; }
        public string PageNumberDisplay { get; set; }
        public string PageTag { get; set; }
        public int BookId { get; set; }
        internal static List<Page> GetAll()
        {
            Database_Helpers db = new Database_Helpers();
            Page page = new Page();
            List<Page> ListOfPages = new List<Page>();
            string query = "select * from pages";
            try
            {
                db.Connection.Open();
                SqlCommand cmd = new SqlCommand(query, db.Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        page = new Page();
                        page.Id = (int)reader["Id"];
                        page.BaabId = (int)reader["BaabId"];
                        page.BookId = (int)reader["BookId"];
                        page.PageTag = reader["PageTag"].ToString();
                        page.PageDetails = reader["PageDetails"].ToString();
                        page.PageNumberDisplay = reader["PageNumberDisplay"].ToString();
                        ListOfPages.Add(page);
                    }
                }
                return ListOfPages;
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return ListOfPages;
                throw ex;
            }

        }
        internal static bool Save(Page page)
        {
            Database_Helpers db = new Database_Helpers();
            if (db.Insert("Pages", page))
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