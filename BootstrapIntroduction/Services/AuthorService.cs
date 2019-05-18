using BootstrapIntroduction.Behaviors;
using BootstrapIntroduction.DAL;
using BootstrapIntroduction.Models;
using BootstrapIntroduction.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;

using System.Web;

namespace BootstrapIntroduction.Services
{
    public class AuthorService : IDisposable
    {
        private BookContext db = new BookContext();       

        public List<Author> Get(QueryOptions queryOptions)
        {
            var start = QueryOptionsCalculator.CalculateStart(queryOptions);
            var authors = db.Authors
                            .OrderBy(queryOptions.Sort)
                            .Skip(start)
                            .Take(queryOptions.PageSize);

            queryOptions.TotalPages = (int)Math.Ceiling((double)db.Authors.Count() / queryOptions.PageSize);

            return authors.ToList();

        }

        public Author GetById(long id)
        {
            var author = db.Authors.Find(id);

            if(author == null)
            {
                throw new System.Data.Entity.Core.ObjectNotFoundException($"Unable to find author with id {0}");
            }

            return author;
        }

        public void Insert(Author author)
        {

            db.Authors.Add(author);
            db.SaveChanges();
        }

        public void Update(Author author)
        {
            db.Entry(author).State = EntityState.Modified;

            db.SaveChanges();
        }

        public void Delete(Author author)
        {
            db.Authors.Remove(author);
            db.SaveChanges();
        }
        public void Dispose()
        {
            db.Dispose();
        }

    }
}