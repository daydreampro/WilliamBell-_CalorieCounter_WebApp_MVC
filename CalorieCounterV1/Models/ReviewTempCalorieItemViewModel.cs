using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class ReviewTempCalorieItemViewModel
    {
        [Required]
        public int TempCalorieItemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Calories { get; set; }
        [Required]
        public double Carbs { get; set; }
        [Required]
        public double Protein { get; set; }
        [Required]
        public double Fat { get; set; }
        [Required]
        [Display(Name="Per Serving Size")]
        public double ServingSize { get; set; }
        [Required]
        public int CategoryId { get; set; }
        
        public string ImagePath { get; set; }
    }
}