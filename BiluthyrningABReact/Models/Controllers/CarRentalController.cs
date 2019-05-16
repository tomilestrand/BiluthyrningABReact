using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BiluthyrningABReact.Models
{
    public class CarRentalController : Controller
    {
        ICarRental service;

        public CarRentalController(ICarRental service)
        {
            this.service = service;
        }

        [HttpPost]
        [Route("/retirecar")]
        public async Task<IActionResult> RetireCar([FromBody]RetireCarSubmitVM json)
        {
            return Json(await service.MakeRetireCarResponse(json));
        }

        [HttpPost]
        [Route("/availablecars")]
        public async Task<IActionResult> AvailableCars([FromBody]RentFormQueryVM json)
        {
            return Json(await service.MakeGetAvailableCarsResponse(json));
        }

        [HttpPost]
        [Route("/rentcar")]
        public async Task<IActionResult> RentCar([FromBody]RentFormSubmitVM json)
        {
            return Json(await service.MakeRentFormResponseVM(json));
        }

        [HttpPost]
        [Route("/returncar")]
        public async Task<IActionResult> ReturnCar([FromBody]ReturnFormSubmitVM json)
        {
            return Json(await service.MakeReturnFormResponseVM(json));
        }

        [HttpPost]
        [Route("/addcar")]
        public async Task<IActionResult> AddCar([FromBody]AddCarSubmitVM json)
        {
            return Json(await service.MakeAddCarResponse(json));
        }
    }
}
