using SystemManagement.Data.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManagement.Data.Data;
using SystemManagement.Data.Entities;

namespace SystemManagement.Service
{
    public class ModelService
    {
        private readonly SystemManagementDbContext _context;
        public ModelService(SystemManagementDbContext carManagementSystemDbContext)
        {

            _context = carManagementSystemDbContext;
        }
        public List<Car> GetCarList()
        {
            return _context.Cars
           .Select(car => new Car()
           {
               CR_Id = car.CR_Id,
               CR_Name = car.CR_Name

           }).ToList();


        }
        public IQueryable<CarModelSubModelDTO> GetModel()
        {
           
            var query=from m in _context.Models
            join c in _context.Cars
            on m.CR_Id equals c.CR_Id
            select new CarModelSubModelDTO()
            {
                MO_Id = m.MO_Id,
                MO_Name = m.MO_Name,
                MO_Discription = m.MO_Discription,
                MO_Feature = m.MO_Feature,
                CR_Name = c.CR_Name
            };
            return query;
        }
        public async Task<bool> AddModel(Model model)
        {
            try
            {
                var modelExists = await _context.Models.FirstOrDefaultAsync(x => x.MO_Name == model.MO_Name);
                if (modelExists != null)
                {
                    return false;
                }
                model.CreatedBy = "Admin";
                model.CreatedDate = DateTime.Now;
                model.ModifiedBy = "Admin";
                model.ModifiedDate = DateTime.Now;
                var result = await _context.Models.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> EditModel(Guid id,Model model)
        {
            try
            {
                var result = await _context.Models.SingleOrDefaultAsync(x => x.MO_Id == id);
                if (result != null)
                {
                    result.MO_Name = model.MO_Name;
                    result.MO_Discription = model.MO_Discription;
                    result.MO_Feature = model.MO_Feature;
                    result.CR_Id = model.CR_Id;
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
        public Model GetModelByID(Guid id)
        {
            return _context.Models.Find(id);
        }
        public void DeleteModel(Guid id)
        {
            Model model = _context.Models.Find(id);
            _context.Models.Remove(model);
            _context.SaveChanges();
        }
    }
}
