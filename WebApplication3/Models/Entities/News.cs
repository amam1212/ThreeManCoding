using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models.Entities
{
    public class News
    {
        public string NewsID { get; set; }
        public string NewsTitle { get; set; }
        public string NewsImages { get; set; }
        public string NewsDescription { get; set; }
    }
}