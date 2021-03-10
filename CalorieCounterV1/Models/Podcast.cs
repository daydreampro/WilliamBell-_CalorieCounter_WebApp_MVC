using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class Podcast
    {
        [Key]
        public int PodcastId { get; set; }
        [Required]
        public string Title { get; set; }
        public double Length { get; set; }

        [Display(Name ="Episode")]
        [Required]
        [DisplayFormat(DataFormatString = "E{0:##}")]
        public int EpisodeNumber { get; set; }

        [Display(Name = "Season")]
        [Required]
        [DisplayFormat(DataFormatString = "S{0:##}")]
        public int SeasonNumber { get; set; }

        [Required]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        public DateTime Date { get; set; }

        [DataType(DataType.Url)]
        [Required]
        [Display(Name ="Podcast File")]
        public string AudioPath { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Image File")]
        public string ImagePath { get; set; }

        public string AudioPathBuilder()
        {
            return this.PodcastId + "_" + this.Title.ToLower() + "_S" + this.SeasonNumber + "_E" + this.EpisodeNumber ;
        }
        public string Info()
        {
            
            return this.SeasonNumber.ToString() + ":" + this.EpisodeNumber.ToString();
        }
        public string ShortDescription()
        {
            int i = this.Description.Length;
            if (i > 30)
            {
                i = 30;
            }
            return this.Description.Substring(0, i) + "...";
        }
    }
}