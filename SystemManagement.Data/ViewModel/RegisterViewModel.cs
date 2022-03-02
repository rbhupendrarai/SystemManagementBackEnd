using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManagement.Data.ViewModel
{
    public class RegisterViewModel
    {
       
            [Required]
            [Display(Name = "User Name")]
            [StringLength(30)]
            public string UserName { get; set; }
            [Required]

            [StringLength(50)]
            [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9_]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Please enter a valid e-mail adress")]
            //[RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
            public string Email { get; set; }
            [Required]
            [StringLength(15)]
            [RegularExpression("^(?=.*[0-9])" + "(?=.*[a-z])(?=.*[A-Z])"
                                + "(?=.*[@#$%^&+=])" + "(?=\\S+$).{8,15}$"
                                , ErrorMessage = "Minimum 8 characters at least 1 Uppercase Alphabet[A-Z], 1 Lowercase Alphabet[a-z] and 1 Number and 1 Special Character.(ex. A@123.com)")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [StringLength(15)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
            public string ConfirmPassword { get; set; }
        
    }
}
