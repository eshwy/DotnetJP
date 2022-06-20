using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal2.Models
{
    public class SoretedProfiles
    {
        [Key]
        public int? RowNumber { get; set; }
        public int SortedBy { get; set; }
        public int SeletedId { get; set; }
    }
}
