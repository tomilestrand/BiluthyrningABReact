using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public class CustomerBookingVM
    {
        public string RegNum { get; set; }
        public string CarbookingId { get; set; }
        public int CarType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MilesDriven { get; set; }
    }
}
