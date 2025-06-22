using DataAccess.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class RoomService
    {
        private readonly RoomRepository _roomRepository;

        public RoomService()
        {
            _roomRepository = new RoomRepository();
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _roomRepository.GetAll();
        }

        public Room GetRoomById(int id)
        {
            return _roomRepository.GetById(id);
        }

        public IEnumerable<Room> SearchRooms(string searchTerm)
        {
            return _roomRepository.Find(r => r.RoomNumber.Contains(searchTerm) || r.RoomDescription.Contains(searchTerm));
        }

        public void AddRoom(Room room)
        {
            _roomRepository.Add(room);
        }

        public void UpdateRoom(Room room)
        {
            _roomRepository.Update(room);
        }

        public void DeleteRoom(int id)
        {
            _roomRepository.Delete(id);
        }

    }
}
