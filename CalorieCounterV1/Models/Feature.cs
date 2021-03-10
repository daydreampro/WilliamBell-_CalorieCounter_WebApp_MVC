using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class Feature
    {
        [Key]
        public int FeatureId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.ImageUrl)]
        [Display(Name ="Upload Image")]
        public string ImagePath { get; set; }
        
        [Required]
        [Display(Name = "RAW Link")]
        public string Link { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        public bool IsFeature { get; set; }


        //method
        public string ImageSave()
        {
            int length = 15;
            string name = Regex.Replace(this.Description, @"\s+", "_").ToLower();
            if (name.Length > length)
            {
                name = name.Substring(0, length);
            }
            return "feautre_" + name + "_" + this.FeatureId;
        }
    }
}