using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class RoomType
    {
        [Required]
        public int RoomTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomTypeName { get; set; }

        [StringLength(250)]
        public string TypeDescription { get; set; }

        [StringLength(250)]
        public string TypeNote { get; set; }
    }
}
