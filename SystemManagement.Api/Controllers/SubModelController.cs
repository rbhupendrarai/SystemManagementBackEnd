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
    public class SubModelController : Controller
    {
        private readonly SubModelService _subModelService;
        public SubModelController(SubModelService subModelService)
        {
            _subModelService = subModelService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetCarModel")]
        public JsonResult GetCarModel()
        {
            var result = _subModelService.GetCarList();
            return new JsonResult(result);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetModel/{id}")]
        public JsonResult GetModel(Guid id)
        {
            var result = _subModelService.GetModelList(id);
            return new JsonResult(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetSubModelDetail")]
        public JsonResult GetSubModelDetail()
        {
            var result = _subModelService.GetSubModel();
            return new JsonResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddSubModel")]
        public async Task<IActionResult> AddSubModel(SubModel subModel)
        {
            try
            {
                var result = await _subModelService.AddSubmodel(subModel);
                if (result == true)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Sub Model Added Successfully" });
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
        [Route("EditSubModel/{id}")]
        public async Task<IActionResult> EditSubModel(Guid id, SubModel subModel)
        {
            try
            {
                var result = await _subModelService.EditSubModel(id, subModel);
                if (result == true)
                {
                    return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Sub Model Updated Successfully" });
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
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetSMId/{id}")]
        public JsonResult GetSMId(Guid id)
        {
            var result = _subModelService.GetSubModelByID(id);
            return new JsonResult(result);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetSMFilters")]
        public ActionResult GetSMFilters(string search)
        {
            try
            {
                var query = _subModelService.GetSubModel();
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.SM_Name.Contains(search) || c.SM_Discription.Contains(search) || c.SM_Feature.Contains(search));
                }
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("RemoveSubModel/{id}")]
        public IActionResult RemoveSubModel(Guid id)
        {
            try
            {
                _subModelService.DeleteModel(id);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "SM Model Deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        [Route("GetProcedure")]
        public async Task<IActionResult> GetProcedure(string sort, int page_size, int page_limit)
        {
            if(page_size == 0)
            {
                page_size = 1;
            }
            if (page_limit== 0)
            {
                page_limit= 10;
            }
            if (sort == null)
            {
                sort = "asc";
            }
            
            var result =  _subModelService.GetFilters(sort, page_size, page_limit);
            return Ok(result);
        }
    }
}

