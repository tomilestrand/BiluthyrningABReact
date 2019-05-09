using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public class CustomersResponseVM
    {
        public string Status { get; set; }
        public CustomersVM[] Customers { get; set; }
    }
}
