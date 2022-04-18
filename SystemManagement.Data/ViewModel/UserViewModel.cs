using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemManagement.Data.ViewModel
{
    public class UserViewModel
    {

        public string Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public bool LockoutEnabled { get; set; }

        public string Phone { get; set; }
        public string Role { get; set; }

     
    }
}
