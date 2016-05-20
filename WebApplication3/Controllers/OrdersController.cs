using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using WebApplication3.Models;
using WebApplication3.Models.EF;
using Microsoft.AspNet.Identity;

namespace OpenOrderFramework.Controllers
{
    public class OrdersController : Controller
    {
        private EfDbContext db = new EfDbContext();

        // GET: Orders
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            string currentUsername = User.Identity.Name;
            var orders = from o in db.Orders
                         where o.Username == currentUsername
                         select o;

            if (User.IsInRole("Administrator"))
            {
                 orders = from o in db.Orders
                             select o;

            }


                if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.FirstName.ToUpper().Contains(searchString.ToUpper())
                                       || s.LastName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.FirstName);
                    break;
                case "Price":
                    orders = orders.OrderBy(s => s.Total);
                    break;
                case "price_desc":
                    orders = orders.OrderByDescending(s => s.Total);
                    break;
                default:  // Name ascending 
                    orders = orders.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(orders.ToPagedList(pageNumber, pageSize));

            //return View(await db.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            var orderDetails = db.OrderDetails.Where(x => x.OrderId == id);

            order.OrderDetails = await orderDetails.ToListAsync();
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Order order, HttpPostedFileBase file,int OrderId)
        {

 

            Order orderselect = await db.Orders.FindAsync(OrderId);
   
            string pathimage = null;
            if (file != null)
            {
                if (System.IO.File.Exists(Server.MapPath(orderselect.TransferSlip)))
                {

                    System.IO.File.Delete(Server.MapPath(orderselect.TransferSlip));
                }

                //string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/Content/Images/TransferSlip/"));
                // file is uploaded
                string extension = System.IO.Path.GetExtension(file.FileName);
                string fileName = file.FileName;

                file.SaveAs(System.IO.Path.Combine(path, fileName));


                //file.SaveAs(path);
                pathimage = "~/Content/Images/TransferSlip/" + fileName;

             
            }

            if (file == null)
            {

                pathimage = orderselect.TransferSlip;
            }
            orderselect.FirstName = order.FirstName;
            orderselect.LastName = order.LastName;
            orderselect.Address = order.Address;
            orderselect.City = order.City;
            orderselect.State = order.State;
            orderselect.PostalCode = order.PostalCode;
            orderselect.Country = order.Country;
            orderselect.Phone = order.Phone;
            orderselect.Status = order.Status;
            orderselect.TransferSlip = pathimage;
            




            if (ModelState.IsValid)
                {
                    db.Entry(orderselect).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(order);

           
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
