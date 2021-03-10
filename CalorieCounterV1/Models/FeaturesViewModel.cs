using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalorieCounterV1.Models
{
    public class FeaturesViewModel
    {
        //[Required]
        [Display(Name="Feature One")]
        public IEnumerable<SelectListItem> Feature1Id { get; set; }
        //[Required]
        [Display(Name = "Feature One")]
        public IEnumerable<SelectListItem> Feature2Id { get; set; }
        //[Required]
        [Display(Name = "Feature One")]
        public IEnumerable<SelectListItem> Feature3Id { get; set; }
        public int F1 { get; set; }
        public int F2 { get; set; }
        public int F3 { get; set; }

    }

    

}