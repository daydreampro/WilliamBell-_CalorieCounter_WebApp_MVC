using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class HomeViewModel
    {
        public HomeCarouselViewModel Features { get; set; }

        public Podcast FeaturePodcast { get; set; }//get latest podcast?

        public CalorieItem FeatureItem{ get; set; }//get featured fucking food!

        public CalorieItem FeatureItem2 { get; set; }
        //a third feature!? but what!?
    }
    public class HomeCarouselViewModel
    {
        public Feature F1 { get; set; }
        public Feature F2 { get; set; }
        public Feature F3 { get; set; }
    }
}