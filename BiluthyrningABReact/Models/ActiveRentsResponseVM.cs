using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public class ActiveRentsResponseVM
    {
        public string Status { get; set; }
        public ActiveRentsVM[] ActiveRents { get; set; }
    }
}
