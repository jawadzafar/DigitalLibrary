using DigitalLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalLibrary.ViewModel
{
    public class BookAsignmentViewModel
    {
        [DontInsert]
        [DontUpdate]
        public List<Book> BookList { get; set; }
        public List<Book> BookList1 { get; set; }
        [DontInsert]
        [DontUpdate]
        public List<Users> ManagersList { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; } 
        public bool Status { get; set; }
        public bool aassign { get; set; }
        public BookAsignmentViewModel()
        {

            ManagersList = Users.GetAllManagers();
            BookList = Book.GetAllUnAssignedBook();
           
        }

        internal bool Save()
        {
            Database_Helpers db = new Database_Helpers();

            if (db.Insert("AssignedBook", this))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static bool Save(string BookId, string UserId)
        {
            Database_Helpers db = new Database_Helpers(); 

            db.values.Add("BookId", BookId);
            db.values.Add("UserId", UserId);
            db.values.Add("Status",false.ToString());
            //db.values.Add("assign", true.ToString());
            if (db.insert("AssignedBook", db.values))
            {
                db.values.Clear();
                db.values.Add("assign", true.ToString());
                db.values.Add("UserId",UserId);
                db.insert("Book",db.values);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}