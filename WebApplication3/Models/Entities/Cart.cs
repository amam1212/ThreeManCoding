using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.Entities
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        public string CartId { get; set; }
        public string productId { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual Product product { get; set; }
    }
}