using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManagement.Data.Data;
using SystemManagement.Data.Entities;

namespace SystemManagement.Service
{
    public class CarService
    {
        private readonly SystemManagementDbContext _context;
        public CarService(SystemManagementDbContext systemManagementSystemDbContext)
        {
            _context = systemManagementSystemDbContext;
        }
        
        public List<Car> GetCarByID()
        {
            return _context.Cars.ToList();
        }

        public async Task<bool> AddCar([FromQuery]Car car)
        {
            try
            {
               
                  
                    car.CreatedBy = "Admin";
                    car.CreatedDate = DateTime.Now;
                    car.ModifiedBy = "Admin";
                    car.ModifiedDate = DateTime.Now;
                    await _context.Cars.AddAsync(car);
                    await _context.SaveChangesAsync();
                
                   

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
