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
        [Route("/customers")]
        public IActionResult Customers()
        {
            return Json(service.GetAllCustomers());
        }

        [HttpPost]
        [Route("/rentcar")]
        public IActionResult RentCar([FromBody]RentFormSubmitVM json)
        {
            return Json(service.MakeRentFormResponseVM(json));
        }

        [Route("/rentcar")]
        [HttpGet]
        public IActionResult RentCar()
        {
            return Ok();
        }

        //[Route("/")]
        //public IActionResult Index()
        //{
        //    return Json(new Car { CarType = 1, NumOfKm = 10000, RegNum = "ABC123" });
        //}

        [HttpPost]
        [Route("/returncar")]
        public IActionResult ReturnCar([FromBody]ReturnFormSubmitVM json)
        {
            ReturnFormResponseVM response = service.MakeReturnFormResponseVM(json);

            return Json(response);
        }
    }
}
