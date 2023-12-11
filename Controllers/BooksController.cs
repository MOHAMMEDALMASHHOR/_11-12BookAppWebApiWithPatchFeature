using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookDemo.Repository;
using BookDemo.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.JsonPatch;

namespace BookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> logger;

        public BooksController(ILogger<BooksController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicaionContext.Books;
            logger.LogInformation("GetAllBook method has been called");
            return Ok(books);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = ApplicaionContext.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();
            logger.LogInformation("GetAllBook method has been called");
            if (book == null) { return NotFound(); }
            return Ok(book);
        }
        [HttpPost]
        public IActionResult AddOneBook([FromBody] Book book)
        {
            try
            {
                ApplicaionContext.Books.Add(book);
                logger.LogInformation("AddOneBook method has been called");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            var entity = ApplicaionContext.Books.Find(b => b.Id == id);//find is better than where because find can return a null
            if (entity == null)
            {
                return NotFound();
            }
            if (id != book.Id)
            {
                return BadRequest();
            }
            ApplicaionContext.Books.Remove(entity);
            ApplicaionContext.Books.Add(book);
            return Ok(book);
            //we can do it with another way, but we can't use LINQ in put at all

        }
        [HttpDelete]
        public IActionResult DeleteAllBooks() {
            ApplicaionContext.Books.Clear();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook(int id)
        {
            var entity=ApplicaionContext.Books.Find(book=>book.Id == id);
            if (entity == null)
            {
                return NotFound(new
                {
                    statusCode= 404,
                    message = $"Book with Id: {id} is not found!!!"
                });
                //409 conflic
                //415 unsupported media types
            }
            ApplicaionContext.Books.Remove(entity);
            return Ok($"The book: {entity.ToString()} is deleted succeesfully");
        }
        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name ="id")]int id,[FromBody] JsonPatchDocument<Book> bookPatch)
        {
            //To add patch we have first to download a package by opening nuget conosle and write this"Install-Package Microsoft.AspNetCore.JsonPatch" 
            //and by coming to the controller we have to use Microsoft.AspNetCore.JsonPatch; 

            //check entity
            var entity = ApplicaionContext.Books.Find(b => b.Id.Equals(id));
            if (entity is null)
            {
                return NotFound($"The Book: {entity.ToString()}");
            }
            bookPatch.ApplyTo(entity);
            return NoContent();
        }
          
    }
}
