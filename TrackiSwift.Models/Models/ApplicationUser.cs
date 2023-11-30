using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TrackiSwift.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string UserName { get; set; } 
        [Required]
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Ward No must be greater than 0.")]
        public int WardNo { get; set; }
    }
}
