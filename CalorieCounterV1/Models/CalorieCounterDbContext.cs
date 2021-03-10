using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CalorieCounterV1.Models
{
    public class CalorieCounterDbContext:IdentityDbContext<User>
    {
        //database tables
        public DbSet<CalorieItem> CalorieItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CalorieItemIntake> CalorieItemIntakes { get; set; }
        public DbSet<DailyCalorieCount> DailyCalorieCounts { get; set; }
        public DbSet<Podcast> Podcasts { get; set; }
        public DbSet<TempCalorieItem> TempCalorieItems { get; set; }

        public DbSet<Feature> Features { get; set; }
        public CalorieCounterDbContext() : base("CalorieCounterV1Connection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DatabaseInit());
            //Database.SetInitializer(new DropCreateDatabaseAlways<CalorieCounterDbContext>());
        }

        public static CalorieCounterDbContext Create()
        {
            return new CalorieCounterDbContext();
        }
    
    }
}