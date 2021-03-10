using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class DailyCalorieCount
    {
        [Key]
        public int DailyCalorieCountId { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString ="{0:0.##} kcal")]
        public double TotalCalories { get; set; }
        [Required]

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yy}")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:ddd - dd / MMM}")]
        public DateTime Date { get; set; }

        //nav props
        public List<CalorieItemIntake> CalorieItemIntakes { get; set; }
        public List<TempCalorieItem> TempCalorieItemIntakes { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }


        //methods
        public string DateToString()
        {
            return this.Date.ToString("ddd - dd/MM");
        }

        
    }
}