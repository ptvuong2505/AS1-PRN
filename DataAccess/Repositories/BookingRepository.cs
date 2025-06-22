using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class BookingRepository : IRepository<Booking>
    {
        private readonly DataContext _context;

        public BookingRepository()
        {
            _context = DataContext.Instance;
        }

        public IEnumerable<Booking> GetAll()
        {
            var bookings = _context.Bookings;
            // Load rooms and room types for each booking
            foreach (var booking in bookings)
            {
                booking.Room = _context.Rooms.FirstOrDefault(r => r.RoomID == booking.RoomID);
                if (booking.Room != null)
                {
                    booking.Room.RoomType = _context.RoomTypes.FirstOrDefault(rt => rt.RoomTypeID == booking.Room.RoomTypeID);
                }
            }
            return bookings;
        }

        public Booking GetById(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingID == id);
            if (booking != null)
            {
                booking.Room = _context.Rooms.FirstOrDefault(r => r.RoomID == booking.RoomID);
                if (booking.Room != null)
                {
                    booking.Room.RoomType = _context.RoomTypes.FirstOrDefault(rt => rt.RoomTypeID == booking.Room.RoomTypeID);
                }
            }
            return booking;
        }

        public IEnumerable<Booking> Find(Expression<Func<Booking, bool>> predicate)
        {
            var bookings = _context.Bookings.AsQueryable().Where(predicate).ToList();
            // Load rooms and room types for each booking
            foreach (var booking in bookings)
            {
                booking.Room = _context.Rooms.FirstOrDefault(r => r.RoomID == booking.RoomID);
                if (booking.Room != null)
                {
                    booking.Room.RoomType = _context.RoomTypes.FirstOrDefault(rt => rt.RoomTypeID == booking.Room.RoomTypeID);
                }
            }
            return bookings;
        }

        public void Add(Booking entity)
        {
            entity.BookingID = _context.Bookings.Any() ? _context.Bookings.Max(b => b.BookingID) + 1 : 1;
            _context.Bookings.Add(entity);
        }

        public void Update(Booking entity)
        {
            var existing = _context.Bookings.FirstOrDefault(b => b.BookingID == entity.BookingID);
            if (existing != null)
            {
                existing.CustomerID = entity.CustomerID;
                existing.RoomID = entity.RoomID;
                existing.StartDate = entity.StartDate;
                existing.EndDate = entity.EndDate;
                existing.TotalPrice = entity.TotalPrice;
            }
        }

        public void Delete(int id)
        {
            var entity = _context.Bookings.FirstOrDefault(b => b.BookingID == id);
            if (entity != null)
            {
                _context.Bookings.Remove(entity);
            }
        }
    }
}
