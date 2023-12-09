using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackiSwift.Models.ViewModels
{
    public class UserVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } 
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Ward No must be greater than 0.")]
        public int WardNo { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}
