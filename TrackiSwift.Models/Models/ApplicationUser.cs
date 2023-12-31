﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackiSwift.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(10)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
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
