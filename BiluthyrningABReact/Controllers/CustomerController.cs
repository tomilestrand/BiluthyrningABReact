using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiluthyrningABReact.Models;
using BiluthyrningABReact.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiluthyrningABReact.Controllers
{
    public class CustomerController : Controller
    {
        CarRentalService service;

        public CustomerController(CarRentalService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("/activerents")]
        public async Task<IActionResult> ActiveRents()
        {
            return Json(await service.GetAllActiveRents());
        }

        [HttpGet]
        [Route("/customers")]
        public async Task<IActionResult> Customers()
        {
            return Json(await service.GetAllCustomers());
        }

        [HttpPost]
        [Route("/customers")]
        public async Task<IActionResult> Customers([FromBody]CustomerBookingsSubmitVM SSN)
        {
            return Json(await service.GetCustomerBookings(SSN.SSN));
        }
    }
}
