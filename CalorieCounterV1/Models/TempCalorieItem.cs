using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class TempCalorieItem
    {
        [Key]
        public int TempCalorieItemId { get; set; }
        [Required]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Calories")]
        [DisplayFormat(DataFormatString = "{0:0.##} kcal")]
        public double Calories { get; set; }

        [Display(Name = "Carbohydrates")]
        [DisplayFormat(DataFormatString = "{0:0.##} g")]
        public double Carbs { get; set; }

        [Display(Name = "Protein")]
        [DisplayFormat(DataFormatString = "{0:0.##} g")]
        
        public double Protein { get; set; }

        [Display(Name = "Fat")]
        [DisplayFormat(DataFormatString = "{0:0.##} g")]
        public double Fat { get; set; }

        [Display(Name = "Quantity")]
        [DisplayFormat(DataFormatString = "{0:0.##} g")]
        public double ServingSize { get; set; }//changed the name of this, if any errors!

        [Display(Name = "Send Suggetion")]
        public bool SendSugestion { get; set; }

        //nav props

        //[ForeignKey("User")]
        //public string UserId { get; set; }
        //public User User { get; set; }

        [ForeignKey("DailyCalorieCount")]
        [Display(Name = "Counts")]
        public int DailyCalorieCountId { get; set; }
        public DailyCalorieCount DailyCalorieCount { get; set; }

    }
}