using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class CarController : Controller
    {
        private readonly CarService _carService;
        public CarController(CarService carService)
        {
            _carService = carService;
        }
      
        [HttpGet("{id}")]  
        public ActionResult<Car> GetCar(Guid id)
        {
            return _carService.GetCarByID(id);         

        }
    }
}
