using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCategoriesController : ControllerBase
    {
        RealocationAppContext db = new RealocationAppContext();

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetUserCategories(int userId)
        {
            var categories = await db.UserCategories
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.Category)
                .ToListAsync();

            if (categories == null || !categories.Any())
            {
                return NotFound("No categories found for this user.");
            }

            return Ok(categories);
        }
    }
}
