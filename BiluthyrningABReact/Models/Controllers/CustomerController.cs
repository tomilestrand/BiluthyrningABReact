using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BiluthyrningABReact.Models
{
    public class CustomerController : Controller
    {
        ICarRental service;

        public CustomerController(ICarRental service)
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
