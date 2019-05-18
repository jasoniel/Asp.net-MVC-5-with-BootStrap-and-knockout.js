using AutoMapper;
using BootstrapIntroduction.DAL;
using BootstrapIntroduction.Models;
using BootstrapIntroduction.Services;
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
        
        private AuthorService authorService;
        private MapperConfiguration configuration;
        private IMapper mapper;
        public AuthorsController()
        {

            authorService = new AuthorService();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorViewModel>();
                cfg.CreateMap<AuthorViewModel,Author>();
            });

            mapper = configuration.CreateMapper();

        }

        public ResultList<AuthorViewModel> Get([FromUri] QueryOptions queryOptions)
        {
            var authors = authorService.Get(queryOptions);
            return new ResultList<AuthorViewModel>(
                    mapper.Map<List<Author>, List<AuthorViewModel>>(authors), queryOptions
                );
        }

        [ResponseType(typeof(AuthorViewModel))]
        public IHttpActionResult Get(int id )
        {

            var author = authorService.GetById(id);

            if(author == null)
            {
                throw new System.Data.Entity.Core.ObjectNotFoundException($"Unable to find author with id {id}");
            }

           
            return Ok(mapper.Map<Author,AuthorViewModel>(author));
        } 
        
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            authorService.Update(mapper.Map<AuthorViewModel, Author>(author));

            return StatusCode(HttpStatusCode.NoContent);
        }



        public IHttpActionResult Post(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            authorService.Insert(mapper.Map<AuthorViewModel, Author>(author));

            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);

        }

        [ResponseType(typeof(Author))]
        public IHttpActionResult DeleteAuthor(int id)
        {

            var author = authorService.GetById(id);
            authorService.Delete(author);

            return Ok(author);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                authorService.Dispose();

            base.Dispose(disposing);             
        }

    }
}