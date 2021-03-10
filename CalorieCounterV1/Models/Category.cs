using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        //nav props
        public List<CalorieItem> CalorieItems { get; set; }
    }
}