using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookings;
        public BookingController(BookingService bookings) => _bookings = bookings;

        // User can see the available class schedule list for each country with class info
        // and can book the class
        
        [HttpPost]
        public async Task<ActionResult<BookingDto>> Book(int userId, int classId)
        {
            var booking = await _bookings.BookClassAsync(userId, classId);
            if (booking == null) return NotFound();
            return Ok(new BookingDto(booking.Id, booking.ClassScheduleId, booking.Canceled, booking.BookedAt));
        }
    }
}