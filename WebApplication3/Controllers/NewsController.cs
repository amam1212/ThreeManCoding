using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models.EF;
using WebApplication3.Models.Entities;
using WebApplication3.Models.Repository;

namespace WebApplication3.Controllers
{
    public class NewsController : Controller
    {
        INewsRepository repository = new NewsRepository();
        // GET: News
        public ActionResult Index()
        {
            return View(repository.GetNews());
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                string pathimage = null;

                EfDbContext _context = new EfDbContext();

                int count = 0;
                foreach (News n in _context.Newsfeed)
                {
                    count = int.Parse(n.NewsID);

                }
                count++;

                string id = count.ToString();

                if (file != null)
                {
                    //string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/News/"));
                    // file is uploaded
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string fileName = id;
                    file.SaveAs(System.IO.Path.Combine(path, fileName + extension));

                    //file.SaveAs(path);
                    pathimage = "~/Content/Images/News/" + fileName + extension;
                }


                
                var title = collection["NewsTitle"];
                var descrip = collection["NewsDescription"];
     
                News news = new News();
                news.NewsID = id;
                news.NewsTitle = title;
                news.NewsImages = pathimage;
                news.NewsDescription = descrip;
                repository.AddNews(news);


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: News/Edit/5
        public ActionResult Edit(string id)
        {
            News news = repository.GetNewsByID(id);
            return View(news);
        }

        // POST: News/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                News news = repository.GetNewsByID(id);
                if (System.IO.File.Exists(Server.MapPath(news.NewsImages)))
                {

                    System.IO.File.Delete(Server.MapPath(news.NewsImages));
                }


                string pathimage = null;
                if (file != null)
                {
                    //string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(
                                           Server.MapPath("~/Content/Images/News"));
                    // file is uploaded
                    string extension = System.IO.Path.GetExtension(file.FileName);
                    string fileName = id;

                    file.SaveAs(System.IO.Path.Combine(path, fileName + extension));


                    //file.SaveAs(path);
                    pathimage = "~/Content/Images/News/" + fileName + extension;
                }

                if (file == null)
                {

                    pathimage = news.NewsImages;
                }

                var title = collection["NewsTitle"];
                var descrip = collection["NewsDescription"];

                news.NewsID = id;
                news.NewsTitle = title;
                news.NewsImages = pathimage;
                news.NewsDescription = descrip;
                repository.EditNews(news);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: News/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: News/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
