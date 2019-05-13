using AutoMapper;
using BootstrapIntroduction.DAL;
using BootstrapIntroduction.Models;
using BootstrapIntroduction.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BootstrapIntroduction.Controllers.api
{
    public class AuthorsController : ApiController
    {


        private BookContext db = new BookContext();



        public ResultList<AuthorViewModel> Get([FromUri] QueryOptions queryOptions )
        {


            var start = (queryOptions.CurrentPage - 1) * queryOptions.PageSize;

            var authors = db.Authors
                    .OrderBy(queryOptions.Sort)
                    .Skip(start)
                    .Take(queryOptions.PageSize);

            queryOptions.TotalPages = (int)Math.Ceiling((double)db.Authors.Count() / queryOptions.PageSize);

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorViewModel>();
            });

            var mapper = configuration.CreateMapper();

            return new ResultList<AuthorViewModel>
            {
                QueryOptions = queryOptions,
                Results = mapper.Map<List<Author>, List<AuthorViewModel>>(db.Authors.ToList())

            };

        } 
        
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AuthorViewModel, Author>();
            });

            var mapper = configuration.CreateMapper();

            db.Entry(mapper.Map<AuthorViewModel, Author>(author)).State = EntityState.Modified;

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }



        public IHttpActionResult Post(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AuthorViewModel, Author>();
            });

            var mapper = configuration.CreateMapper();

            db.Authors.Add(mapper.Map<AuthorViewModel, Author>(author));
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);             
        }

    }
}