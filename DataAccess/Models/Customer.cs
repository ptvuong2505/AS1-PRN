using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Customer
    {
        [Required]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerFullName { get; set; }

        [Required]
        [StringLength(12)]
        public string Telephone { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public DateTime? CustomerBirthday { get; set; }

        [Required]
        [Range(1, 2)]
        public int CustomerStatus { get; set; } // 1 Active, 2 Deleted

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
