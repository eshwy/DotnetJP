using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortalMVC.Models
{
    public class ParticularDetails
    {
        [Key]
        public int? User_Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone_Number { get; set; }
        public string ImagePath { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime Date_Of_Birth { get; set; }
        public string UserAddress { get; set; }
        public string City { get; set; }
        public int Experience { get; set; }
        public string Skills { get; set; }
        public string PrimaryEducation { get; set; }
        public string PercentPrimaryEducation { get; set; }
        public DateTime StartPrimaryEducationYear { get; set; }
        public DateTime EndPrimaryEducationYear { get; set; }
        public string SecodaryEducation { get; set; }
        public string PercentSecodaryEducation { get; set; }
        public DateTime StartSecodaryEducationYear { get; set; }
        public DateTime EndSecodaryEducationYear { get; set; }
        public string SecodaryEducationBranch { get; set; }
        public string UnderGraduationEducation { get; set; }
        public string PercentageUnderGraduationEducation { get; set; }
        public DateTime? StartUnderGraduationYear { get; set; }
        public DateTime? EndUnderGraduationYear { get; set; }
        public string UnderGraduationBranch { get; set; }
        public string PostGraduationEducation { get; set; }
        public string PercentagePostEducation { get; set; }
        public DateTime? StartPostGraduationYear { get; set; }
        public DateTime? EndPostGraduationYear { get; set; }
        public string PostGraduationBranch { get; set; }
    }
}
