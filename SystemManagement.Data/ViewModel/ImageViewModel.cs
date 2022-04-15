using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManagement.Data.Entities;

namespace SystemManagement.Data.ViewModel
{
    public class ImageViewModel
    {       

        [Display(Name = "Img")]
        public IFormFile Img { get; set; }

        [Display(Name = "Model Id")]
        public Guid MO_Id { get; set; }

    }
}
