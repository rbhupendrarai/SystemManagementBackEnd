
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemManagement.Data.Entities;

namespace SystemManagement.Data.DTO
{
    public class CarModelSubModelDTO
    {
    
        public Car car { get; set; }
        public Guid CR_Id { get; set; }
        public string CR_Name { get; set; }
        public string CR_Discription{ get; set; }
        public Images image { get; set; }
        public Guid Img_Id { get; set; }
        public byte[] Img { get; set; }

        public Model model { get; set; }    
        public Guid MO_Id { get; set; }
        public string MO_Name { get; set; }
        public string MO_Discription { get; set; }
        public string MO_Feature { get; set; }

        public SubModel subModel { get; set; }
        public Guid SM_Id { get; set; }
        public string SM_Name { get; set; }
        public string SM_Discription { get; set; }
        public string SM_Feature { get; set; }
        public decimal SM_Price { get; set; }

    }
}
