using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VGrad_Empty.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Provide Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Please Provide Password")]
        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}