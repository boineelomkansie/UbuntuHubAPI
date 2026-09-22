using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UbuntuHub.Api.Data;
using UbuntuHub.Api.Models;

namespace UbuntuHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly UbuntuHubDbContext _context;

        public PostsController(UbuntuHubDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(
            [FromBody] CreatePostRequest request)
        {
            if (request == null)
                return BadRequest("Post data is required.");

            if (request.UserId <= 0)
                return BadRequest("A valid user is required.");

            var userExists = await _context.Users
                .AnyAsync(u => u.Id == request.UserId);

            if (!userExists)
                return BadRequest("The specified user does not exist.");

            if (string.IsNullOrWhiteSpace(request.Title))
                return BadRequest("Title is required.");

            if (string.IsNullOrWhiteSpace(request.Description))
                return BadRequest("Description is required.");

            if (string.IsNullOrWhiteSpace(request.Category))
                return BadRequest("Category is required.");

            var post = new Post
            {
                UserId = request.UserId,
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                Location = request.Location?.Trim() ?? string.Empty,
                Category = request.Category.Trim()
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPosts),
                new { id = post.Id },
                post
            );
        }
    }
}