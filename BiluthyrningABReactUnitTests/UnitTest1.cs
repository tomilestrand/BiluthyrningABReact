using BiluthyrningABReact.Models;
using System;
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
        }
    }
}
