using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models.Entities;

namespace WebApplication3.Models.Repository
{
    public partial interface IShoppingCartRepository
    {
         ShoppingCartRepository GetCart(HttpContextBase context);
        int AddToCart(Product item);
        int AddToCart(Product item, int amount);
        int RemoveFromCart(string id);
         void EmptyCart();
         List<Cart> GetCartItems();
         int GetCount();
         decimal GetTotal(string role);
         string GetCartId(HttpContextBase context);
         void MigrateCart(string userName);

    }
}
