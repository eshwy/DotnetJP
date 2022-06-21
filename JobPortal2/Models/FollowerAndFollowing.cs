using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobPortal2.Models
{
    public class FollowerAndFollowing
    {
        [Key]
        public int UserId { get; set; }
        public int FollowerId { get; set; }
    }
}
