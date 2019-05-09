using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public class CarVM
    {
        public string RegNum { get; set; }
        public int CarType { get; set; }
        public int NumOfKm { get; set; }
    }
}
