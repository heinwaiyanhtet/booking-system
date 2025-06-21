using BookingSystem.Entities;
using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackageController : ODataController
    {
        private readonly PackageService _packages;
        public PackageController(PackageService packages) => _packages = packages;

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Package>>> Get()
        {
            var list = await _packages.GetAllPackagesAsync(); 
            return Ok(list);
        }

    }

}