using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackiSwift.Models.Models
{
    public class Rider
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser Users { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("BookingId")]
        public Order Orders { get; set; }

    }
}
