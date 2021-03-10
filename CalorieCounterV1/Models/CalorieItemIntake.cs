using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class CalorieItemIntake
    {
        [Key]
        public int CalorieItemIntakeId { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public double Quantity { get; set; }

        //[Required]
        //[Display(Name ="Time eaten")]
        //[DataType(DataType.Time)]
        //[DisplayFormat(DataFormatString = "{0:hh:mm}")]
        //public DateTime Time { get; set; }

        //nav props
        [ForeignKey("CalorieItem")]
        public int CalorieItemId { get; set; }
        public CalorieItem CalorieItem { get; set; }

        [ForeignKey("DailyCalorieCount")]
        [Display(Name = "Counts")]
        public int DailyCalorieCountId { get; set; }
        public DailyCalorieCount DailyCalorieCount { get; set; }

        //meals?
        //public int List<MealItem> MealItems{ get; set; }
    }
}