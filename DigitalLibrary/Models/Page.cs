using System;
using System.Collections.Generic;
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
    }
}