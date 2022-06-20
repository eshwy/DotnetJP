using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JobPortal2.Models
{
    public class Article
    {
        [Key]
        public int? RowNumber { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}
