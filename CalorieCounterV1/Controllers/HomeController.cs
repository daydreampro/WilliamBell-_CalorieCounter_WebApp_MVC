using CalorieCounterV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using PagedList;
using System.Web.Helpers;
using System.IO;

namespace CalorieCounterV1.Controllers
{
    public class HomeController : Controller
    {
        private CalorieCounterDbContext db = new CalorieCounterDbContext();

        //Splash/Home Page
        public ActionResult Index()
        {
            //produce 2 unique random numbers
            int randy = new Random().Next(1, db.CalorieItems.Count());
            int randy2 = randy;

            while(randy == randy2)
            {
                randy2 = new Random().Next(1, db.CalorieItems.Count());
            }
            
            //Janky way of error handling implimentation of carousel items
            //List of all features that are set with the boolean of IsFeature
            List<Feature> feat = db.Features.Where(f => f.IsFeature == true).ToList();
            //Default of the carousel items are 3 so initilising 3 Feature objects
            var feat1 = new Feature();
            var feat2 = new Feature();
            var feat3 = new Feature();
            //switch statement to assign what feature where, 
            if (feat.Count > 0)
            {
                
                switch(feat.Count)
                {
                    case 1://all 3 if necessary
                        feat1 = feat[0];
                        feat2 = feat[0];
                        feat3 = feat[0];
                        break;
                    case 2:
                        feat1 = feat[0];
                        feat2 = feat[1];
                        feat3 = feat[0];
                        break;
                    case 3://best outcome
                        feat1 = feat[0];
                        feat2 = feat[1];
                        feat3 = feat[2];
                        break;
                }
            }
            else // if no features are meant to be featured...
            {
                feat1.ImagePath = "#";
                feat2.ImagePath = "#";
                feat3.ImagePath = "#";
            }
            //creating a view model
                var vm = new HomeViewModel()
            {
                Features = new HomeCarouselViewModel()//carousel items
                {
                    F1 = feat1,
                    F2 = feat2,
                    F3 = feat3
                },
                    
                FeatureItem = db.CalorieItems.Find(randy),//finds random calorie item to send to view
                FeatureItem2 = db.CalorieItems.Find(randy2),
                FeaturePodcast = db.Podcasts.OrderByDescending(p => p.Date).ToList().FirstOrDefault()//finds most "up to date" podcast
            };
            return View(vm);
        }

        public ActionResult FoodIndex(string sortOrder, string searchString, int? catId, int? page, string currentFilter)
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

            return View(items.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id) //no longer used, replaced with the [HttpGet] Add method
        {
            var item = db.CalorieItems.Find(id);
            var cat = db.Categories.Find(item.CategoryId);
            item.Category = cat;

            return View(item);
        }
        
