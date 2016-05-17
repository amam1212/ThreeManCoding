using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models.Entities;

namespace WebApplication3.Models.Repository
{
    public interface INewsRepository
    {
        void AddNews(News news);
        void DeleteNews(string id);
        void EditNews(News news);
        List<News> GetNews();
        News GetNewsByID(string id);
        void SaveNews();
    }
}