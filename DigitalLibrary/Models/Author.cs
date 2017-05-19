using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalLibrary.Models
{
    public class Author
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desigination { get; set; }
        public int UserId { get; set; }
    }
}