using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class registerController : ControllerBase
    {

        RealocationAppContext db = new RealocationAppContext();
        [HttpPost]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            if (await db.Users.AnyAsync(u => u.Username == user.Username || user.Username == "" || user.PasswordHash == ""))
            {
                Console.WriteLine(user.Email);
                return BadRequest("Username already exists.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            db.Users.Add(user);
            await db.SaveChangesAsync();

            // Return a success message or the created user object
            return Ok(new { message = "Registration successful!", user });
        }
    }
}
