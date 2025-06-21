using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly PackageService _packages;
        public PackageController(PackageService packages) => _packages = packages;

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<PackageDto>>> Get([FromQuery] string country)
        {
            var list = await _packages.GetPackagesAsync(country);
            return Ok(list.Select(p => new PackageDto(p.Id, p.Name, p.Country, p.Credits, p.Price, p.ExpireAt)));
        }


        
    }
}