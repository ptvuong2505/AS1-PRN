using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Booking
    {
        [Required]
        public int BookingID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public int RoomID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        public Customer Customer { get; set; } // Navigation property
        public Room Room { get; set; } // Navigation property
    }
}
