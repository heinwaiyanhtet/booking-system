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




        // book the class
        [HttpPost]
        public async Task<ActionResult<BookingDto>> Book(int userId, int classId)
        {
            var booking = await _bookings.BookClassAsync(userId, classId);
            Console.WriteLine(booking);
            if (booking == null) return NotFound();
            return Ok(new BookingDto(booking.Id, booking.ClassScheduleId, booking.Canceled, booking.BookedAt));
        }
        




        [HttpPost("{bookingId}/cancel")]
        public async Task<ActionResult> Cancel(int bookingId)
        {
            var ok = await _bookings.CancelAsync(bookingId);
            if (!ok) return NotFound();
            return Ok();
        }

        [HttpPost("{bookingId}/checkin")]
        public async Task<ActionResult> CheckIn(int bookingId)
        {
            var ok = await _bookings.CheckInAsync(bookingId);
            if (!ok) return NotFound();
            return Ok();
        }
        
    }
}