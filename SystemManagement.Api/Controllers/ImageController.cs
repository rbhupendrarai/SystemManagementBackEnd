using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SystemManagement.Data.Data;
using SystemManagement.Data.Entities;
using SystemManagement.Data.Helper;
using SystemManagement.Data.ViewModel;
using SystemManagement.Service;

namespace SystemManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;
        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetCarModel")]
        public JsonResult GetCarModel()
        {
            var result = _imageService.GetCarList();
            return new JsonResult(result);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetModel/{id}")]
        public JsonResult GetModel(Guid id)
        {
            var result = _imageService.GetModelList(id);
            return new JsonResult(result);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetImage")]
        public async Task<JsonResult> GetImage()
        {
            var result = _imageService.GetImage();
            return new JsonResult(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("AddImage/{id}")]
        public async Task<ActionResult> AddImage(List<IFormFile> image,Guid id)
        {
            try
            {
                if (image.ToList().Count != 0)
                {
                    var result = await _imageService.AddImage(image, id);
                    return Ok();
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
        [Route("EditImage/{id}")]
        public async Task<IActionResult> EditImage(Guid id, List<IFormFile> image)
        {
            try
            {
                if (image.ToList().Count != 0 &&  id != Guid.Empty)
                {
                    var result = await _imageService.EditImage(id, image);
                    if (result == true)
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
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
        [Route("GetImageFilters")]
        public ActionResult GetImageFilters(string search)
        {
            try
            {
                var query = _imageService.GetImage();
                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(c => c.MO_Name.Contains(search) );
                } 
                return Ok(query);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetImageId/{id}")]
        public JsonResult GetImageId(Guid id)
        {
            var result =  _imageService.GetImageById(id);
            return new JsonResult(result);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("RemoveImage/{id}")]
        public ActionResult RemoveImage(Guid id)
        {
            try
            {
                 Images img =  _imageService.GetImageById(id);
                  _imageService.DeleteImage(id);
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "Image Deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

    }
}
