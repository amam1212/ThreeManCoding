using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using WebApplication3.Models.EF;
using WebApplication3.Models.Repository;

namespace WebApplication3.Controllers
{
    public class ProductController : Controller
    {

        ProductRepository repository = new ProductRepository();
        // GET: Product

        public ActionResult Index()
        {
            //var isRetail = User.IsInRole("Retail");
            if (User.IsInRole("Retail"))
            {
                return RedirectToAction("Retail");
            }
            if (User.IsInRole("Wholesale"))
            {
                return RedirectToAction("Wholesale");
            }
            return View(repository.GetAllProduct());
        }

        // GET: Product
        [Authorize(Roles = "Retail")]
        public ActionResult Retail()
        {
            return View(repository.GetAllProduct());
        }

        [Authorize(Roles = "Wholesale")]
        public ActionResult Wholesale()
        {
            return View(repository.GetAllProduct());
        }



        public ActionResult Manage()
        {
            return View(repository.GetAllProduct());
        }


        // GET: Product/Details/5
        public ActionResult Details(string id)
        {
            return View(repository.GetProductByID(id));
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                string pathimage = null;
                Product product = new Product();
                EfDbContext _context = new EfDbContext();

                int count=0;
                foreach (Product pro in _context.Products)
                {
                    count = int.Parse(pro.ID);
                    
                }
                count++;

                string id = count.ToString();
                if (file != null)
                {
                    //string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/Product/"));
                    // file is uploaded
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string fileName = id;
                    file.SaveAs(System.IO.Path.Combine(path, fileName + extension));

                    //file.SaveAs(path);
                    pathimage = "~/Content/Images/Product/" + fileName + extension;
                }

                
                var name = collection["Name"];
                var retailprice = collection["Retail_Price"];
                var wholesaleprice = collection["Wholesale_Price"];
                double ConvertNum = double.Parse(retailprice);
                
                product.ID = id;
                product.Name = name;
                product.Images = pathimage;
                product.Retail_Price = double.Parse(retailprice);
                product.Wholesale_Price = double.Parse(wholesaleprice);

                repository.AddProduct(product);

                return RedirectToAction("Manage");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/Product/"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image 
                // directly to database
                // in-case if you want to store byte[] ie. for DB


            }
            // after successfully uploading redirect the user
            return View();
        }







        // GET: Product/Edit/5
        public ActionResult Edit(string id)
        {
            Product product = repository.GetProductByID(id);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection, HttpPostedFileBase file)
        {
            try
            {

                Product p = repository.GetProductByID(id);
                if (System.IO.File.Exists(Server.MapPath(p.Images)))
                {

                    System.IO.File.Delete(Server.MapPath(p.Images));
                }


                string pathimage = null;
                if (file != null)
                {
                    //string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/Product"));
                    // file is uploaded
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string fileName = id;

                    file.SaveAs(System.IO.Path.Combine(path, fileName + extension));


                    //file.SaveAs(path);
                    pathimage = "~/Content/Images/Product/" + fileName + extension;
                }
                
                if (file == null)
                {
                 
                    pathimage = p.Images;
                }

                var name = collection["Name"];
                var retailprice = collection["Retail_Price"];
                var wholesaleprice = collection["Wholesale_Price"];

                p.ID = id;
                p.Name = name;
                p.Retail_Price = double.Parse(retailprice);
                p.Wholesale_Price = double.Parse(wholesaleprice);
                p.Images = pathimage;
                repository.EditProduct(p);
                
                return RedirectToAction("Manage");
            }
            catch
            {
                return RedirectToAction("Manage");
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(string id)
        {
            Product product = repository.GetProductByID(id);
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                Product product = repository.GetProductByID(id);
                if (System.IO.File.Exists(Server.MapPath(product.Images)))
                {

                    System.IO.File.Delete(Server.MapPath(product.Images));
                }

                repository.DeleteProduct(id);
                return RedirectToAction("Manage");
            }
            catch
            {
                return View();
            }
        }
    }
}
