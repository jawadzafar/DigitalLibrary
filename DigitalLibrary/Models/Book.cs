using System;
using System.Collections.Generic;
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
        public int EditionNumber { get; set; }
        public int BookCompleted { get; set; }
        public int NoOfPages { get; set; }
        public int NoOfAbwaabs { get; set; }
    }
}