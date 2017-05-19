using System;
using System.Collections.Generic;
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
    }
}