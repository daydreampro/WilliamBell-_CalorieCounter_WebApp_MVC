using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CalorieCounterV1.Models;
using System.Web.Mvc;
using PagedList;
using System.IO;
using System.Data.Entity;
using System.Net;

namespace CalorieCounterV1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly CalorieCounterDbContext db = new CalorieCounterDbContext();
        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        ////////////////////////////////////////////////////////////////////
        ///////////////// DISPLAY ALLS/////////////////////////////
        ///////////////////////////////////////////////////////////////////
        public ActionResult AllCalorieItems(string sortOrder, string searchString, int? catId, int? page, string currentFilter)
        {
            //sortbags
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentCat = catId;
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CalorieSort = sortOrder == "Calories" ? "cal_desc" : "Calories";
            ViewBag.Categories = db.Categories.ToList();

            //paging
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //creation of IQueryable<T>
            var items = from s in db.CalorieItems
                        select s;
            //categories?
            if (catId > 0)
            {
                items = items.Where(i => i.CategoryId == catId);
            }
            //searchstring
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.Name.Contains(searchString));
            }
            //what order to sort by
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(i => i.Name);
                    break;
                case "Calories":
                    items = items.OrderBy(i => i.Calories);
                    break;
                case "cal_desc":
                    items = items.OrderByDescending(i => i.Calories);
                    break;
                default:
                    items = items.OrderBy(i => i.Name);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);//if page has a value return that otherwise return 1

            if(db.CalorieItems.Where(i=>i.CategoryId == 0).ToList().Count > 0)
            {
                ViewBag.Other = true;
            }
            else
            {
                ViewBag.Other = false;
            }

            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AllCategories()
        {
            var cats = db.Categories.ToList();
            return View(cats);
        }

        public ActionResult AllUsers()
        {
            return View(db.Users.ToList());
        }

        public ActionResult AllRequests()
        {
            return View(db.TempCalorieItems.Where(t => t.SendSugestion == true).ToList());
        }

        public ActionResult AllPodcasts()
        {
            return View(db.Podcasts.ToList());
        }

        public ActionResult AllFeatures()
        {
            var features = db.Features.ToList();

            return View(features);
        }
        ////////////////////////////////////////////////////////////////////
        ///////////////// FEATURES      /////////////////////////////
        ///////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult CreateFeature()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateFeature([Bind(Include ="Description,ImagePath,Link")] Feature feature)
        {
            if (ModelState.IsValid)
            {
                feature.Date = DateTime.Now;
                feature.IsFeature = false;
                db.Features.Add(feature);
                db.SaveChanges();


                if (!String.IsNullOrEmpty(feature.ImagePath))
                {

                    HttpPostedFileBase ImagePath = Request.Files["ImagePath"];

                    if (ImagePath != null && ImagePath.ContentLength > 0)
                    {
                        try
                        {
                            string extension = System.IO.Path.GetExtension(ImagePath.FileName);
                            string fileName = feature.ImageSave() + extension;
                            string path = Path.Combine(Server.MapPath(@"~/Content/Images/Features/"), fileName);

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            ImagePath.SaveAs(path);
                            feature.ImagePath = @"/Content/Images/Features/" + fileName;
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            string error = e.Message.ToString();
                        }
                    }
                }

                return RedirectToAction("AllFeatures");
            }
            return View(feature);
        }
        [HttpGet]
        public ActionResult EditFeature(int? id)
        {
            if (id == null)
            {

            }
            var f = db.Features.Find(id);
            if (f == null)
            {
                return HttpNotFound();
            }
            return View(f);
        }
        [HttpPost]
        public ActionResult EditFeature([Bind(Include ="FeatureId, IsFeature, Description,ImagePath,Link")]Feature f,string NewImagePath)
        {
            
            if (ModelState.IsValid)
            {
                //if the NewImagePath has a string make that the new image
                if (!String.IsNullOrEmpty(NewImagePath))
                {
                    HttpPostedFileBase ImagePath = Request.Files["NewImagePath"];

                    if (ImagePath != null && ImagePath.ContentLength > 0)
                    {
                        try
                        {
                            string extension = System.IO.Path.GetExtension(ImagePath.FileName);
                            string fileName = f.ImageSave() + extension;
                            string path = Path.Combine(Server.MapPath(@"~/Content/Images/Features/"), fileName);

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            ImagePath.SaveAs(path);
                            f.ImagePath = @"/Content/Images/Features/" + fileName;
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            string error = e.Message.ToString();
                        }
                    }
                }

                f.Date = DateTime.Now;
                
                db.Entry(f).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AllFeatures");
            }
            return View(f);
        }
        [HttpGet]
        public ActionResult EditFeatures()
        {
            var list = db.Features.ToList();

            var vm = new FeaturesViewModel()
            {
                Feature1Id = new SelectList(list,"FeatureId","Description"),
                Feature2Id = new SelectList(list,"FeatureId","Description"),
                Feature3Id = new SelectList(list,"FeatureId","Description"),
            };

            return View(vm);
        }
        [HttpPost]
        public ActionResult EditFeatures([Bind(Include ="F1,F2,F3,")]FeaturesViewModel vm)
        {
            if (ModelState.IsValid)
            {
                foreach(var i in db.Features.ToList())
                {
                    i.IsFeature = false;
                }
                db.Features.Find(vm.F1).IsFeature = true;
                db.Features.Find(vm.F2).IsFeature = true;
                db.Features.Find(vm.F3).IsFeature = true;

                db.SaveChanges();

                return RedirectToAction("AllFeatures");
            }
            return View(vm);
        }

        public ActionResult DeleteFeature(int id)
        {
            var f = db.Features.Find(id);
            db.Features.Remove(f);
            db.SaveChanges();
            return RedirectToAction("AllFeatures");
        }
        ////////////////////////////////////////////////////////////////////
        ///////////////// CALORIE ITEMS   /////////////////////////////
        ///////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult CreateCalorieItem()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult CreateCalorieItem([Bind(Include = "Name,Calories,Carbs,Protein,Fat,ServingSize,CategoryId,ImagePath")] CalorieItem calorieItem)
        {
            if (ModelState.IsValid)
            {

                db.CalorieItems.Add(calorieItem);
                db.SaveChanges();

                if (!String.IsNullOrEmpty(calorieItem.ImagePath))
                {

                    HttpPostedFileBase ImagePath = Request.Files["ImagePath"];
                    CalorieItemImage(calorieItem.CalorieItemId, ImagePath);
                }
                else
                {
                    calorieItem.ImagePath = @"/Content/Images/CalorieItemImages/dog_ate_img.jpeg";
                    db.SaveChanges();
                }
                

                return RedirectToAction("AllCalorieItems");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");

            return View(calorieItem);
        }

        [HttpGet]
        public ActionResult EditCalorieItem(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("AllCalorieItems");
            }

            var item = db.CalorieItems.Find(id);

            if (item == null)
            {
                return RedirectToAction("AllCalorieItems");
            }

            IList<Category> cats = db.Categories.ToList();

            IEnumerable<SelectListItem> catList = cats.Select(s => new SelectListItem
            {
                Value = s.CategoryId.ToString(),
                Text = s.Name
            }).ToList();

            foreach(SelectListItem s in catList)
            {
                if(s.Value == item.CategoryId.ToString())
                {
                    s.Selected = true;
                }
            }

            ViewBag.CategoryId = catList;


            //ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");

            return View(item);
        }

        [HttpPost]
        public ActionResult EditCalorieItem([Bind(Include = "CalorieItemId,Name,Calories,Carbs,Protein,Fat,ServingSize,CategoryId,ImagePath")] CalorieItem item)
        {
            if (ModelState.IsValid)
            {
                var temp = db.CalorieItems.Find(item.CalorieItemId);
                string currentImagePath = "";
                if (!String.IsNullOrEmpty(temp.ImagePath) && String.IsNullOrEmpty(item.ImagePath))
                {
                    currentImagePath = temp.ImagePath;
                }

                db.Entry(temp).State = EntityState.Detached;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

                if (!String.IsNullOrEmpty(item.ImagePath) )
                {
                    HttpPostedFileBase ImagePath = Request.Files["ImagePath"];

                    CalorieItemImage(item.CalorieItemId, ImagePath);
                }
                else
                {
                    item.ImagePath = currentImagePath;
                    db.SaveChanges();
                }

                return RedirectToAction("AllCalorieItems");
            }
            IList<Category> cats = db.Categories.ToList();

            IEnumerable<SelectListItem> catList = cats.Select(s => new SelectListItem
            {
                Value = s.CategoryId.ToString(),
                Text = s.Name
            }).ToList();

            foreach (SelectListItem s in catList)
            {
                if (s.Value == item.CategoryId.ToString())
                {
                    s.Selected = true;
                }
            }

            ViewBag.CategoryId = catList;
            return View(item);
        }

        
        public ActionResult DeleteCalorieItem(int id)
        {
            var item = db.CalorieItems.Find(id);
            if(item != null)
            {
                db.CalorieItems.Remove(item);
                db.SaveChanges();
                return RedirectToAction("AllCalorieItems");

            }
            return View();
        }

        ////////////////////////////////////////////////////////////////////
        ///////////////// PODCASTS    /////////////////////////////
        ///////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult CreatePodcast()
        {
            //send the latest season and the next episode nmber to the view, for ease of use
            int ep = db.Podcasts.ToList().LastOrDefault().EpisodeNumber + 1;
            int se = db.Podcasts.ToList().LastOrDefault().SeasonNumber;
            return View(new Podcast() { EpisodeNumber = ep, SeasonNumber = se});
        }
        [HttpPost]
        public ActionResult CreatePodcast([Bind(Include = "Title,Description,EpisodeNumber,SeasonNumber,AudioPath")]Podcast podcast)
        {
            if (ModelState.IsValid)
            {
                podcast.Date = DateTime.Now;
                podcast.ImagePath = @"/Content/Images/podcast_cat.jpg";
                db.Podcasts.Add(podcast);
                db.SaveChanges();

                if (!String.IsNullOrEmpty(podcast.AudioPath))
                {
                    HttpPostedFileBase AudioPath = Request.Files["AudioPath"];

                    if (AudioPath != null && AudioPath.ContentLength > 0)
                    {
                        try
                        {

                            string extension = System.IO.Path.GetExtension(AudioPath.FileName);
                            string fileName = podcast.AudioPathBuilder() + extension;
                            string path = Path.Combine(Server.MapPath(@"/Content/Podcasts/"), fileName);

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            AudioPath.SaveAs(path);
                            podcast.AudioPath = @"/Content/Podcasts/" + fileName;
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            string ex = e.Message;
                        }
                    }
                }
                return RedirectToAction("AllPodcasts");
            }
            return View(podcast);
        }

        [HttpGet]
        public ActionResult EditPodcast(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pod = db.Podcasts.Find(id);

            if(pod == null)
            {
                return HttpNotFound();
            }

            return View(pod);
        }

        [HttpPost]
        public ActionResult EditPodcast([Bind(Include ="PodcastId,Date,ImagePath,Title,EpisodeNumber,SeasonNumber,Description,AudioPath")]Podcast pod, string NewAudioPath)
        {

            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(NewAudioPath))
                {
                    HttpPostedFileBase AudioPath = Request.Files["NewAudioPath"];

                    if (AudioPath != null && AudioPath.ContentLength > 0)
                    {
                        try
                        {

                            string extension = System.IO.Path.GetExtension(AudioPath.FileName);
                            string fileName = pod.AudioPathBuilder() + extension;
                            string path = Path.Combine(Server.MapPath(@"/Content/Podcasts/"), fileName);

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }

                            AudioPath.SaveAs(path);
                            pod.AudioPath = @"/Content/Podcasts/" + fileName;
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            string ex = e.Message;
                        }

                    }

                }
                db.Entry(pod).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("AllPodcasts");
            }
            return View(pod);
        }

        public ActionResult DeletePodcast(int id)
        {
            var pod = db.Podcasts.Find(id);

            db.Podcasts.Remove(pod);

            db.SaveChanges();

            return RedirectToAction("AllPodcasts");
        }

        ////////////////////////////////////////////////////////////////////
        ///////////////// CATEGORIES /////////////////////////////
        ///////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult CreateCategory()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateCategory([Bind(Include ="Name")] Category cat)
        {
            if (ModelState.IsValid)
            {
                if (db.Categories.Where(c => c.Name.Equals(cat.Name)).Count() > 0)
                {
                    ViewBag.Message = "Category already exists!";
                    return View(cat);
                }

                db.Categories.Add(cat);
                db.SaveChanges();

                return RedirectToAction("AllCategories");

            }
            return View();
        }

        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            return View(db.Categories.Find(id));
        }
        [HttpPost]
        public ActionResult EditCategory([Bind(Include = "CategoryId, Name")] Category cat)
        {
            if (ModelState.IsValid)
            {
                if (db.Categories.Where(c => c.Name.Equals(cat.Name)).Count() > 0)
                {
                    ViewBag.Message = "Category already exists!";
                    return View(cat);
                }

                db.Entry(cat).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = null;
                return RedirectToAction("AllCategories");

            }
            return View();
        }

        ////////////////////////////////////////////////////////////////////
        ///////////////// REQUESTS /////////////////////////////
        ///////////////////////////////////////////////////////////////////
        [HttpGet]
        public ActionResult ReviewRequest(int? id)
        {
            if (id == null)
            {
                ViewBag.ReqMessage = "Request Error: ID does not exist!";
                return RedirectToAction("AllRequests");
            }

            var item = db.TempCalorieItems.Find(id);

            var vm = new ReviewTempCalorieItemViewModel()
            {
                TempCalorieItemId = item.TempCalorieItemId,
                Name = item.Name,
                Calories = item.Calories,
                Carbs = item.Carbs,
                Protein = item.Protein,
                Fat = item.Fat,
                ServingSize = item.ServingSize
            };


            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            
            return View(vm);
        }
        [HttpPost]
        public ActionResult ReviewRequest([Bind(Include ="Name, Calories, Carbs, Protein, Fat, ServingSize,ImagePath, CategoryId")]ReviewTempCalorieItemViewModel calItem, int TempCalorieItemId)
        {
            if (ModelState.IsValid)
            {
                var item = new CalorieItem()
                {
                    Name = calItem.Name,
                    Calories = calItem.Calories,
                    Carbs = calItem.Carbs,
                    Protein = calItem.Protein,
                    Fat = calItem.Fat,
                    ServingSize = calItem.ServingSize,
                    Category = db.Categories.Find(calItem.CategoryId)
                };
                //add the new calorie Item
                db.CalorieItems.Add(item);
                db.SaveChanges();
                //add Image
                if (!String.IsNullOrEmpty(calItem.ImagePath))
                {
                    HttpPostedFileBase ImagePath = Request.Files["ImagePath"];
                    CalorieItemImage(item.CalorieItemId, ImagePath);
                }
                else
                {
                    item.ImagePath = @"/Content/Images/CalorieItemImages/dog_ate_img.jpeg";
                    db.SaveChanges();
                }

                //REMOVING TEMP AND ADDING NEW CALORIE ITEM INSTEAD
                //find calorie count of temp calorie item
                var temp = db.TempCalorieItems.Find(TempCalorieItemId);
                var count = db.DailyCalorieCounts.Find(temp.DailyCalorieCountId);
                //remove temp and add the new, accpeted, calorie item
                //create intake item
                var calorieItemIntake = new CalorieItemIntake()
                {
                    CalorieItem = item,
                    DailyCalorieCount = count,
                    Quantity = temp.ServingSize
                };
                //add intake item, as to give it a ID
                db.CalorieItemIntakes.Add(calorieItemIntake);

                //// DO THE MATH IF : calorie values/ratio were to change the count would also to reflect this update, otherwise the total stays the same
                //minus the temp calories from total calories
                //count.TotalCalories -= temp.Calories;
                //add the new calorie
                //count.TotalCalories += Calculator.Sum(db.CalorieItems.Find(calorieItemIntake.CalorieItemId).Calories, calorieItemIntake.Quantity, db.CalorieItems.Find(calorieItemIntake.CalorieItemId).ServingSize);
                //db.Entry(count).State = EntityState.Modified;

                db.TempCalorieItems.Remove(temp);
                db.SaveChanges();

                return RedirectToAction("AllRequests");
            }


            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");

            return View(calItem);
        }

        public ActionResult DenyRequest(int? id)
        {
            
            if (id == null)
            {
                ViewBag.ReqMessage = "Request Error: ID does not exist!";
                return RedirectToAction("AllRequests");
            }
            var item= db.TempCalorieItems.Find(id);

            item.SendSugestion = false;

            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.ReqMessage = "Request Denied";

            return RedirectToAction("AllRequests");
        }

        private void CalorieItemImage(int cal_id, HttpPostedFileBase ImagePath)
        {
            var calorieItem = db.CalorieItems.Find(cal_id);

            if (ImagePath != null && ImagePath.ContentLength > 0)
            {
                try
                {
                    string extension = System.IO.Path.GetExtension(ImagePath.FileName);
                    string fileName = calorieItem.Name.ToLower() + "_" + calorieItem.CalorieItemId + extension;
                    string path = Path.Combine(Server.MapPath(@"~/Content/Images/CalorieItemImages/"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    ImagePath.SaveAs(path);
                    calorieItem.ImagePath = @"/Content/Images/CalorieItemImages/" + fileName;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    string error = e.Message.ToString();
                }
            }
        }

        
    }

}