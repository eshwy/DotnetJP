using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortalMVC.Models
{
    public class ArticleView
    {
        [Key]
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }        
        public string Gender { get; set; }        
        public int Experience { get; set; }
        public string Skills { get; set; }

    }
}
