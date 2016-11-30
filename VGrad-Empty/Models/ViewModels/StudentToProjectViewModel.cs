using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VGrad_Empty.Models.ViewModels
{
    public class StudentToProjectViewModel
    {
        [Required(ErrorMessage = "Please select any student from dropdown")]
        [Display(Name ="Student")]
        public int StudentID { get; set; }
        [Required(ErrorMessage = "Please select any Project from dropdown")]
        [Display(Name = "Project")]
        public int ProjectID { get; set; }
    }
}