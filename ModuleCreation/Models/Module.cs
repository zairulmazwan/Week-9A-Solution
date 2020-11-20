using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModuleCreation.Models
{
    public class Module
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Module Code")]
        public string ModuleCode { get; set; }
        [Required]
        [Display(Name = "Module Name")]
        public string ModuleName { get; set; }
        [Required]
        [Display(Name = "Module Level")]
        public int ModuleLevel { get; set; }

        [Required]
        [Display(Name = "Module Year")]
        public List<bool> ModuleYear { get; set; } = new List<bool> { false, false, false, false }; //We will have year 1,2,3,4 : using checkboxes - True of flase
        [Required]
        [Display(Name = "Module Course")]
        public List<string> ModuleCourse { get; set; }
        [Required]
        public string ModuleOfferStatus { get; set; }



        
        public string Year { get; set; }

        public string Course { get; set; }

    }
}
