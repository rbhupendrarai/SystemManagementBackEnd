using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemManagement.Data.Entities;
using SystemManagement.Data.Helper;
using SystemManagement.Service;

namespace SystemManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : Controller
    {
        private readonly CarService _carService;
        public CarController(CarService carService)
        {
            _carService = carService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetCar")]
        public JsonResult GetCar()
        {
            var result = _carService.GetCar();
            return new JsonResult(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("GetCarById/{id}")]
        public JsonResult GetCarById(Guid id)
        {
            var result = _carService.GetCarByID(id);
            return new JsonResult(result);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("RemoveCar/{id}")]
        public  ActionResult  RemoveCar(Guid id)
        {
             _carService.DeleteCar(id);
             return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Car Deleted" });

        }
        [HttpGet]
        [Route("GetCarDetail")]
        public ActionResult GetCarDetail(string sort_by, string sort_type, string search, int page, int page_size)
        {
            try 
            {
                var query = _carService.GetCarDetail();
                var pageSize = query.Count();
                switch (sort_by)
                {
                    case "CR_Name":
                        if (sort_type == "asc")
                        {
                            query = query.OrderBy(c => c.CR_Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(c => c.CR_Name);
                        }

                        break;
                    case "CR_Discription":
                        if (sort_type == "asc")
                        {
                            query = query.OrderBy(c => c.CR_Discription);
                        }
                        else
                        {
                            query = query.OrderByDescending(c => c.CR_Discription);
                        }
                        break;
                }
                //your search
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.CR_Name.Contains(search));
                }
                var data = query.Skip((page - 1) * page_size).Take(pageSize).ToList();
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [Route("AddCar")]
        public async Task<IActionResult> AddCar(Car car)
        {
            try
            {
                var result = await _carService.AddCar(car);
                if (result == true)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("EditCar/{id}")]
        public async Task<IActionResult> EditCar(Guid id,Car car)
        {
            try
            {   
                var result = await _carService.EditCar(id,car);
                if (result == true)
                {                        
                   return Ok(result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }             
          
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
         
        }
    }
}
