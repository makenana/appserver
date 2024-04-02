using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CityModel;

namespace appserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParksController(ProjectModelsContext context) : ControllerBase
    {
        // GET: api/Parks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Park>>> GetParks()
        {
            return await context.Parks.Take(100).ToListAsync();
        }

        // GET: api/Parks/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Park>> GetPark(int id)
        {
            Park? park = await context.Parks.FindAsync(id);

            if (park == null)
            {
                return NotFound();
            }

            return park;
        }

        // PUT: api/Parks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutPark(int id, Park park)
        {
            if (id != park.ParkId)
            {
                return BadRequest();
            }

            context.Entry(park).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParkExists(id))
                {
                    return NotFound();
                }
               
                    throw;
            }

            return NoContent();
        }

        // POST: api/Parks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Park>> PostPark(Park park)
        {
            context.Parks.Add(park);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetPark", new { id = park.ParkId }, park);
        }

        // DELETE: api/Parks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePark(int id)
        {
            var park = await context.Parks.FindAsync(id);
            if (park == null)
            {
                return NotFound();
            }

            context.Parks.Remove(park);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParkExists(int id) => context.Parks.Any(e => e.ParkId == id);
    }
}
