using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarManagementSystem.Data.ViewModel
{
    public class EditDetailViewModel
    {

        [StringLength(30)]
        public string UserName { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9_]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }

        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }


        [StringLength(15)]       
        [DataType(DataType.Password)]       
        public string NewPassword { get; set; }

      
    }
}
