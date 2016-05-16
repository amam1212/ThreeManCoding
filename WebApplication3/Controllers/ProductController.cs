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
            return View(repository.GetAllProduct());
        }


        [HttpPost]
        public ActionResult Index(Product p)
        {
            repository.AddProduct(p);
            //Test commit
            return RedirectToAction("Index");
        }




        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                int count = _context.Products.Count();
                count++;
                string id = count.ToString();
                if (file != null)
                {
                    //string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/images"));
                    // file is uploaded
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string fileName = id;
                    file.SaveAs(System.IO.Path.Combine(path, fileName + extension));

                    //file.SaveAs(path);
                    pathimage = "~/images/" + fileName + extension;
                }

                
                var name = collection["Name"];
                var images = collection["images"];
                var price = collection["Price"];
                double ConvertNum = double.Parse(price);
                
                product.ID = id;
                product.Name = name;
                product.images = pathimage;
                product.Price = ConvertNum;

                repository.AddProduct(product);

                return RedirectToAction("Index");
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
                                       Server.MapPath("~/images"), pic);
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
                if (System.IO.File.Exists(Server.MapPath(p.images)))
                {

                    System.IO.File.Delete(Server.MapPath(p.images));
                }


                string pathimage = null;
                if (file != null)
                {
                    //string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/images"));
                    // file is uploaded
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string fileName = id;

                    file.SaveAs(System.IO.Path.Combine(path, fileName + extension));


                    //file.SaveAs(path);
                    pathimage = "~/images/" + fileName + extension;
                }


                Product product = new Product();
                var name = collection["Name"];
                var price = collection["Price"];
                double ConvertNum = double.Parse(price);
                product.ID = id;
                product.Name = name;
                product.Price = ConvertNum;
                product.images = pathimage;
                repository.EditProduct(product);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
                if (System.IO.File.Exists(Server.MapPath(product.images)))
                {

                    System.IO.File.Delete(Server.MapPath(product.images));
                }

                repository.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
