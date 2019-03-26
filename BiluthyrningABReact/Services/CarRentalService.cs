using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiluthyrningABReact.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace BiluthyrningABReact.Services
{
    public class CarRentalService
    {
        private const decimal baseDayRental = 500;
        private const decimal kmPrice = 20;
        private static decimal CarCost(long numberOfDays, int numberOfKm, CarType typeCar)
        {
            return baseDayRental * numberOfDays * CarFactor() + KmPrice();

            decimal CarFactor()
            {
                switch (typeCar)
                {
                    case CarType.Smallcar:
                        return 1M;
                    case CarType.Van:
                        return 1.2M;
                    case CarType.Minibus:
                        return 1.7M;
                    default:
                        throw new Exception("That type of car is not defined");
                }
            }

            decimal KmPrice()
            {
                switch (typeCar)
                {
                    case CarType.Smallcar:
                        return 0;
                    case CarType.Van:
                        return kmPrice * numberOfKm;
                    case CarType.Minibus:
                        return kmPrice * numberOfKm * 1.5M;
                    default:
                        throw new Exception("That type of car is not defined");
                }
            }
        }

        private static bool ValidSSN(string input)
        {
            return input.Length == 13 && input[8] == '-' && input.Substring(0, 8).All(char.IsDigit) && input.Substring(9, 4).All(char.IsDigit);
        }

        internal async Task<ActiveRentsResponseVM> GetAllActiveRents()
        {
            return new ActiveRentsResponseVM { Status = "OK", ActiveRents = await GetAllActiveRentsArray() };
        }

        internal async Task<RetireCarResponseVM> MakeRetireCarResponse(RetireCarSubmitVM json)
        {
            try
            {
                var collection = DatabaseService.GetCollectionFromDb<BsonDocument>("Car");
                //BsonSerializer.Deserialize<CarDM>(collection.ToBsonDocument());
                var filter = Builders<BsonDocument>.Filter.Eq("RegNum", json.RegNum) & Builders<BsonDocument>.Filter.Eq("CarType", json.CarType);
                var update = Builders<BsonDocument>.Update.Set("Retired", true);
                await DatabaseService.UpdateDb(filter, update, collection);
                return new RetireCarResponseVM { Status = "OK" };
            }
            catch (Exception e)
            {
                var error = e;
                return new RetireCarResponseVM { Status = "Failed" };
            }
        }

        private async Task<ActiveRentsVM[]> GetAllActiveRentsArray()
        {
            var response = new List<ActiveRentsVM>();

            var collection = DatabaseService.GetCollectionFromDb<BsonDocument>("CarBooking");
            var filter = Builders<BsonDocument>.Filter.Not(Builders<BsonDocument>.Filter.Exists("ReturnDate"));
            var bookings = await collection.Find(filter).ToListAsync();

            return response.ToArray();
        }

        internal async Task<AvailableCarsResponse> MakeGetAvailableCarsResponse(RentFormQueryVM json)
        {
            var cars = await GetAvailableCarsByCarType(json);
            if (cars.Length > 0)
            {
                return new AvailableCarsResponse { Cars = cars, Status = "OK" };
            }
            else
            {
                return new AvailableCarsResponse { Status = "Failure" };
            }
        }

        internal async Task<CustomersResponseVM> GetAllCustomers()
        {
            try
            {
                return new CustomersResponseVM { Status = "OK", Customers = await GetCustomersArray() };
            }
            catch (Exception e)
            {
                var error = e;
                return new CustomersResponseVM { Status = "Failed" };
            }
        }

        internal async Task<CustomerBookingsVM> GetCustomerBookings(string ssn)
        {
            return new CustomerBookingsVM { Status = "OK", CustomerBookings = await GetCustomersRents(ssn) };
        }

        internal async Task<AddCarResponseVM> MakeAddCarResponse(AddCarSubmitVM json)
        {
            return new AddCarResponseVM { Status = await AddNewCar(json) };
        }

        internal async Task<CustomerBookingVM[]> GetCustomersRents(string ssn)
        {
            var response = new List<CustomerBookingVM>();
            var collection = DatabaseService.GetCollectionFromDb<BsonDocument>("CarBooking");
            var filter = Builders<BsonDocument>.Filter.Eq("CustomerId", ssn);
            var bookings = await collection.Find(filter).ToListAsync();
            foreach (var item in bookings)
            {
                try
                {
                    response.Add(new CustomerBookingVM
                    {
                        CarType = item["CarType"].ToInt32(),
                        RegNum = item["CarRegistrationNumber"].ToString(),
                        StartTime = item["StartTime"].ToLocalTime(),
                        CarbookingId = item["BookingId"].ToString(),
                        EndTime = !item["ReturnDate"].IsBsonNull ? item["ReturnDate"].ToLocalTime() : default,
                        MilesDriven = !item["NewMilage"].IsBsonNull ? item["NewMilage"].ToInt32() - item["NumberOfKmStart"].ToInt32() : 0
                    });
                }
                catch (Exception e)
                {
                    var error = e;
                }
            }
            return response.ToArray();
        }

        private async Task<CustomersVM[]> GetCustomersArray()
        {
            var response = new List<CustomersVM>();
            var collection = DatabaseService.GetCollectionFromDb<BsonDocument>("Customers");
            var documents = await collection.Find(new BsonDocument()).ToListAsync();
            foreach (var item in documents)
            {
                response.Add(new CustomersVM
                {
                    FirstName = item["FirstName"].ToString(),
                    LastName = item["LastName"].ToString(),
                    SSN = item["SSN"].ToString()
                });
            }
            return response.ToArray();
        }

        internal async Task<RentFormResponseVM> MakeRentFormResponseVM(RentFormSubmitVM json)
        {
            CarVM car = await GetCarByRegNum(json.RegNum);
            if (car == null)
                return new RentFormResponseVM { Status = "No car of that type available" };
            if (!ValidSSN(json.SSN))
                return new RentFormResponseVM { Status = "Invalid SSN" };
            try
            {
                var collection = DatabaseService.GetCollectionFromDb<BsonDocument>("CarBooking");
                string bookingId = Guid.NewGuid().ToString().Substring(0, 8);
                var document = new BsonDocument
                {
                    { "CarType", car.CarType },
                    { "CarRegistrationNumber", car.RegNum },
                    { "NumberOfKmStart", car.NumOfKm },
                    { "StartTime", DateTime.Now.Date },
                    { "CustomerId", json.SSN },
                    { "BookingId", bookingId },
                    {"EndTime",BsonValue.Create(null) },
                    {"NewMilage",BsonValue.Create(null) }
                };
                await DatabaseService.InsertIntoDb<BsonDocument>(document, collection);
                return new RentFormResponseVM { CarbookingId = bookingId, CarType = car.CarType, RegNum = car.RegNum, Status = "OK" };
            }
            catch (Exception e)
            {
                var error = e;
                return new RentFormResponseVM { Status = "Transaction failed" };
            }
        }

        private async Task<string> AddNewCar(AddCarSubmitVM car)
        {
            try
            {
                var collection = DatabaseService.GetCollectionFromDb<BsonDocument>("Car");
                var document = new BsonDocument
            {
                {"CarType", car.CarType },
                {"RegNum", car.RegNum },
                {"NumOfKm", car.NumOfKm },
                {"Retired", false },
                {"DateAdded", DateTime.Now }
            };
                await DatabaseService.InsertIntoDb<BsonDocument>(document, collection);

                return "OK";
            }
            catch (Exception e)
            {
                var error = e;
                return "Failed";
            }
        }

        internal async Task<CarVM[]> GetAvailableCarsByCarType(RentFormQueryVM json)
        {
            int carType = json.CarType;
            var collection = DatabaseService.GetCollectionFromDb<BsonDocument>("Car");
            var filter = Builders<BsonDocument>.Filter.Eq("CarType", carType) & Builders<BsonDocument>.Filter.Eq("Retired", false);
            var cars = await collection.Find(filter).ToListAsync();

            collection = DatabaseService.GetCollectionFromDb<BsonDocument>("CarBooking");
            filter = Builders<BsonDocument>.Filter.Eq("CarType", carType) & Builders<BsonDocument>.Filter.Eq("EndTime", BsonValue.Create(null));
            var bookings = await collection.Find(filter).Project(q => q["CarRegistrationNumber"]).ToListAsync();
            cars.RemoveAll(car => bookings.Contains(car["RegNum"]));

            return cars.Select(car => new CarVM { CarType = car["CarType"].ToInt32(), NumOfKm = car["NumOfKm"].ToInt32(), RegNum = car["RegNum"].ToString() }).ToArray();
        }

        private async Task<CarVM> GetCarByRegNum(string regNum)
        {
            var collection = DatabaseService.GetCollectionFromDb<BsonDocument>("Car");
            var filter = Builders<BsonDocument>.Filter.Eq("RegNum", regNum) & Builders<BsonDocument>.Filter.Eq("Retired", false);
            var car = await collection.Find(filter).FirstOrDefaultAsync();
            return new CarVM { CarType = car["CarType"].ToInt32(), NumOfKm = car["NumOfKm"].ToInt32(), RegNum = car["RegNum"].ToString() };
        }

        internal async Task<ReturnFormResponseVM> MakeReturnFormResponseVM(ReturnFormSubmitVM json)
        {
            try
            {
                var collection = DatabaseService.GetCollectionFromDb<BsonDocument>("CarBooking");
                var filter = Builders<BsonDocument>.Filter.Eq("BookingId", json.CarbookingId);
                var booking = await collection.Find(filter).FirstOrDefaultAsync();
                DateTime startDate = booking["StartTime"].ToLocalTime();
                DateTime returnDate = DateTime.Now.Date;

                if (DateTime.Compare(startDate, returnDate) > 0)
                    return new ReturnFormResponseVM { Status = "Invalid return date" };

                int initalKm = booking["NumberOfKmStart"].ToInt32();

                if (initalKm > json.NewMilage)
                    return new ReturnFormResponseVM { Status = "Invalid return milage" };

                var update = Builders<BsonDocument>.Update.Set("ReturnDate", returnDate).Set("NewMilage", json.NewMilage);
                await DatabaseService.UpdateDb(filter, update, collection);

                string regNum = booking["CarRegistrationNumber"].ToString();
                collection = DatabaseService.GetCollectionFromDb<BsonDocument>("Car");
                filter = Builders<BsonDocument>.Filter.Eq("RegNum", regNum);
                update = Builders<BsonDocument>.Update.Set("NumOfKm", json.NewMilage);
                await DatabaseService.UpdateDb(filter, update, collection);

                long numberOfDays = (returnDate.Ticks - startDate.Ticks) / TimeSpan.TicksPerDay;
                int numberOfKm = json.NewMilage - initalKm;
                return new ReturnFormResponseVM { TotalPrice = CarCost(numberOfDays, json.NewMilage - initalKm, (CarType)booking["CarType"].ToInt32()).ToString("0.00"), Status = "OK" };
            }
            catch (Exception e)
            {
                var error = e;
                return new ReturnFormResponseVM { Status = "Failed" };
            }
        }
    }
}
