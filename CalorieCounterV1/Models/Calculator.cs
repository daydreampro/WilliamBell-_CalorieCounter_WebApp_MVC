using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public static class Calculator
    {
        public static double GrandTotal(List<CalorieItemIntake> items)
        {
            double total = 0;
               
            foreach(CalorieItemIntake i in items)
            {
                total += Calculator.Sum(i.CalorieItem.Calories, i.Quantity, i.CalorieItem.ServingSize);
            }

            return total;
        }
        public static double Sum(double baseValue, double qnty,double percentage)
        {
            //if everything is based off of 100g
            //qnty is 1 to 1 percentage of orrigional attribute
            double total = baseValue * (qnty / percentage);

            return Math.Round(total,2);
        }

        public static string SumString(double baseValue, double qnty, double perc)
        {
            double total = Sum(baseValue, qnty, perc);
            
            return String.Format("{0:0.##} g", total);
            
        }
        public static string SumStringCal(double baseValue, double qnty, double perc)
        {
            double total = Sum(baseValue, qnty, perc);

            return String.Format("{0:0.##} kcal", total);

        }

        private static double PercentageSum()
        {
            double answer = 0;

            return answer;
        }
    }
}