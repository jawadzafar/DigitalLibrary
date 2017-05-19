using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalLibrary.Models
{
    public class BookMark
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}