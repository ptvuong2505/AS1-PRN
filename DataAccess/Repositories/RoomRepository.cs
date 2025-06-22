using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RoomRepository : IRepository<Room>
    {
        private readonly DataContext _context;

        public RoomRepository()
        {
            _context = DataContext.Instance;
        }

        public IEnumerable<Room> GetAll()
        {
            return _context.Rooms.Where(r => r.RoomStatus == 1); // Only active rooms
        }

        public Room GetById(int id)
        {
            return _context.Rooms.FirstOrDefault(r => r.RoomID == id && r.RoomStatus == 1);
        }

        public IEnumerable<Room> Find(Expression<Func<Room, bool>> predicate)
        {
            return _context.Rooms.AsQueryable().Where(predicate).Where(r => r.RoomStatus == 1);
        }

        public void Add(Room entity)
        {
            entity.RoomID = _context.Rooms.Any() ? _context.Rooms.Max(r => r.RoomID) + 1 : 1;
            _context.Rooms.Add(entity);
        }

        public void Update(Room entity)
        {
            var existing = _context.Rooms.FirstOrDefault(r => r.RoomID == entity.RoomID);
            if (existing != null)
            {
                existing.RoomNumber = entity.RoomNumber;
                existing.RoomDescription = entity.RoomDescription;
                existing.RoomMaxCapacity = entity.RoomMaxCapacity;
                existing.RoomStatus = entity.RoomStatus;
                existing.RoomPricePerDate = entity.RoomPricePerDate;
                existing.RoomTypeID = entity.RoomTypeID;
            }
        }

        public void Delete(int id)
        {
            var entity = _context.Rooms.FirstOrDefault(r => r.RoomID == id);
            if (entity != null)
            {
                entity.RoomStatus = 2; // Mark as deleted
            }
        }
    }
}
