using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public class CustomerBookingsVM
    {
        public string Status { get; set; }
        public CustomerBookingVM[] CustomerBookings { get; set; }
    }
}
