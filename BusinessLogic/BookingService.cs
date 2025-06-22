using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly RoomRepository _roomRepository;

        public BookingService()
        {
            _bookingRepository = new BookingRepository();
            _roomRepository = new RoomRepository();
        }

        public IEnumerable<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAll();
        }

        public IEnumerable<Booking> GetBookingsByCustomer(int customerId)
        {
            return _bookingRepository.Find(b => b.CustomerID == customerId);
        }

        public IEnumerable<Booking> GetBookingsByPeriod(DateTime startDate, DateTime endDate)
        {
            return _bookingRepository.Find(b => b.StartDate >= startDate && b.EndDate <= endDate)
                .OrderByDescending(b => b.TotalPrice);
        }

        public void AddBooking(Booking booking)
        {
            var room = _roomRepository.GetById(booking.RoomID);
            if (room != null)
            {
                var days = (booking.EndDate - booking.StartDate).Days;
                booking.TotalPrice = room.RoomPricePerDate * days;
                _bookingRepository.Add(booking);
            }
        }

        public void UpdateBooking(Booking booking)
        {
            var room = _roomRepository.GetById(booking.RoomID);
            if (room != null)
            {
                var days = (booking.EndDate - booking.StartDate).Days;
                booking.TotalPrice = room.RoomPricePerDate * days;
                _bookingRepository.Update(booking);
            }
        }

        public void DeleteBooking(int id)
        {
            _bookingRepository.Delete(id);
        }
    }
}
