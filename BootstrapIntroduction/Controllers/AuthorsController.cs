using AutoMapper;
using BootstrapIntroduction.DAL;
using BootstrapIntroduction.Filters;
using BootstrapIntroduction.Models;
using BootstrapIntroduction.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace BootstrapIntroduction.Controllers
{
    public class AuthorsController : Controller
    {

        private BookContext db = new BookContext();
        // GET: Authors
        [GenerateResultListFilter(typeof(Author), typeof(AuthorViewModel))]
        public ActionResult Index([Form] QueryOptions queryOptions)
        {
            var start = (queryOptions.CurrentPage - 1) * queryOptions.PageSize;
          
            var authors = db.Authors
                            .OrderBy(queryOptions.Sort)
                            .Skip(start)
                            .Take(queryOptions.PageSize);

            queryOptions.TotalPages = (int)Math.Ceiling((double)db.Authors.Count() / queryOptions.PageSize);

            ViewData["QueryOptions"] = queryOptions;
            return View(authors.ToList());
        }

        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var autor = db.Authors.Find(id);

            if(autor == null)
            {
                throw new System.Data.Entity.Core.ObjectNotFoundException($"Unable to find author with id {id}");
            }

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorViewModel>();
            });
            
            var mapper = configuration.CreateMapper();

            return View("Form",mapper.Map<Author,AuthorViewModel>(autor));
        }


        public ActionResult Create()
        {
            return View("Form", new AuthorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="Id,FirstName,LastName,Biography")] AuthorViewModel author)
        {
            if (ModelState.IsValid)
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<AuthorViewModel, Author>();
                });

                var mapper = configuration.CreateMapper();
                
                db.Authors.Add(mapper.Map<AuthorViewModel,Author>(author));
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            

            return View(author);
        }

        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var author = db.Authors.Find(id);

            if(author == null)
            {
                return HttpNotFound();
            }

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorViewModel>();
            });

            var mapper = configuration.CreateMapper();


            return View("Form", mapper.Map<Author,AuthorViewModel>(author));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Biography")] AuthorViewModel author)

        {
            if(ModelState.IsValid)
            {

                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Author, AuthorViewModel>();
                });

                var mapper = configuration.CreateMapper();

                db.Entry(mapper.Map<AuthorViewModel,Author>(author)).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(author);
        }

        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = db.Authors.Find(id);

            if(author == null)
            {
                return HttpNotFound();
            }

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorViewModel>();
            });

            var mapper = configuration.CreateMapper();

            return View(mapper.Map<Author, AuthorViewModel>(author));
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var author = db.Authors.Find(id);
            db.Authors.Remove(author);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }




    }
}