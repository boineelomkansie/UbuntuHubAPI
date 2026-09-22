using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UbuntuHub.Api.Controllers;
using UbuntuHub.Api.Data;
using UbuntuHub.Api.Models;

namespace UbuntuHub.Api.Tests
{
    public class PostsControllerTests
    {
        private UbuntuHubDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<UbuntuHubDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new UbuntuHubDbContext(options);

            context.Users.Add(new User
            {
                Id = 1,
                FirebaseUid = "test-firebase-uid",
                Username = "Test User",
                Email = "test@example.com"
            });

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task GetPosts_ReturnsSuccess()
        {
            using var context = CreateDbContext();

            context.Posts.Add(new Post
            {
                UserId = 1,
                Title = "Test Post",
                Description = "Test description",
                Location = "",
                Category = "Community"
            });

            await context.SaveChangesAsync();

            var controller = new PostsController(context);

            var result = await controller.GetPosts();

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task CreatePost_WithValidData_ReturnsCreated()
        {
            using var context = CreateDbContext();

            var controller = new PostsController(context);

            var request = new CreatePostRequest
            {
                UserId = 1,
                Title = "Test Post",
                Description = "This is a test post.",
                Location = "",
                Category = "Community"
            };

            var result = await controller.CreatePost(request);

            var createdResult =
                Assert.IsType<CreatedAtActionResult>(result);

            Assert.NotNull(createdResult.Value);
        }

        [Fact]
        public async Task CreatePost_WithEmptyDescription_ReturnsBadRequest()
        {
            using var context = CreateDbContext();

            var controller = new PostsController(context);

            var request = new CreatePostRequest
            {
                UserId = 1,
                Title = "Test Post",
                Description = "",
                Location = "",
                Category = "Community"
            };

            var result = await controller.CreatePost(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreatePost_WithInvalidUser_ReturnsBadRequest()
        {
            using var context = CreateDbContext();

            var controller = new PostsController(context);

            var request = new CreatePostRequest
            {
                UserId = 99999,
                Title = "Test Post",
                Description = "This should fail.",
                Location = "",
                Category = "Community"
            };

            var result = await controller.CreatePost(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreatePost_WithEmptyCategory_ReturnsBadRequest()
        {
            using var context = CreateDbContext();

            var controller = new PostsController(context);

            var request = new CreatePostRequest
            {
                UserId = 1,
                Title = "Test Post",
                Description = "This should fail.",
                Location = "",
                Category = ""
            };

            var result = await controller.CreatePost(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}