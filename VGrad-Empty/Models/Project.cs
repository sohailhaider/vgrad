using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VGrad_Empty.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }
        [Required]
        [Display(Name = "Project Title")]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }
        [Required]
        [Display(Name = "Supervisor Name")]
        public string SupervisorName { get; set; }
        [Required]
        public string Batch { get; set; }
        public virtual List<Student> GroupMembers { get; set; }
    }
}