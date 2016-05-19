using WebApplication3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication3.Models.Entities;

namespace WebApplication3.ViewModel
{
    public class ShoppingCartViewModel
    {
        [Key]
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}