        [HttpGet]
        public ActionResult Add(int id)
        {
            //find item by id
            CalorieItem ci = db.CalorieItems.Find(id);
            //find its category
            var cat = db.Categories.Find(ci.CategoryId);
            //set its category
            ci.Category = cat;
            //find user id
            var userId = User.Identity.GetUserId();
            //create a list of that users daily calorie counts
            //this is only seen if a member is logged in 
            IList<DailyCalorieCount> list = db.DailyCalorieCounts.Where(d => d.UserId == userId).OrderByDescending(d=>d.Date).ToList();

            //view model
            var addItem = new ItemIntakeViewModel()
            {
                CalorieItemId = ci.CalorieItemId, // to send to post
                CalorieItem = ci, //to grab info from view
                Quantity = ci.ServingSize, 
                // a list of counts are individually created into selectListItems
                // with value and text set
                // this was to set the format of the date how i wanted!
                DailyCalorieCountId = list.Select(s => new SelectListItem
                {
                    Value = s.DailyCalorieCountId.ToString(),
                    Text = (s.Name + ": " + s.Date.ToString("ddd - dd/MM"))
                }).ToList(),
                ImagePath = ci.ImagePath
            };

            return View(addItem);
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        public ActionResult Add([Bind(Include = "CalorieItemId, Quantity, SelectedCalorieCount")]ItemIntakeViewModel ii)
        {
            //if the required items have assigned values!
            if (ModelState.IsValid)
            {
                //new insstance of a calorie count
                DailyCalorieCount dcc = new DailyCalorieCount();

                //if there has been no existing calorie count selected
                if (ii.SelectedCalorieCount==null ||ii.SelectedCalorieCount == 0)
                {
                    // FUTURE :if not logged in save a session count?

                    //create a new daily count based on today
                    dcc = new DailyCalorieCount()
                    {
                        Date = DateTime.Now,
                        Name = "New Count",
                        UserId = User.Identity.GetUserId(),
                        TotalCalories = 0
                    };
                    db.DailyCalorieCounts.Add(dcc);
                    db.SaveChanges();
                    
                }
                else //otherwise if the user selected an existing daily calorie count save set instance to that
                {
                    dcc = db.DailyCalorieCounts.Find(ii.SelectedCalorieCount);
                }

                //after deciding what daily count to add the item to
                //add the item to it based on the view model
                db.CalorieItemIntakes.Add(new CalorieItemIntake()
                {
                    CalorieItemId = ii.CalorieItemId,
                    CalorieItem = db.CalorieItems.Find(ii.CalorieItemId),
                    Quantity = ii.Quantity,
                    DailyCalorieCount = dcc,
                    DailyCalorieCountId = dcc.DailyCalorieCountId
                });

                //add the amount to the calories
                //based on the base calorie amount * (qnty eaten / base serving size)
                dcc.TotalCalories += Calculator.Sum(db.CalorieItems.Find(ii.CalorieItemId).Calories,ii.Quantity,db.CalorieItems.Find(ii.CalorieItemId).ServingSize);
                
                //FUTURE ADD OTHER TOTALS in the same way
                //add totals of other attributes
                //dcc.TotalCarbs += Calculator.Sum()
                //dcc.TotalProtien += Calculator.Sum()
                //dcc.TotalFat += Calculator.Sum()

                db.SaveChanges();
                return RedirectToAction("FoodIndex");
                //update days total in nav bar?
            }
            // get user id
            var userId = User.Identity.GetUserId();
            //get calorie item and send back to view if viewModel is not valid
            ii.CalorieItem = db.CalorieItems.Find(ii.CalorieItemId);
            //imagepath
            ii.ImagePath = db.CalorieItems.Find(ii.CalorieItemId).ImagePath;
            // re send the daily counts to view
            ii.DailyCalorieCountId = db.DailyCalorieCounts.Where(c => c.UserId == userId).ToList().Select(s => new SelectListItem()
            {
                Value = s.DailyCalorieCountId.ToString(),
                Text = s.DateToString()
            }).ToList();

            return View(ii);
        }


        //creating a custom calorie item to add to users calorie count
        [Authorize(Roles = "Member")]
        public ActionResult CreateCustom()
        {
            // similar to adding a calorie to a count,
            //  a user may create their own if they cannot find the desired item
            // adding it to a temporary items table
            // the user will also have the ability to suggest the item to an admin for review
            var userId = User.Identity.GetUserId();
            IList<DailyCalorieCount> list = db.DailyCalorieCounts.Where(l => l.UserId == userId).OrderByDescending(l=>l.Date).ToList();
            CustomCalorieViewModel vm = new CustomCalorieViewModel()
            {
                DailyCalorieCountId = list.Select(s => new SelectListItem()
                {
                    Value = s.DailyCalorieCountId.ToString(),
                    Text = (s.Name + ": " + s.Date.ToString("ddd - dd/MM"))
                }).ToList()
            };
            
            return View(vm);
        }
        [HttpPost]
        public ActionResult CreateCustom([Bind(Include ="Name,Calories,Carbs,Protein,Fat,Quantity,SelectedCount, SendSugestion")]CustomCalorieViewModel vm)
        {
            if (ModelState.IsValid)
            {
                DailyCalorieCount dcc = new DailyCalorieCount();
                bool send = vm.SendSugestion;//set the send suggestion bool
                
                if (vm.SelectedCount == null || vm.SelectedCount == 0)
                {
                    
                    dcc = new DailyCalorieCount()
                    {
                        Date = DateTime.Now,
                        Name = "New Count",
                        UserId = User.Identity.GetUserId(),
                        TotalCalories = 0
                    };
                    db.DailyCalorieCounts.Add(dcc);
                    db.SaveChanges();
                }
                else
                {
                    dcc = db.DailyCalorieCounts.Find(vm.SelectedCount);
                }
                
                db.TempCalorieItems.Add(new TempCalorieItem()//creating the temp item
                {
                    Name = vm.Name,
                    Calories = CastDouble(vm.Calories),
                    Carbs = CastDouble(vm.Carbs),
                    Protein= CastDouble(vm.Protein),
                    Fat = CastDouble(vm.Fat),
                    ServingSize = CastDouble(vm.Quantity),
                    DailyCalorieCount = dcc,
                    DailyCalorieCountId = dcc.DailyCalorieCountId,
                    SendSugestion = send
                });

                dcc.TotalCalories += Calculator.Sum((double)vm.Calories, (double)vm.Quantity, (double)vm.Quantity);
                //add totals of other attributes
                //dcc.TotalCarbs += Calculator.Sum()
                //dcc.TotalProtien += Calculator.Sum()
                //dcc.TotalFat += Calculator.Sum()

                db.SaveChanges();
                return RedirectToAction("FoodIndex");

            }
            var userId = User.Identity.GetUserId();
            vm.DailyCalorieCountId = db.DailyCalorieCounts.Where(c=>c.UserId == userId).OrderByDescending(c=>c.Date).ToList().Select(s => new SelectListItem()
            {
                Value = s.DailyCalorieCountId.ToString(),
                Text = (s.Name + ": " + s.Date.ToString("ddd - dd/MM"))
            }).ToList();
            return View(vm);
        }

        private double CastDouble(double? d)
        {//making usre null values are not being passed
            if (d != null)
            {
                return (double)d;
            }
            return 0;
        }

        public ActionResult UnderstandingCalories()
        {
            // nothing fancy in here
            return View();
        }

        public ActionResult Contact()
        {
            //or here
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}