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
        public string Title { get; set; }
        [Required]
        public string Batch { get; set; }
        public List<Student> GroupMembers { get; set; }
    }
}