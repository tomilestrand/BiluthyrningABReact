﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public class AvailableCarsResponse
    {
        public string Status { get; set; }
        public Car[] Cars { get; set; }
    }
}