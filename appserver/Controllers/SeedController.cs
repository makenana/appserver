using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using appserver.Data;
using CityModel;

namespace appserver.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(ProjectModelsContext db, IHostEnvironment environment) : ControllerBase
    {
        private readonly string _pathName = Path.Combine(environment.ContentRootPath, "Data/LACountyParks.csv");

        [HttpPost("Cities")]
        public async Task<IActionResult> ImportCitiesAsync()
        {
            Dictionary<string, City> citiesByName = db.Cities
            .AsNoTracking().ToDictionary(x => x.CityName, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            List<LACountyParksCsv> records = csv.GetRecords<LACountyParksCsv>().ToList();
            foreach (LACountyParksCsv record in records)
            {
                if (citiesByName.ContainsKey(record.CITY))
                {
                    continue;
                }
                City city = new()
                {
                    CityName = record.CITY
                };
                await db.Cities.AddAsync(city);
                citiesByName.Add(record.CITY, city);
            }
            await db.SaveChangesAsync();

            return new JsonResult(citiesByName.Count);
        }

        [HttpPost("Parks")]
        public async Task<IActionResult> ImportParksAsync()
        {
            Dictionary<string, City> cities = await db.Cities//.AsNoTracking()
                .ToDictionaryAsync(c => c.CityName);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            int parkCount = 0;
            using (StreamReader reader = new(_pathName))
            using (CsvReader csv = new(reader, config))
            {
                IEnumerable<LACountyParksCsv>? records = csv.GetRecords<LACountyParksCsv>();
                foreach (LACountyParksCsv record in records)
                {
                    if (!cities.TryGetValue(record.CITY, out City? value))
                    {
                        Console.WriteLine($"Not found city for {record.CITY}");
                        return NotFound(record);
                    }
                    Park park = new()
                    {
                        ParkName = record.PARK_NAME,
                        Type = record.TYPE,
                        Address = record.ADDRESS,
                        Acres = record.GIS_ACRES,
                        CityId = value.CityId
                    };
                    db.Parks.Add(park);
                    parkCount++;
                }
                await db.SaveChangesAsync();
            }
            return new JsonResult(parkCount);
        }
}

