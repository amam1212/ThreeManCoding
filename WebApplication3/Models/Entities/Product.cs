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
        public string Images { get; set; }
        public string Description { get; set; }
        public double Retail_Price { get; set; }
        public double Wholesale_Price { get; set; }

    }
}