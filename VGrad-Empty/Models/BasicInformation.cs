using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VGrad_Empty.Models
{
    public class BasicInformation
    {
        [Key]
        public int BasicInformationID { get; set; }
        [Required]
        [Display(Name ="Father Name")]
        public string FatherName { get; set; }
        
        public string Image { get; set; }

        [Required]
        [Display(Name ="Your Introduction")]
        public string Introduction { get; set; }
    }
}