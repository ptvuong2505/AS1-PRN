using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Room
    {
        [Required]
        public int RoomID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomNumber { get; set; }

        [StringLength(220)]
        public string RoomDescription { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RoomMaxCapacity { get; set; }

        [Required]
        [Range(1, 2)]
        public int RoomStatus { get; set; } // 1 Active, 2 Deleted

        [Required]
        [Range(0, double.MaxValue)]
        public decimal RoomPricePerDate { get; set; }

        [Required]
        public int RoomTypeID { get; set; }

        public RoomType RoomType { get; set; } // Navigation property
    }
}
