using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public interface ICarRental
    {
        Task<ActiveRentsResponseVM> GetAllActiveRents();


        Task<RetireCarResponseVM> MakeRetireCarResponse(RetireCarSubmitVM json);


        Task<ActiveRentsVM[]> GetAllActiveRentsArray();


        Task<AvailableCarsResponseVM> MakeGetAvailableCarsResponse(RentFormQueryVM json);


        Task<CustomersResponseVM> GetAllCustomers();


        Task<CustomerBookingsVM> GetCustomerBookings(string ssn);


        Task<AddCarResponseVM> MakeAddCarResponse(AddCarSubmitVM json);


        Task<CustomerBookingVM[]> GetCustomersRents(string ssn);


        Task<CustomersVM[]> GetCustomersArray();


        Task<RentFormResponseVM> MakeRentFormResponseVM(RentFormSubmitVM json);


        Task<string> AddNewCar(AddCarSubmitVM car);



        Task<CarVM[]> GetAvailableCarsByCarType(RentFormQueryVM json);


        Task<CarVM> GetCarByRegNum(string regNum);


        Task<ReturnFormResponseVM> MakeReturnFormResponseVM(ReturnFormSubmitVM json);

    }
}
