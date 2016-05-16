using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Product
    {
        
        public string ID { get; set; }

        public string Name { get; set; }
        public string images { get; set; }
        public double Price { get; set; }

    }
}