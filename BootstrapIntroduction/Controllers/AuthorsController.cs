using AutoMapper;
using BootstrapIntroduction.DAL;
using BootstrapIntroduction.Filters;
using BootstrapIntroduction.Models;
using BootstrapIntroduction.Services;
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
    [RoutePrefix("Writer")]
    public class AuthorsController : Controller
    {

        private AuthorService authorService;
        private MapperConfiguration configuration;

        public AuthorsController()
        {
            authorService = new AuthorService();


            configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorViewModel>();
            });

        }

        // GET: Authors
        [GenerateResultListFilter(typeof(Author), typeof(AuthorViewModel))]
        [Route("~/Writers")]
        public ActionResult Index([Form] QueryOptions queryOptions)
        {
            var authors = authorService.Get(queryOptions);

            ViewData["QueryOptions"] = queryOptions;

            return View(authors.ToList());
        }

        [Route("Details/{id:int:min(0)?}")]
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var autor = authorService.GetById(id.Value);

            if(autor == null)
            {
                throw new System.Data.Entity.Core.ObjectNotFoundException($"Unable to find author with id {id}");
            }
            var mapper = configuration.CreateMapper();

            return View("Form",mapper.Map<Author,AuthorViewModel>(autor));
        }

        [BasicAuthorization]
        public ActionResult Create()
        {
            return View("Form", new AuthorViewModel());
        }

        [BasicAuthorization]
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var author = authorService.GetById(id.Value);

            if(author == null)
            {
                return HttpNotFound();
            }

            var mapper = configuration.CreateMapper();

            return View("Form", mapper.Map<Author,AuthorViewModel>(author));
        }

        
        [BasicAuthorization]
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var author = authorService.GetById(id.Value);

            if(author == null)
            {
                return HttpNotFound();
            }
            var mapper = configuration.CreateMapper();

            return View(mapper.Map<Author, AuthorViewModel>(author));
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [BasicAuthorization]
        public ActionResult DeleteConfirmed(int id)
        {
            var author = authorService.GetById(id);

            authorService.Delete(author);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                authorService.Dispose();

            base.Dispose(disposing);
        }




    }
}