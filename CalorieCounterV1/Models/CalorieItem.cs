using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class CalorieItem
    {
        [Key]
        public int CalorieItemId { get; set; }
        [Required]
        [Display(Name = "Item Name")]
        public string Name  { get; set; }

        [Required]
        [Display(Name = "Calories")]
        [DisplayFormat(DataFormatString = "{0:0.##} kcal")]
        public double Calories { get; set; }

        [Required]
        [Display(Name = "Carbohydrates")]
        [DisplayFormat(DataFormatString = "{0:0.##} g")]
        public double Carbs { get; set; }

        [Required]
        [Display(Name = "Protein")]
        [DisplayFormat(DataFormatString = "{0:0.##} g")]
        public double Protein { get; set; }

        [Required]
        [Display(Name = "Fat")]
        [DisplayFormat(DataFormatString = "{0:0.##} g")]
        public double Fat { get; set; }

        [Required]
        [Display(Name = "Per size")]
        [DisplayFormat(DataFormatString = "{0:0.##} g")]
        public double ServingSize { get; set; }

        //image application
        
        [Display(Name = "Image Path")]
        [DataType(DataType.ImageUrl)]
        public string ImagePath { get; set; }

        //nav props
        [ForeignKey("Category")]
        [Display(Name="Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<CalorieItemIntake> CalorieItemIntakes { get; set; }



        public string Info()
        {
            return "Carbohydrates: " + this.Carbs + ". Protien: " + this.Protein + ". Fat: " + this.Fat + "\nPer: "+ this.ServingSize;
        }

    }

}