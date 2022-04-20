using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManagement.Data.Procedure
{
    [Keyless]

    public class SubModelFiltersInput
    {            
        public Guid SM_Id { get; set; }    
        public string SM_Name { get; set; }              
        public string SM_Discription { get; set; }
        public string SM_Feature { get; set; }
        public decimal SM_Price { get; set; }
        public Guid CR_Id { get; set; }
        public string CR_Name { get; set; }   
        public Guid MO_Id { get; set; }
        public string MO_Name { get; set; }

    }
}
