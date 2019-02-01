using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiluthyrningABReact.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BiluthyrningABReact.Services
{
    public class CarRentalService
    {
        private const decimal baseDayRental = 500;
        private const decimal kmPrice = 20;

        private const string connString = "mongodb://localhost:27017";
        private const string database = "BiluthyrningAB";

        static readonly List<Car> cars = new List<Car>
            {
            new Car{CarType = 1, NumOfKm = 12000, RegNum = "LTN123"},
            new Car{CarType = 2, NumOfKm = 5500, RegNum = "VAN123"},
            new Car{CarType = 3, NumOfKm = 4000, RegNum = "MBS123"},
            };

        internal async Task<ActiveRentsResponseVM> GetAllActiveRents()
        {
            return new ActiveRentsResponseVM { Status = "OK", ActiveRents = await GetAllActiveRentsArray() };
        }

        private async Task<ActiveRentsVM[]> GetAllActiveRentsArray()
        {
            var response = new List<ActiveRentsVM>();

            var collection = GetCollectionFromDb<BsonDocument>("CarBooking");
            var filter = Builders<BsonDocument>.Filter.Exists("ReturnDate");
            var filter2 = Builders<BsonDocument>.Filter.Not(filter);
            var bookings = await collection.Find(filter2).ToListAsync();

            return response.ToArray();
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

        internal async Task<CustomerBookingVM[]> GetCustomersRents(string ssn)
        {
            var response = new List<CustomerBookingVM>();
            var collection = GetCollectionFromDb<BsonDocument>("CarBooking");
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
                        EndTime = !item["ReturnDate"].IsBsonNull ? item["ReturnDate"].ToLocalTime() : default,
                        MilesDriven = !item["NewMilage"].IsBsonNull ? item["NewMilage"].ToInt32() - item["NumberOfKmStart"].ToInt32() : 0
                    });
                }
                catch (Exception e)
                {
                    var error = e;
                    response.Add(new CustomerBookingVM
                    {
                        CarType = item["CarType"].ToInt32(),
                        RegNum = item["CarRegistrationNumber"].ToString(),
                        StartTime = item["StartTime"].ToLocalTime()
                    });
                }
            }
            return response.ToArray();
        }

        private IMongoCollection<T> GetCollectionFromDb<T>(string collection)
        {
            var client = new MongoClient(connString);
            var dataBase = client.GetDatabase(database);
            return dataBase.GetCollection<T>(collection);
        }

        private async Task<bool> InsertIntoDb<T>(T row, IMongoCollection<T> collection)
        {
            try
            {
                await collection.InsertOneAsync(row);
                return true;
            }
            catch (Exception e)
            {
                var error = e;
                return false;
            }
        }



        private async Task<CustomersVM[]> GetCustomersArray()
        {
            var response = new List<CustomersVM>();
            var collection = GetCollectionFromDb<BsonDocument>("Customers");
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
            try
            {
                var collection = GetCollectionFromDb<BsonDocument>("CarBooking");
                var car = cars.SingleOrDefault(c => c.CarType == json.CarType);
                string bookingId = Guid.NewGuid().ToString().Substring(0, 8);
                var document = new BsonDocument
                {
                    { "CarType", car.CarType },
                    { "CarRegistrationNumber", car.RegNum },
                    { "NumberOfKmStart", car.NumOfKm },
                    { "StartTime", DateTime.Now.Date },
                    { "CustomerId", json.SSN },
                    { "BookingId", bookingId }
                };
                await InsertIntoDb<BsonDocument>(document, collection);
                collection = GetCollectionFromDb<BsonDocument>("CarBooking");
                return new RentFormResponseVM { CarbookingId = bookingId, CarType = car.CarType, RegNum = car.RegNum, Status = "OK" };
            }
            catch (Exception e)
            {
                var error = e;
                return new RentFormResponseVM { Status = "Failed" };
            }
        }

        internal async Task<ReturnFormResponseVM> MakeReturnFormResponseVM(ReturnFormSubmitVM json)
        {
            try
            {
                DateTime startDate;
                DateTime returnDate = DateTime.Now.Date;
                var collection = GetCollectionFromDb<BsonDocument>("CarBooking");

                var filter = Builders<BsonDocument>.Filter.Eq("BookingId", json.CarbookingId);
                var update = Builders<BsonDocument>.Update.Set("ReturnDate", returnDate);
                await UpdateDb(filter, update, collection);

                update = Builders<BsonDocument>.Update.Set("NewMilage", json.NewMilage);
                await UpdateDb(filter, update, collection);

                var booking = await collection.Find(filter).FirstOrDefaultAsync();
                startDate = booking["StartTime"].ToLocalTime();
                long numberOfDays = (startDate.Ticks - returnDate.Ticks) / TimeSpan.TicksPerDay;
                int initalKm = booking["NumberOfKmStart"].ToInt32();
                int numberOfKm = json.NewMilage - initalKm;
                return new ReturnFormResponseVM { TotalPrice = CarCost(numberOfDays, initalKm, booking["CarType"].ToInt32()).ToString("0.00"), Status = "OK" };
            }
            catch (Exception e)
            {
                var error = e;
                return new ReturnFormResponseVM { Status = "Failed" };
            }
        }

        private async Task<bool> UpdateDb<T>(FilterDefinition<T> filter, UpdateDefinition<T> update, IMongoCollection<T> collection)
        {
            try
            {
                var result = await collection.UpdateOneAsync(filter, update);
                return result.IsModifiedCountAvailable;
            }
            catch (Exception e)
            {
                var error = e;
                return false;
            }
        }

        private static decimal CarCost(long numberOfDays, int numberOfKm, int typeCar)
        {
            return baseDayRental * numberOfDays * CarFactor() + KmPrice();

            decimal CarFactor()
            {
                switch (typeCar)
                {
                    case 1:
                        return 1M;
                    case 2:
                        return 1.2M;
                    case 3:
                        return 1.7M;
                    default:
                        return -1M;
                }
            }

            decimal KmPrice()
            {
                switch (typeCar)
                {
                    case 1:
                        return 0;
                    case 2:
                        return kmPrice * numberOfKm;
                    case 3:
                        return kmPrice * numberOfKm * 1.5M;
                    default:
                        return -1;
                }
            }
        }
    }
}
