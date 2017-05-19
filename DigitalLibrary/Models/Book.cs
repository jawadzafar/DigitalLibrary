using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DigitalLibrary.Models
{
    public class Book
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public string Name { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public DateTime PublishingYear { get; set; }

        internal static List<Book> GetAll()
        {
            Book books = new Book();
            List<Book> BooksList = new List<Book>();
            string query = "select * from Books";
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
                        books = new Book();
                        books.Id = (int)reader["Id"];
                        books.Name = reader["Name"].ToString();
                        books.AuthorId = (int)reader["AuthorId"];
                        books.PublisherId = (int)reader["PublisherId"];
                        books.PublishingYear = Convert.ToDateTime(reader["PublishingYear"].ToString());
                        books.BookCompleted = Convert.ToByte(reader["BookCompleted"]);
                        books.NoOfPages = Convert.ToInt16(reader["NoOfPages"]);
                        books.NoOfAbwaabs= Convert.ToInt16(reader["NoOfAbwaabs"]);
                        books.BookCover = reader["BookCover"].ToString();
                        BooksList.Add(books);
                    }
                }
                return BooksList;
            }
            catch (Exception ex)
            {
                db.Connection.Close();
                return BooksList;
            }
        }

        public int EditionNumber { get; set; }
        public int BookCompleted { get; set; }
        public int NoOfPages { get; set; }
        public int NoOfAbwaabs { get; set; }
        public string BookCover { get; set; }
        
        internal static bool Save(Book book)
        {
            Database_Helpers db = new Database_Helpers();
            if(db.Insert("Books",book ))
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