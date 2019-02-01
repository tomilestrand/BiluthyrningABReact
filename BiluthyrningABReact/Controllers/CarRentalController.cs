using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiluthyrningABReact.Models;
using BiluthyrningABReact.Services;
using Microsoft.AspNetCore.Mvc;

namespace BiluthyrningABReact.Controllers
{
    public class CarRentalController : Controller
    {
        CarRentalService service;

        public CarRentalController(CarRentalService service)
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
        [Route("/rentcar")]
        public async Task<IActionResult> RentCar([FromBody]RentFormSubmitVM json)
        {
            return Json(await service.MakeRentFormResponseVM(json));
        }

        [HttpPost]
        [Route("/customers")]
        public async Task<IActionResult> Customers([FromBody]CustomerBookingsSubmit SSN)
        {
            return Json(await service.GetCustomerBookings(SSN.SSN));
        }

        //[Route("/rentcar")]
        //[HttpGet]
        //public IActionResult RentCar()
        //{
        //    return Ok();
        //}

        //[Route("/")]
        //public IActionResult Index()
        //{
        //    return Json(new Car { CarType = 1, NumOfKm = 10000, RegNum = "ABC123" });
        //}

        [HttpPost]
        [Route("/returncar")]
        public async Task<IActionResult> ReturnCar([FromBody]ReturnFormSubmitVM json)
        {
            //ReturnFormResponseVM response = service.MakeReturnFormResponseVM(json);

            return Json(await service.MakeReturnFormResponseVM(json));
        }
    }
}
