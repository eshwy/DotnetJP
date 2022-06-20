using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortalMVC.Models
{
    public class UserPersonalDetailsTable
    {
        [Key]
        public string Email { get; set; }
        public int? User_Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone_Number { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public string UserAddress { get; set; }
        public string City { get; set; }
        public int Experience { get; set; }
        public string Skills { get; set; }
    }
}
