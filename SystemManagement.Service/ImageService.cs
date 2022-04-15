using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManagement.Data.Data;
using SystemManagement.Data.DTO;
using SystemManagement.Data.Entities;
using SystemManagement.Data.ViewModel;

namespace SystemManagement.Service
{
    public class ImageService
    {
        private readonly SystemManagementDbContext _context;
        public ImageService(SystemManagementDbContext context)
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
        public IQueryable<CarModelSubModelDTO> GetModelList(Guid id)
        {
            return from car in _context.Cars
                   join model in _context.Models
                   on car.CR_Id equals model.CR_Id
                   where car.CR_Id == id
                   select new CarModelSubModelDTO()
                   {
                       MO_Id = model.MO_Id,
                       MO_Name = model.MO_Name
                   };
        }
        public IQueryable<CarModelSubModelDTO> GetImage()
        {
            return from m in _context.Images // outer sequence
                   join c in _context.Models //inner sequence 
                   on m.MO_Id equals c.MO_Id // key selector 
                   select new CarModelSubModelDTO()
                   { // result selector 
                       Img_Id=m.Img_Id,
                       Img=m.Img,
                       MO_Name = c.MO_Name,
                       MO_Id=c.MO_Id
                   };
        }
        public async Task<bool> AddImage(List<IFormFile> fileupload,Guid id)
        {
            try
            {
                Images img = new Images();
                foreach (var file in fileupload)
                {
                    MemoryStream ms = new MemoryStream();
                    file.CopyTo(ms);
                    img.Img = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }              
                img.MO_Id = id;
                img.CreatedDate = DateTime.Now;
                img.CreatedBy = "Admin";
                img.ModifiedDate = DateTime.Now;
                img.ModifiedBy = "Admin";
                _context.Images.Add(img);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<bool> EditImage(Guid id,Images img, IFormFile[] fileupload)
        {
            try
            {
                var result = await _context.Images.SingleOrDefaultAsync(x => x.Img_Id == id);
                if (result != null)
                {
                    foreach (IFormFile file in fileupload)
                    {
                        MemoryStream ms = new MemoryStream();
                        file.CopyTo(ms);
                        img.Img = ms.ToArray();
                        ms.Close();
                        ms.Dispose();
                    }
                    result.ModifiedBy = "Admin";
                    result.ModifiedDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public Images GetImageById(Guid id)
        {
            return _context.Images.Find(id);
        }
        public void DeleteImage(Guid id)
        {
            Images img = _context.Images.Find(id);
            _context.Images.Remove(img);
            _context.SaveChanges();
        }

    }

}
