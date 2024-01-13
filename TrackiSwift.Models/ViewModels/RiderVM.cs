using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackiSwift.Models.Models;

namespace TrackiSwift.Models.ViewModels
{
    public class RiderVM
    {
        public List<Order>? Orders { get; set; }
        public List<Rider>? Riders { get; set; }
        public List<ApplicationUser>? ApplicationUsers { get; set; }
    }
}
