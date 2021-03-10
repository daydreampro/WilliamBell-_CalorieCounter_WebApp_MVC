using CalorieCounterV1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;

namespace CalorieCounterV1.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class MemberController : Controller
    {
        private CalorieCounterDbContext db = new CalorieCounterDbContext();
        // GET: Member

        public ActionResult Index()
        {
            var counts = db.DailyCalorieCounts.Include(c => c.CalorieItemIntakes).Include(c => c.User);
            var userId = User.Identity.GetUserId();

            counts = counts.Where(c => c.UserId == userId).OrderByDescending(c => c.Date);

            return View(counts.ToList());
        }

        // GET: Member/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dcc = db.DailyCalorieCounts.Find(id);

            var items = db.CalorieItemIntakes.Include(i=> i.CalorieItem).Where(i => i.DailyCalorieCountId == dcc.DailyCalorieCountId).ToList();

            var tempItems = db.TempCalorieItems.Where(t => t.DailyCalorieCountId == id).ToList();

            DailyCountsDetailViewModel vm = new DailyCountsDetailViewModel()
            {
                DailyCalorieCountId =dcc.DailyCalorieCountId,
                CalorieItemIntakes = items,
                TempCalorieItems = tempItems
            };

            ViewBag.Count = dcc.Name + " - " + dcc.Date.ToString("dddd - dd / MMM");

            if(dcc == null)
            {
                return HttpNotFound();
            }

            return View(vm);

            //return View(items.ToList());
        }

        // GET: Member/Create
        public ActionResult CreateCount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCount([Bind(Include ="Name, Date")]DailyCalorieCount dcc)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                if (!String.IsNullOrEmpty(userId))
                {
                    var cs = db.DailyCalorieCounts.Where(c => c.UserId.Equals(userId));

                    foreach(var c in cs)
                    {
                        if (c.Date.Date.Equals(dcc.Date.Date))
                        {
                            return View(dcc);
                        }
                    }
                    dcc.UserId = userId;
                    dcc.User = db.Users.Find(userId);

                    db.DailyCalorieCounts.Add(dcc);
                    
                    db.SaveChanges();

                    int id = dcc.DailyCalorieCountId;

                    return RedirectToAction("Details", new { id = id });
                }

                
            }

            return View(dcc);
        }

        [HttpGet]
        public ActionResult EditCount(int? id)
        {
            if(id == null)
            {
                //do that thing
            }
            var dcc = db.DailyCalorieCounts.Find(id);
            if(dcc == null)
            {
                return HttpNotFound();
            }

            return View(dcc);
        }

        [HttpPost]
        public ActionResult EditCount([Bind(Include ="UserId, TotalCalories, DailyCalorieCountId, Name, Date")] DailyCalorieCount dcc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dcc).State = EntityState.Modified;
                db.SaveChanges();

                int id = dcc.DailyCalorieCountId;
                return RedirectToAction("Details", new { id = id });
            }

            return View(dcc);
        }
        //get
        public ActionResult EditCalorieItem(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            CalorieItemIntake cii = db.CalorieItemIntakes.Find(id);


            if (cii == null)
            {
                return HttpNotFound();
            }

            ViewBag.Name = db.CalorieItems.Find(cii.CalorieItemId).Name;

            return View(cii);
        }
        
        [HttpPost]
        public ActionResult EditCalorieItem([Bind(Include ="CalorieItemIntakeId, CalorieItemId, DailyCalorieCountId,Quantity")]CalorieItemIntake item)
        {
            if (ModelState.IsValid)
            {
                DailyCalorieCount dcc = db.DailyCalorieCounts.Find(item.DailyCalorieCountId);

                CalorieItemIntake ocu = db.CalorieItemIntakes.Find(item.CalorieItemIntakeId);
                //pdateCountSum(item.CalorieItemIntakeId,dcc.DailyCalorieCountId);
                double cal = db.CalorieItems.Find(ocu.CalorieItemId).Calories;
                double size = db.CalorieItems.Find(ocu.CalorieItemId).ServingSize;
                double qnty = ocu.Quantity;

                dcc.TotalCalories -= Calculator.Sum(cal, qnty, size);

                dcc.TotalCalories += Calculator.Sum(db.CalorieItems.Find(item.CalorieItemId).Calories, item.Quantity, db.CalorieItems.Find(item.CalorieItemId).ServingSize);

                db.Entry(ocu).State = EntityState.Detached;
                db.Entry(dcc).State = EntityState.Modified;
                

                //dis bitch breaks//////////////////////////////////
                db.Entry(item).State = EntityState.Modified;////////
                ////////////////////////////////////////////////////
                
                db.SaveChanges();

                return RedirectToAction("Details", new { id = item.DailyCalorieCountId });
            }

            ViewBag.Name = db.CalorieItems.Find(item.CalorieItemId);

            return View(item);
        }

        private void UpdateCountSum(int cal_id,int count_id)
        {
            CalorieItemIntake ci = db.CalorieItemIntakes.Find(cal_id);
            DailyCalorieCount dc = db.DailyCalorieCounts.Find(count_id);

            dc.TotalCalories -= Calculator.Sum(ci.CalorieItem.Calories, ci.Quantity, ci.CalorieItem.ServingSize);

        }

        [HttpGet]
        public ActionResult EditTempItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            TempCalorieItem tci = db.TempCalorieItems.Find(id);
            //var c = db.DailyCalorieCounts.Find(tci.DailyCalorieCountId);
            if (tci == null)
            {
                return HttpNotFound();
            }
            CustomCalorieEditViewModel vm = new CustomCalorieEditViewModel()
            {
                Name = tci.Name,
                Calories = tci.Calories,
                Carbs = tci.Carbs,
                Protein = tci.Protein,
                Fat=tci.Fat,
                Quantity = tci.ServingSize,
                DailyCalorieCountId = tci.DailyCalorieCountId,
                TempCalorieItemId = tci.TempCalorieItemId,
                SendSugestion = tci.SendSugestion
            };
            return View(vm);
        }
        [HttpPost]
        public ActionResult EditTempItem([Bind(Include = "TempCalorieItemId, DailyCalorieCountId,Name, Calories, Carbs,Protein,Fat, Quantity, SendSugestion")]CustomCalorieEditViewModel item)
        {
            if (ModelState.IsValid)
            {
                DailyCalorieCount dcc = db.DailyCalorieCounts.Find(item.DailyCalorieCountId);

                //TempCalorieItem tci = db.TempCalorieItems.Where(o => o.DailyCalorieCountId == dcc.DailyCalorieCountId).ToList()[0];
                TempCalorieItem tci = db.TempCalorieItems.Find(item.TempCalorieItemId);
                //minus the og calories
                dcc.TotalCalories -= tci.Calories;
                //add the new calories
                dcc.TotalCalories += (double)item.Calories;

                //save the dailycount
                db.Entry(dcc).State = EntityState.Modified;

                //make sure shit aint null
                if(item.Carbs == null)
                {
                    item.Carbs = 0;
                }
                if (item.Protein == null)
                {
                    item.Protein = 0;
                }
                if (item.Fat == null)
                {
                    item.Fat = 0;
                }
                //update temp item
                tci.Name = item.Name;
                tci.Calories = (double)item.Calories;
                tci.Carbs = (double)item.Carbs;
                tci.Protein = (double)item.Protein;
                tci.Fat = (double)item.Fat;
                tci.ServingSize = (double)item.Quantity;
                tci.SendSugestion = item.SendSugestion;
                tci.DailyCalorieCount = db.DailyCalorieCounts.Find(item.DailyCalorieCountId);

                //save that change
                db.Entry(tci).State = EntityState.Modified;
                db.SaveChanges();
                //send them packing to the calorie count they were on
                return RedirectToAction("Details", new { id = item.DailyCalorieCountId });
            }

            //ViewBag.Name = db.CalorieItems.Find(item.TempCalorieItemId);

            return View(item);
        }
        


        public ActionResult DeleteCalorieItem(int id)
        {
            int i = db.CalorieItemIntakes.Find(id).DailyCalorieCountId;

            CalorieItemIntake c =  db.CalorieItemIntakes.Find(id);

            db.CalorieItemIntakes.Remove(c);

            db.SaveChanges();

            return RedirectToAction("Details", new { id = i });
        }

        public ActionResult DeleteTempItem(int id)
        {
            TempCalorieItem tci = db.TempCalorieItems.Find(id);
            int i = tci.DailyCalorieCountId;

            db.TempCalorieItems.Remove(tci);

            db.SaveChanges();

            return RedirectToAction("Details", new { id = i });
        }

        public ActionResult DeleteDailyCount(int id)
        {
            List<CalorieItemIntake> intake = db.CalorieItemIntakes.Where(i => i.DailyCalorieCountId == id).ToList();
            foreach(CalorieItemIntake c in intake)
            {
                db.CalorieItemIntakes.Remove(c);
            }
            DailyCalorieCount dc = db.DailyCalorieCounts.Find(id);

            db.DailyCalorieCounts.Remove(dc);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
