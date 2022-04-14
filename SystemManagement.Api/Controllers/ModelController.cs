using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public class ModelController : Controller
    {
        private readonly ModelService _modelService;
        public ModelController(ModelService modelService)
        {
            _modelService = modelService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetCarList")]
        public JsonResult GetCarList()
        {
            var result = _modelService.GetCarList();
            return new JsonResult(result);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetModelDetail")]
        public JsonResult GetModelDetail()
        {
            var result = _modelService.GetModel();
            return new JsonResult(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddModel")]
        public async Task<IActionResult> AddModel(Model model)
        {
            try
            {
                var result = await _modelService.AddModel(model);
                if (result == true)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Model Added Successfully" });
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
        [Route("EditModel/{id}")]
        public async Task<IActionResult> EditModel(Guid id, Model model)
        {
            var result = await _modelService.EditModel(id, model);
            if (result == true)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetModelId/{id}")]
        public JsonResult GetModelId(Guid id)
        {
            var result = _modelService.GetModelByID(id);
            return new JsonResult(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("RemoveModel/{id}")]
        public IActionResult RemoveModel(Guid id)
        {
            try
            {
                _modelService.DeleteModel(id);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Model Deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetModelFilters")]
        public ActionResult GetModelFilters(string search)
        {
            try
            {
                var query = _modelService.GetModel();
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.MO_Name.Contains(search) || c.MO_Discription.Contains(search) || c.MO_Feature.Contains(search));
                }
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}