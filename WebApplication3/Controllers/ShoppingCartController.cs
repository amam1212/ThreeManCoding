using WebApplication3.Models;
using WebApplication3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models.EF;
using WebApplication3.Models.Repository;

namespace ShoppingCartController.Controllers
{
    public class ShoppingCartController : Controller
    {

        ShoppingCartRepository ShoppingCart = new ShoppingCartRepository();
        EfDbContext storeDB = new EfDbContext();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            string role = null;
            if (User.IsInRole("Retail"))
            {
                role = "Retail";
            }
            else
            {
                role = "Wholesale";
            }

            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {

                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(role)
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        [HttpPost]
        public ActionResult AddToCart(string id,int amount)
        {
            // Retrieve the item from the database
            var addedItem = storeDB.Products
                .Single(Product => Product.ID.Equals(id));

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            int count = amount;
            int sum = cart.GetCount() + amount;
            cart.AddToCart(addedItem, amount);
            // Display the confirmation message
            string role = null;
            if (User.IsInRole("Retail"))
            {
                role = "Retail";
            }
            else{
                role = "Wholesale";
            }
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(addedItem.Name) +
                    " has been added to your shopping cart.",

                CartTotal = cart.GetTotal(role),
                CartCount = sum,
                ItemCount = sum,
                DeleteId = int.Parse(id)
            };
            return Json(results);

            // Go back to the main store page for more shopping
            // return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(string id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the item to display confirmation

            // Get the name of the album to display confirmation
            string itemName = storeDB.Products
                .Single(item => item.ID == id).Name;


            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            string role = null;
            if (User.IsInRole("Retail"))
            {
                role = "Retail";
            }
            else
            {
                role = "Wholesale";
            }
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = "One (1) " + Server.HtmlEncode(itemName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(role),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = int.Parse(id)
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}