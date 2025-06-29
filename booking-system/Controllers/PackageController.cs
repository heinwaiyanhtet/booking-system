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
    public class PackageController : Controller
    {
        private readonly PackageService _packages;
        public PackageController(PackageService packages) => _packages = packages;


        // Users can see the available package list to buy for each country

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageDto>>> Get()
        {
            var list = await _packages.GetAllPackagesAsync();
            return Ok(list);
        }

        //  User can see the available class schedule list for each country with class info
        
        [HttpGet("country/{country}")]
        public async Task<ActionResult<IEnumerable<PackageDto>>> GetByCountry(string country)
        {
            var list = await _packages.GetPackagesAsync(country);
            return Ok(list.Select(p => new PackageDto(p.Id, p.Name, p.Country, p.Credits, p.Price, p.ExpireAt)));
        }


        //   Users can see purchased own package (expired pack need to show with
        //   expired status) and remaining credit
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserPackageDto>>> GetUserPackages(int userId)
        {
            var list = await _packages.GetUserPackagesAsync(userId);
            return Ok(list.Select(up => new UserPackageDto(up.PackageId, up.Package!.Name, up.Package!.ExpireAt < DateTime.UtcNow, up.RemainingCredits)));
        }


    }

}