using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalorieCounterV1.Models
{
    public class CustomCalorieViewModel
    {
        [Required]
        [StringLength(50,ErrorMessage ="The {0} must be at least {2} characters long.",MinimumLength =1)]
        public string Name { get; set; }
        [Required]
        public double? Calories { get; set; }
        public double? Carbs { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        [Required]
        public double? Quantity { get; set; }
        [Display(Name="Send Sugestion to admin?")]
        public bool SendSugestion { get; set; }

        [Display(Name="Calorie Counts")]
        public IList<SelectListItem> DailyCalorieCountId { get; set; }
        public int? SelectedCount { get; set; }
    }

    public class CustomCalorieEditViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        public double? Calories { get; set; }
        public double? Carbs { get; set; }
        public double? Protein { get; set; }
        public double? Fat { get; set; }
        [Required]
        public double? Quantity { get; set; }
        [Display(Name = "Send Sugestion to admin?")]
        public bool SendSugestion { get; set; }

        public int TempCalorieItemId { get; set; }

        public int DailyCalorieCountId { get; set; }
    }

}