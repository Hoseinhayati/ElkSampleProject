using ElkProject.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElkProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;

        public BooksController(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        [HttpPost]
        public async Task<IActionResult> IndexBook([FromBody] Book book)
        {
            var indexResponse = await _elasticClient.IndexDocumentAsync(book);
            if (indexResponse.IsValid)
            {
                return Ok("Book indexed successfully.");
            }
            return BadRequest("Failed to index book.");
        }

        [HttpGet("{query}")]
        public async Task<IActionResult> SearchBooks(string query)
        {
            var searchResponse = await _elasticClient.SearchAsync<Book>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Title)
                        .Query(query)
                    )
                )
            );

            if (searchResponse.IsValid)
            {
                var books = searchResponse.Documents;
                return Ok(books);
            }
            return NotFound();
        }
    }
}
