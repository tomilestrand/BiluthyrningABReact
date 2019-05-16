using BiluthyrningABReact.Models;
using System;
using AspNetCore.Mvc;
using Xunit;

namespace BiluthyrningABReactUnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }

        [Fact]
        public void CarRentalController()
        {
            var controller = new CarRentalController(new CarRentalRepository(new DatabaseRepository()));

            System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> response = controller.AddCar(new AddCarSubmitVM { CarType = 1, NumOfKm = 1234, RegNum = "AAA111");
        }
    }
}
