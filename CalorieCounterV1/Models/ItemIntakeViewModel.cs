using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CalorieCounterV1.Models
{
    public class ItemIntakeViewModel
    {

        /// 
        /// /////////////////////////////////////////////////////////
        /// 
        //ITEM
        public int CalorieItemId { get; set; }

        public CalorieItem CalorieItem { get; set; }

        [Display(Name = "Portion size")]
        [DisplayFormat(DataFormatString = "{0}g")]
        public double Quantity { get; set; }

        //public List<DailyCalorieCount> DailyCalorieCounts { get; set; }
        [Display(Name = "Calorie Counts")]
        public IList<SelectListItem> DailyCalorieCountId { get; set; }

        public int? SelectedCalorieCount { get; set; }

        public string ImagePath { get; set; }

    
    }

    

}