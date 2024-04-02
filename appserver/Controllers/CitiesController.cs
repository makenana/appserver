using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CityModel;

namespace appserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController(ProjectModelsContext context) : ControllerBase
    {
        // GET: api/Cities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities() =>
            await context.Cities.OrderBy(c => c.CityName).ToListAsync();
        
        [HttpGet("CityParks/{id:int}")]
        public async Task<ActionResult<IEnumerable<Park>>> GetCityParksAsync(int id)
        {
            City? city = await context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return await context.Parks.Where(t => t.CityId == id).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            City? city = await context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound(); 
            }
            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutCity(int id, City city)
        {
            if (id != city.CityId)
            {
                return BadRequest();
            }

            context.Entry(city).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
               
                    throw;            
            }

            return NoContent();
        }

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            context.Cities.Add(city);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.CityId }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            City? city = await context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            context.Cities.Remove(city);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(int id) => context.Cities.Any(e => e.CityId == id);
        
    }
}
