using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManagement.Data.Data;
using SystemManagement.Data.DTO;
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
        public List<Car> GetCar()
        {
            return _context.Cars.ToList();
        }
        public Car GetCarByID(Guid id)
        {
            return _context.Cars.Find(id);
        }
        public  IQueryable<Car> GetCarDetail()
        {
            return  _context.Cars.Select(car => new Car()
            {
                CR_Id = car.CR_Id,
                CR_Name = car.CR_Name,
                CR_Discription = car.CR_Discription,
            });
        }
        public IQueryable<CarModelSubModelDTO> GetAllModel()
        {
             return from car in _context.Cars 
                    join model in _context.Models
                    on car.CR_Id equals model.CR_Id
                    join submodel in _context.SubModels on model.MO_Id equals submodel.MO_Id
                    join images in _context.Images on model.MO_Id equals images.MO_Id
                    select new CarModelSubModelDTO()
                    { // result selector 

                        CR_Id=car.CR_Id,
                        CR_Name=car.CR_Name,
                        CR_Discription=car.CR_Discription,
                        MO_Id=model.MO_Id,
                        MO_Name=model.MO_Name,
                        MO_Discription=model.MO_Discription,
                        MO_Feature=model.MO_Feature,
                        SM_Id=submodel.SM_Id,
                        SM_Name=submodel.SM_Name,
                        SM_Feature=submodel.SM_Feature,
                        SM_Discription=submodel.SM_Discription,
                        SM_Price=submodel.SM_Price,
                        Img_Id=images.Img_Id,
                        Img=images.Img                   
                    };
        }
        public async Task<bool> AddCar(Car car)
        {
            try
            {
                var carExists = await _context.Cars.FirstOrDefaultAsync(x => x.CR_Name == car.CR_Name);
                if (carExists != null)
                {
                    return false;
                }
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
        public async Task<bool> EditCar(Guid id,Car cars)
        {
            try
            {
                var result = await _context.Cars.SingleOrDefaultAsync(x => x.CR_Id == id);
                if (result != null)
                {
                    result.CR_Name = cars.CR_Name;
                    result.CR_Discription = cars.CR_Discription;
                    result.ModifiedBy = "Admin";
                    result.ModifiedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public void DeleteCar(Guid id)
        {
            Car car = _context.Cars.Find(id);
           _context.Cars.Remove(car);
           _context.SaveChanges();
   
        }
    }
}
