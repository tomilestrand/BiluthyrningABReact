using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public class CarDM
    {
        public int CarType { get; set; }
        public string RegNum { get; set; }
        public int NumOfKm { get; set; }
        public bool Retired { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
