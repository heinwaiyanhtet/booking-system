using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly ClassService _classes;
        public ClassController(ClassService classes) => _classes = classes;


        //  Users can see the available package list to buy for each country.
        
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ClassDto>>> Get([FromQuery] string country)
        {
            var list = await _classes.GetSchedulesAsync(country);
            return Ok(list.Select(c => new ClassDto(c.Id, c.Title, c.Country, c.RequiredCredits, c.StartTime, c.Capacity)));
        }
    }
}