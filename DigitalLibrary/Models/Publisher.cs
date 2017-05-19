using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalLibrary.Models
{
    public class Publisher
    {
        [DontInsert]
        [DontUpdate]
        public int Id { get; set; }
        public int Name { get; set; }
        public int Address { get; set; }
    }
}