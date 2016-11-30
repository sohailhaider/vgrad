using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VGrad_Empty.Models
{
    public class Student
    {
        [Key, ForeignKey("User")]
        [Display(Name ="User Account")]
        public int StudentId { get; set; }
        [Required]
        public string Batch { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Graduation Date")]
        public DateTime GraduationDate { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name ="Contact Number")]
        public string ContactNumber { get; set; }
        [Display(Name ="Employeement Status")]
        public EmployementStatus Employeement { get; set; }

        public virtual User User { get; set; }
        public Project Project { get; set; }
    }
}