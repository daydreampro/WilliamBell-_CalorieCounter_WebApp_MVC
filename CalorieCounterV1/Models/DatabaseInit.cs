using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web;
using System.Data.Entity.Validation;

namespace CalorieCounterV1.Models
{
    public class DatabaseInit: DropCreateDatabaseAlways<CalorieCounterDbContext>
    {
        
        protected override void Seed(CalorieCounterDbContext context)
        {
            if (!context.Users.Any())
            {
                /////////////////////////////////////////////////////
                ///         roles                    \\\\\\\\\\\\
                ////////////////////////////////////////////////////
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //if the admin role does not exist
                if (!roleManager.RoleExists("Admin"))
                {
                    //create admin
                    roleManager.Create(new IdentityRole("Admin"));
                }

                //if member doesnt exist
                if (!roleManager.RoleExists("Member"))
                {
                    roleManager.Create(new IdentityRole("Member"));
                }

                context.SaveChanges();

                

                //create users-------------------

                UserManager<User> userManager = new UserManager<User>(new UserStore<User>(context));

                /////////////////////////////////////////////////////
                ///         admin                        \\\\\\\\\\\\
                ////////////////////////////////////////////////////
                if (userManager.FindByName("admin@ecc.co.uk") == null)
                {
                    userManager.PasswordValidator = new PasswordValidator()
                    {
                        RequireDigit = false,
                        RequiredLength = 1,
                        RequireLowercase = false,
                        RequireUppercase = false,
                        RequireNonLetterOrDigit = false
                    };

                    var admin = new User()
                    {
                        UserName = "admin@ecc.co.uk",
                        Email = "admin@ecc.co.uk",
                        FirstName = "William",
                        LastName = "Bell",
                        Gender = Gender.Male,
                        EmailConfirmed = true,
                        RegisteredAt = new DateTime(2020, 1, 3, 16, 30, 10)
                    };

                    userManager.Create(admin, "admin123");

                    userManager.AddToRole(admin.Id, "Admin");

                    /////////////////////////////////////////////////////
                    ///         Users                         \\\\\\\\\\\\
                    ////////////////////////////////////////////////////
                    var member1 = new User()
                    {
                        UserName = "member1@ecc.com",
                        Email = "member1@ecc.com",
                        FirstName = "Paul",
                        LastName = "Blart",
                        Gender = Gender.Male,
                        RegisteredAt = new DateTime(2020, 1, 3, 16, 30, 10),
                        EmailConfirmed = true
                    };

                    if (userManager.FindByName("member1@ecc.com") == null)
                    {
                        userManager.Create(member1, "123");

                        userManager.AddToRole(member1.Id, "Member");
                    }

                    var member2 = new User()
                    {
                        UserName = "member2@ecc.com",
                        Email = "member2@ecc.com",
                        FirstName = "Ania",
                        LastName = "Kochi",
                        Gender = Gender.Female,
                        RegisteredAt = new DateTime(2020, 1, 3, 16, 30, 10),
                        EmailConfirmed = true
                    };

                    if (userManager.FindByName("member2@ecc.com") == null)
                    {
                        userManager.Create(member2, "321");

                        userManager.AddToRole(member2.Id, "Member");
                    }

                    context.SaveChanges();


                    /////////////////////////////////////////////////////
                    ///         category                    \\\\\\\\\\\\
                    ////////////////////////////////////////////////////
                    ///

                    var cat1 = new Category() { Name = "Fruit" };
                    var cat2 = new Category() { Name = "Vegetables" };
                    var cat3 = new Category() { Name = "Grains" };
                    var cat4 = new Category() { Name = "Protein" };
                    var cat5 = new Category() { Name = "Dairy" };
                    var cat6 = new Category() { Name = "Drinks" };

                    context.Categories.Add(cat1);
                    context.Categories.Add(cat2);
                    context.Categories.Add(cat3);
                    context.Categories.Add(cat4);
                    context.Categories.Add(cat5);
                    context.Categories.Add(cat6);

                    context.SaveChanges();


                    /////////////////////////////////////////////////////
                    ///         calorie items                 \\\\\\\\\\\\
                    ////////////////////////////////////////////////////
                    ///

                    var item1 = new CalorieItem()
                    {
                        Name = "Apple",
                        Calories = 52,
                        Carbs = 14,
                        Protein = .3,
                        Fat = .2,
                        ServingSize = 100,
                        Category = cat1,
                        ImagePath = "/Content/Images/CalorieItemImages/apple.jpg"
                    };
                    var item2 = new CalorieItem()
                    {
                        Name = "Carrot",
                        Calories = 41,
                        Carbs = 9.6,
                        Protein = .9,
                        Fat = .2,
                        ServingSize = 61,
                        Category = cat2,
                        ImagePath = "/Content/Images/CalorieItemImages/carrot.jpg"
                    };
                    var item4 = new CalorieItem()
                    {
                        Name = "Beef Tenderloin",
                        Calories = 324,
                        Carbs = 0,
                        Protein = 24,
                        Fat = 25,
                        ServingSize = 100,
                        Category = cat4,
                        ImagePath = "/Content/Images/CalorieItemImages/tenderloin.jpg"
                    };
                    var item3 = new CalorieItem()
                    {
                        Name = "Pasta",
                        Calories = 131,
                        Carbs = 25,
                        Protein = 5,
                        Fat = 1.1,
                        ServingSize = 100,
                        Category = cat3,
                        ImagePath = "/Content/Images/CalorieItemImages/pasta.jpg"
                    };
                    var item5 = new CalorieItem()
                    {
                        Name = "Cheddar Cheese",
                        Calories = 402,
                        Carbs = 1.3,
                        Protein = 25,
                        Fat = 33,
                        ServingSize = 100,
                        Category = cat5,
                        ImagePath = "/Content/Images/CalorieItemImages/cheddar_cheese.jpg"
                    };
                    var item6 = new CalorieItem()
                    {
                        Name = "Oats",
                        Calories = 50,
                        Carbs = 11,
                        Protein = 1.4,
                        Fat = .2,
                        ServingSize = 100,
                        Category = cat3,
                        ImagePath = "/Content/Images/CalorieItemImages/oats.jpg"
                    };

                    var item7 = new CalorieItem()
                    {
                        Name = "Coffee",
                        Calories = 15,
                        Carbs = .3,
                        Protein = .5,
                        Fat = .1,
                        ServingSize = 100,
                        Category = cat6,
                        ImagePath = "/Content/Images/CalorieItemImages/coffee_img.jpg"

                    };

                    context.CalorieItems.Add(item1);
                    context.CalorieItems.Add(item2);
                    context.CalorieItems.Add(item3);
                    context.CalorieItems.Add(item4);
                    context.CalorieItems.Add(item5);
                    context.CalorieItems.Add(item6);
                    context.CalorieItems.Add(item7);

                    context.SaveChanges();


                    /////////////////////////////////////////////////////
                    ///         calorie counts                \\\\\\\\\\\\
                    ////////////////////////////////////////////////////
                    ///

                    var count1 = new DailyCalorieCount()
                    {
                        Date = new DateTime(2020, 1, 1),
                        Name = "Fatty Day",
                        TotalCalories = 0,
                        User = member1
                    };
                    var count2 = new DailyCalorieCount()
                    {
                        Date = new DateTime(2020, 2, 4),
                        Name = "Training Day",
                        TotalCalories = 0,
                        User = member1
                    };
                    var count3 = new DailyCalorieCount()
                    {
                        Date = new DateTime(2020, 4, 12),
                        Name = "Leg Day",
                        TotalCalories = 0,
                        User = member1
                    };
                    var count4 = new DailyCalorieCount()
                    {
                        Date = new DateTime(2020, 5, 1),
                        Name = "I was Sad! OKAY!",
                        TotalCalories = 0,
                        User = member2
                    };

                    context.DailyCalorieCounts.Add(count1);
                    context.DailyCalorieCounts.Add(count2);
                    context.DailyCalorieCounts.Add(count3);
                    context.DailyCalorieCounts.Add(count4);

                    context.SaveChanges();

                    /////////////////////////////////////////////////////
                    ///         calorie items intakes           \\\\\\\\\\\\
                    ////////////////////////////////////////////////////
                    ///

                    var in1 = new CalorieItemIntake()
                    {
                        Quantity = 140,
                        CalorieItem = item1,
                        DailyCalorieCount = count1,
                        
                    };
                    var in2 = new CalorieItemIntake()
                    {
                        Quantity = 55,
                        CalorieItem = item2,
                        DailyCalorieCount = count1
                    };
                    var in3 = new CalorieItemIntake()
                    {
                        Quantity = 200,
                        CalorieItem = item4,
                        DailyCalorieCount = count1
                    };
                    var in4 = new CalorieItemIntake()
                    {
                        Quantity = 140,
                        CalorieItem = item5,
                        DailyCalorieCount = count1

                    };

                    var in5 = new CalorieItemIntake()
                    {
                        Quantity = 140,
                        CalorieItem = item1,
                        DailyCalorieCount = count2,

                    };
                    var in6 = new CalorieItemIntake()
                    {
                        Quantity = 55,
                        CalorieItem = item2,
                        DailyCalorieCount = count2
                    };
                    var in7 = new CalorieItemIntake()
                    {
                        Quantity = 200,
                        CalorieItem = item4,
                        DailyCalorieCount = count3
                    };
                    var in8 = new CalorieItemIntake()
                    {
                        Quantity = 140,
                        CalorieItem = item5,
                        DailyCalorieCount = count3

                    };
                    var in9 = new CalorieItemIntake()
                    {
                        Quantity = 140,
                        CalorieItem = item1,
                        DailyCalorieCount = count4,

                    };
                    var in10 = new CalorieItemIntake()
                    {
                        Quantity = 55,
                        CalorieItem = item2,
                        DailyCalorieCount = count4
                    };
                    var in11 = new CalorieItemIntake()
                    {
                        Quantity = 200,
                        CalorieItem = item4,
                        DailyCalorieCount = count2
                    };
                    var in12 = new CalorieItemIntake()
                    {
                        Quantity = 140,
                        CalorieItem = item5,
                        DailyCalorieCount = count3

                    };
                    context.CalorieItemIntakes.Add(in1);
                    context.CalorieItemIntakes.Add(in2);
                    context.CalorieItemIntakes.Add(in3);
                    context.CalorieItemIntakes.Add(in4);
                    context.CalorieItemIntakes.Add(in5);
                    context.CalorieItemIntakes.Add(in6);
                    context.CalorieItemIntakes.Add(in7);
                    context.CalorieItemIntakes.Add(in8);
                    context.CalorieItemIntakes.Add(in9);
                    context.CalorieItemIntakes.Add(in10);
                    context.CalorieItemIntakes.Add(in11);
                    context.CalorieItemIntakes.Add(in12);

                    context.SaveChanges();

                    context.SaveChanges();


                    //////////////////////////////////
                    ///podcasts//////////////////
                    ///

                    var pod1 = new Podcast()
                    {
                        Title = "Introduction",
                        EpisodeNumber = 1,
                        SeasonNumber = 1,
                        Description = "Our debut podcast episode, in this episode we discus the basic principles of eating healthily" +
                        "and why you should plan and regulate your diet.",
                        Date = new DateTime(2020, 1, 5),
                        AudioPath = "/Content/Podcasts/5_2_Diet_Podcast_Ep_1_Introducing_5_2_December_2014.mp3",
                        ImagePath = "/Content/Images/podcast_cat.jpg"

                    };
                    var pod2 = new Podcast()
                    {
                        Title = "Planning your first fast",
                        EpisodeNumber = 2,
                        SeasonNumber = 1,
                        Description = "Our second episode, in this episode we discus how to start your journet to better and healthier eating. Starting with" +
                        "eveyones favourite idea, FASTING!",
                        Date = new DateTime(2020, 1, 7),
                        AudioPath = "/Content/Podcasts/5_2_Diet_Podcast_Ep_2_Planning_for_your_first_fast.mp3",
                        ImagePath = "/Content/Images/podcast_cat.jpg"
                    };

                    context.Podcasts.Add(pod1);
                    context.Podcasts.Add(pod2);

                    context.SaveChanges();
                    

                    /////////////////////
                    //features
                    ////////
                    ///
                    var f1 = new Feature()
                    {
                        Date = new DateTime(2020,1,4),
                        Description = "Count Calories",
                        ImagePath = "/Content/Images/Features/calories_count.jpg",
                        Link = "https://localhost:44342/Home/FoodIndex",
                        IsFeature = true
                    };

                    var f2 = new Feature()
                    {
                        Date = new DateTime(2019, 12, 28),
                        ImagePath = "/Content/Images/Features/kids_eat_food.jpg",
                        Description = "Kids eat food",
                        Link = "https://localhost:44342/Podcast",
                        IsFeature = true
                    };

                    var f4 = new Feature()
                    {
                        Date = new DateTime(2019, 5, 5),
                        ImagePath = "/Content/Images/Features/this_thing.png",
                        Description = "How to drop food, properly!",
                        Link = "https://localhost:44342/Home/UnderstandingCalories",
                        IsFeature = true
                    };

                    var f3 = new Feature()
                    {
                        Date = new DateTime(2019, 12, 28),
                        ImagePath = "/Content/Images/Features/buffet_food.jpg",
                        Description = "BUFFETS!",
                        Link = "https://localhost:44342/Home/Contact",
                        IsFeature =false
                    };

                    context.Features.Add(f1);
                    context.Features.Add(f2);
                    context.Features.Add(f3);
                    context.Features.Add(f4);

                    context.SaveChanges();


                    // requests

                    var r = new TempCalorieItem()
                    {
                        Calories = 329,
                        Carbs = 2.8,
                        Protein = 10,
                        Fat = 8.6,
                        ServingSize = 100,
                        Name = "Hamburger",
                        SendSugestion = true,
                        DailyCalorieCount = count1
                    };

                    var r2 = new TempCalorieItem()
                    {
                        Calories = 329,
                        Protein = 10,
                        Fat = 8.6,
                        ServingSize = 100,
                        Name = "Pizza",
                        SendSugestion = true,
                        DailyCalorieCount = count1
                    };

                    context.TempCalorieItems.Add(r);
                    context.TempCalorieItems.Add(r2);


                    foreach (var i in context.DailyCalorieCounts)
                    {
                        List<CalorieItemIntake> calorieItemIntakes = new List<CalorieItemIntake>();

                        if (i.CalorieItemIntakes.ToList() != null)
                        {
                            calorieItemIntakes = i.CalorieItemIntakes.ToList();
                        }

                        if (calorieItemIntakes != null && calorieItemIntakes.Count > -1)
                        {
                            i.TotalCalories = Calculator.GrandTotal(calorieItemIntakes)
                                + Calculator.GrandTotal(new List<CalorieItemIntake>()
                            {
                                    new CalorieItemIntake()
                                    {
                                        CalorieItem = new CalorieItem()
                                        {
                                            Name = r.Name,
                                            Calories = r.Calories,
                                            ServingSize = r.ServingSize
                                        },
                                        Quantity = r.ServingSize
                                    },
                                    new CalorieItemIntake()
                                    {
                                        CalorieItem = new CalorieItem()
                                        {
                                            Name = r2.Name,
                                            Calories = r2.Calories,
                                            ServingSize = r2.ServingSize
                                        },
                                        Quantity = r.ServingSize
                                    }
                            });
                        }

                    }

                    context.SaveChanges();


                    
                    /*try
                    {
                        context.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }*/



                }

            }
        }
    }
}
 