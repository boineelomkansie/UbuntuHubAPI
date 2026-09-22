using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UbuntuHub.Api.Data;
using UbuntuHub.Api.Models;

namespace UbuntuHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UbuntuHubDbContext _context;

        public UsersController(UbuntuHubDbContext context)
        {
            _context = context;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncUser(
            [FromBody] UserSyncRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FirebaseUid))
                return BadRequest("Firebase UID is required.");

            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.FirebaseUid == request.FirebaseUid);

            if (existingUser == null)
            {
                var newUser = new User
                {
                    FirebaseUid = request.FirebaseUid.Trim(),
                    Username = request.Username.Trim(),
                    Email = request.Email.Trim()
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(SyncUser),
                    new { id = newUser.Id },
                    newUser
                );
            }

            existingUser.Username = request.Username.Trim();
            existingUser.Email = request.Email.Trim();

            await _context.SaveChangesAsync();

            return Ok(existingUser);


        }

        [HttpGet("by-firebase/{firebaseUid}")]
        public async Task<IActionResult> GetUserByFirebaseUid(string firebaseUid)
        {
            if (string.IsNullOrWhiteSpace(firebaseUid))
                return BadRequest("Firebase UID is required.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid);

            if (user == null)
                return NotFound("User not found.");

            return Ok(user);
        }
    }
}