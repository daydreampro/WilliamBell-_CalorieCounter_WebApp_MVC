using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class DailyCountsDetailViewModel
    {
        public int DailyCalorieCountId { get; set; }
        public List<CalorieItemIntake> CalorieItemIntakes { get; set; }
        public List<TempCalorieItem> TempCalorieItems { get; set; }
    }
}