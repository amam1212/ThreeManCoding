using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication3.Models.EF;
using WebApplication3.Models.Entities;

namespace WebApplication3.Models.Repository
{
    public class NewsRepository : INewsRepository
    {
        EfDbContext _context = new EfDbContext();
        public void AddNews(News news)
        {
            _context.Newsfeed.Add(news);
            SaveNews();
            
        }

        public void DeleteNews(string id)
        {
            News news = _context.Newsfeed.Find(id);
            _context.Newsfeed.Remove(news);
            SaveNews();
        }

        public void EditNews(News news)
        {
            _context.Entry(news).State = EntityState.Modified;
            SaveNews();
        }

        public List<News> GetNews()
        {
            return _context.Newsfeed.ToList();
        }

        public News GetNewsByID(string id)
        {
            return _context.Newsfeed.Find(id);
        }

        public void SaveNews()
        {
            _context.SaveChanges();
        }
    }
}