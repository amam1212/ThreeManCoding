using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models.EF;
using WebApplication3.Models.Entities;

namespace WebApplication3.Models.Repository
{
    public partial class ShoppingCartRepository : IShoppingCartRepository
    {
        EfDbContext _context = new EfDbContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public ShoppingCartRepository GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCartRepository();
            cart.ShoppingCartId = cart.GetCartId(context);
            
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public ShoppingCartRepository GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public int AddToCart(Product item)
        {
            // Get the matching cart and item instances
            var cartItem = _context.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.productId.Equals(item.ID));

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    productId = item.ID,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                _context.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            _context.SaveChanges();

            return cartItem.Count;
        }

        public int AddToCart(Product item, int amount)
        {
            // Get the matching cart and item instances
            var cartItem = _context.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.productId.Equals(item.ID));

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    productId = item.ID,
                    CartId = ShoppingCartId,
                    Count = amount,
                    DateCreated = DateTime.Now
                };
                _context.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity

                cartItem.Count += amount;
            }
            // Save changes
            _context.SaveChanges();

            return cartItem.Count;
        }

        public int RemoveFromCart(string id)
        {


            // Get the cart

            var cartItem = _context.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.productId.Equals(id));


            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count = 0;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _context.Carts.Remove(cartItem);
                }
                // Save changes
                _context.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = _context.Carts.Where(
                cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _context.Carts.Remove(cartItem);
            }
            // Save changes
            _context.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return _context.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in _context.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal(string role)
        {
            // Multiply item price by count of that item to get 
            // the current price for each of those items in the cart
            // sum all item price totals to get the cart total
            //decimal? total = null;
            decimal? total = null;
            if (role.Equals("Retail")) { 
              total = (from cartItems in _context.Carts
                     where cartItems.CartId == ShoppingCartId
                     select (int?)cartItems.Count *
                     (int?)cartItems.product.Retail_Price).Sum();

            return total ?? decimal.Zero;

            }
            else if (role.Equals("Wholesale"))
            {
                total = (from cartItems in _context.Carts
                                  where cartItems.CartId == ShoppingCartId
                                  select (int?)cartItems.Count *
                                  (int?)cartItems.product.Wholesale_Price).Sum();

                return total ?? decimal.Zero;

            }
            return total ?? decimal.Zero;
        }

        public Order CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            order.OrderDetails = new List<OrderDetail>();

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            
            foreach (var item in cartItems)
            {
           
        
                var orderDetail = new OrderDetail
                {
                    ItemId = int.Parse(item.productId),
                    OrderId = order.OrderId,
                    UnitPrice = (decimal)(item.product.Retail_Price),
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (decimal)(item.Count * item.product.Retail_Price);
                order.OrderDetails.Add(orderDetail);
                _context.OrderDetails.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            _context.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order;
        }


        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }




        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = _context.Carts.Where(
                c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }
            _context.SaveChanges();
        }



    }
}