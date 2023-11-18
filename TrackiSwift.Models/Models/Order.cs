﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TrackiSwift.Models
{
    public class Order
    {
        [Key]
        [DisplayName("Order Id")]
        public int OrderId { get; set; }
        [Required]
        [DisplayName("Receiver Name")]

        public string ReceiverName { get; set; }
        [Required]
        [DisplayName("Receiver Number")]
        [MaxLength(10)]
        [MinLength(10, ErrorMessage = "Number must be of Length 10.")]
        public string ReceiverNumber { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now.Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
        [Required]
        [DisplayName("Delivery Address")]
        public string DeliveryAddress { get; set; }
        [Required]
        [DisplayName("Weight (KG)")]
        public double Weight { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        [DisplayName("Delivery Status")]
        public string DeliveryStatus { get; set; }
        [Required]
        [DisplayName("Payment Status")]
        public string PaymentStatus { get; set; }

    }
}
