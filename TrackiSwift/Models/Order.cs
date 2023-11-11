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

        public string ReceiverName {  get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        [Required]
        [DisplayName("Delivery Address")]
        public string DeliveryAddress { get; set; }
        [Required]
        public double Weight {  get; set; }
        [Required]
        public double Amount {  get; set; }

    }
}
