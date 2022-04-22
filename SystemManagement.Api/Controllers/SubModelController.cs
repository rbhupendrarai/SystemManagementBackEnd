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
        [Route("GetModel")]
        public JsonResult GetModel(Guid id)
        {
            var result = _subModelService.GetModelList(id);
            return new JsonResult(result);
        }

        [HttpGet]

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
        public async Task<IActionResult> GetProcedure(string sort, int page, int page_limit, string search,int total)
        {
            page = page <= 0 ? 1 : page;
            page_limit = page_limit <= 0 ? 3 : page_limit;
            sort = !string.IsNullOrEmpty(sort) ? string.IsNullOrEmpty(sort.Trim()) ? "ASC" :sort : "ASC";
            total = total <= 0 ? 1 : total;
            var result = await _subModelService.GetFilters(sort, page, page_limit, search, total);         
            var jsonData = new {  data = result };
            return Ok(jsonData);
        }
    }
}

