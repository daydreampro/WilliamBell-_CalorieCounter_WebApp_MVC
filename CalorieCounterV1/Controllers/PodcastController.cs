using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CalorieCounterV1.Models;

namespace CalorieCounterV1.Controllers
{
    public class PodcastController : Controller
    {
        CalorieCounterDbContext db = new CalorieCounterDbContext();
        // GET: Podcast
        public ActionResult Index()
        {
            //organise podcasts by season filter?

            var podcasts = db.Podcasts.ToList();
            return View(podcasts);
        }


    }
}
