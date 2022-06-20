using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortalMVC.Models
{
    public class UserFollowers
    {
        [Key]
        public int? RowNumber { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int FollowerId { get; set; }
        public string FollowerName { get; set; }

    }
}
