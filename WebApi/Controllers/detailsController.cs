using Data.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO;

namespace WebApi.Controllers
{
    [EnableCors()]

    public class detailsController : ControllerBase
    {

         RealocationAppContext db = new RealocationAppContext();


        //[HttpPost]
        //public async Task<ActionResult<RelocationDetail>> PostRelocationDetail([FromBody] RelocationDetail detail)
        //{
        //    db.RelocationDetails.Add(detail);
        //    await db.SaveChangesAsync();
        //    return Ok(new { message = "Relocation details submitted!", detail });
        //}



        [HttpPost]
        public async Task<IActionResult> PostRelocationDetail([FromBody] RelocationDetailDTO relocationDetailDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var relocationDetail = new RelocationDetail
            {
                UserId = relocationDetailDto.UserId,
                DestinationCountry = relocationDetailDto.DestinationCountry,
                MoveDate = relocationDetailDto.MoveDate,
                HasChildren = relocationDetailDto.HasChildren,
                CreatedAt = DateTime.Now
            };

            db.RelocationDetails.Add(relocationDetail);
            await db.SaveChangesAsync();

            // Insert selected categories into UserCategories
            foreach (var categoryId in relocationDetailDto.SelectedCategories)
            {
                var userCategory = new UserCategory
                {
                    UserId = relocationDetailDto.UserId,
                    CategoryId = categoryId,
                    CreatedAt = DateTime.Now
                };
                db.UserCategories.Add(userCategory);
            }

            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRelocationDetail), new { id = relocationDetail.RelocationId }, relocationDetail);
        }

        [HttpGet]
        [Route("api/")]
        public async Task<IActionResult> GetRelocationDetail(int id)
        {
            var relocationDetail = await db.RelocationDetails.FindAsync(id);

            if (relocationDetail == null)
            {
                return NotFound();
            }

            return Ok(relocationDetail);
        }

    }
}
