using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public class RentFormResponseVM
    {
        public string Status { get; set; }
        public string RegNum { get; set; }
        public int CarbookingId { get; set; }
        public int CarType { get; set; }
    }
}
