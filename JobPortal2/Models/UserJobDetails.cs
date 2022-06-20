using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal2.Models
{
    public class UserJobDetails
    {
        [Key]
        public int rowId { get; set; }
        public int User_Id { get; set; }
        public string CompanyName { get; set; }
        public DateTime WorkedFrom { get; set; }
        public DateTime? WorkedTill { get; set; }
        public string LocationWorked { get; set; }

        
    }
}
