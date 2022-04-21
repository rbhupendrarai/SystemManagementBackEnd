using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManagement.Data.Data;
using SystemManagement.Data.DTO;
using SystemManagement.Data.Entities;
using SystemManagement.Data.Procedure;

namespace SystemManagement.Service
{
    public class SubModelService
    {
        private readonly SystemManagementDbContext _context;
        SubModelFiltersInput SubModelFiltersInput = new SubModelFiltersInput();
        public SubModelService(SystemManagementDbContext context)
        {
            _context = context;
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
        public  IQueryable<CarModelSubModelDTO> GetModelList(Guid id)
        {
            return from car in _context.Cars
                   join model in _context.Models
                   on car.CR_Id equals model.CR_Id
                   where car.CR_Id == id || car.CR_Id == Guid.Empty
                   select new CarModelSubModelDTO()
                   {
                       MO_Id=model.MO_Id,
                       MO_Name=model.MO_Name
                   };
        }
        public IQueryable<CarModelSubModelDTO> GetSubModel()
        {
            return from m in _context.SubModels // outer sequence
                   join c in _context.Models //inner sequence 
                   on m.MO_Id equals c.MO_Id // key selector 
                   select new CarModelSubModelDTO()
                   { // result selector 
                       SM_Id = m.SM_Id,
                       SM_Name = m.SM_Name,
                       SM_Discription = m.SM_Discription,
                       SM_Feature = m.SM_Feature,
                       SM_Price = m.SM_Price,
                       MO_Name = c.MO_Name
                   };
        }
        public async Task<bool> AddSubmodel(SubModel subModel)
        {
            try
            {
                var modelExists = await _context.SubModels.FirstOrDefaultAsync(x => x.SM_Name == subModel.SM_Name);
                if (modelExists != null)
                {
                    return false;
                }
                subModel.CreatedBy = "Admin";
                subModel.CreatedDate = DateTime.Now;
                subModel.ModifiedBy = "Admin";
                subModel.ModifiedDate = DateTime.Now;
                var result = await _context.SubModels.AddAsync(subModel);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> EditSubModel(Guid id,SubModel subModel)
        {
            try
            {
                var result = _context.SubModels.SingleOrDefault(s => s.SM_Id == id);
                if (result != null)
                {
                    result.SM_Name = subModel.SM_Name;
                    result.SM_Discription = subModel.SM_Discription;
                    result.SM_Feature = subModel.SM_Feature;
                    result.SM_Price = subModel.SM_Price;
                    result.MO_Id = subModel.MO_Id;
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
        public SubModel GetSubModelByID(Guid id)
        {
            return _context.SubModels.Find(id);
        }
        public void DeleteModel(Guid id)
        {
            SubModel subModel = _context.SubModels.Find(id);
            _context.SubModels.Remove(subModel);
            _context.SaveChanges();
        }

        public async Task<List<SubModelFiltersInput>> GetFilters(string sort, int page, int page_limit, string search, int a)
        {
            var parameterReturn = new SqlParameter
            {
                ParameterName = "ReturnValue",
                SqlDbType = System.Data.SqlDbType.Int,
                Direction = System.Data.ParameterDirection.Output,
            };
            var result = await _context.FiltersInputs.FromSqlRaw("EXEC spSModelFilter {0},{1},{2},{3},{4}", sort, page, page_limit, search, a).ToListAsync();
            var result1 = _context.FiltersInputs.FromSqlRaw("EXEC  @Total =spSModelFilter", parameterReturn);
            int returnValue = (int)parameterReturn.Value;
            Console.WriteLine("",+returnValue);
            return result;
        }

    }
}
