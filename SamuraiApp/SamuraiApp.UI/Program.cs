using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        private static SamuraiContextNoTracking _contextNT = new SamuraiContextNoTracking();

        private static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            //AddSamurai("Bri3","Alex3", "Ashlyn", "Wes");

            //AddVariousTypes();
            //GetSamurais();
            //QueryFilters();
            //QueryAggregates();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurai();
            //MultipleDatabaseOperations();
            //DeleteSamurai();
            //QueryAndUpdateBattles_disconnected();
            //QueryFiltersNT();
            //InsertNewSamuraiWithAQuote();
            //InsertNewSamuraiWithManyQuotes();
            //AddQuoteToExistingSamuraiWhileTracked();
            //AddQuoteToExistingSamuraiWhileNotTracked(2);
            //EagerLoadSamuraiWithQuotes();
            //ProjectLimitedProperties();
            //ExplicitLoadFromMemory();
            //LazyLoadQuotes();
            //ModifyRelatedDataWhenTracked();
            //ModifyRelatedDataWhenNotTracked();

            //Console.Write("Press any key...");
            //Console.ReadKey();

            //AddNewSamuraiToExistingBattle();
            //ReturnBattleWithSamurai();
            //AddAllSamuraiToAllBattles();
            //ReturnBattleWithManySamurai();
            //RemoveSamuraiFromBattle();
            //RemoveSamuraiFromBattleExplicit();
            //AddNewSamuraiWithHorse();
            //AddNewHorseToExistingSamurai();
            //GetHorseWithSamurai();
            //QuerySamuraiBattleStats();
            
            QueryRawSql();
            QueryRawIntrepolationSql();
        }

        /// <summary>
        /// Add a new Samurai named "Sampson" to the Samurai DB
        /// </summary>
        /// <param name="names">array of names that will createa new Samurai for each</param>
        private static void AddSamurai(params string[] names)
        {
            foreach (var name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }

        private static void AddVariousTypes()
        {
            _context.AddRange(
                new Samurai { Name = "Taylor" },
                new Samurai { Name = "Hannah" },
                new Battle { Name = "Battle of Lincoln" },
                new Battle { Name = "Battle of NASA" }
            );
            _context.SaveChanges();
        }

        private static void QueryFilters()
        {
            var name = "Alex2";
            var likeName = "A";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, $"{likeName}%")).ToList();
        }

        private static void QueryFiltersNT()
        {
            var name = "Alex2";
            var likeName = "A";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            var samurais = _contextNT.Samurais.Where(s => EF.Functions.Like(s.Name, $"{likeName}%")).ToList();
        }

        private static void QueryAggregates()
        {
            var name = "Alex2";
            //var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);//LINQ special syntax for where top 1
            var samurai = _context.Samurais.Find(2);//Finds objects stored in memory so we do not need to query DB.
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault(); //Get first samurai 
            samurai.Name += "San"; //Append San to the end of their name
            _context.SaveChanges(); //Save that new name
        }

        private static void RetrieveAndUpdateMultipleSamurai()
        {
            var samurais = _context.Samurais.Skip(1).Take(7).ToList();//LINQ to get the 2-8 values in the Samurai table
            samurais.ForEach(s => s.Name += "San"); //Append the "San" to the end of each Name
            _context.SaveChanges(); //Update the values
        }

        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.Samurais.Add(new Samurai { Name = "Shino" });
            _context.SaveChanges();
        }

        private static void DeleteSamurai()
        {
            var samurai = _context.Samurais.Find(8); //Get first samurai 
            _context.Samurais.Remove(samurai); //Append San to the end of their name
            _context.SaveChanges(); //Save that new name
        }

        
        /// <summary>
        /// Updates the dates of battles locally by first getting the context, manipulating the data
        /// and opening a second context witht the new data and saving it. Important to note this will only 
        /// work with context classes. Working with a class directly will open and update the db on the fly
        /// </summary>
        private static void QueryAndUpdateBattles_disconnected()
        {
            List<Battle> diconnectedBattles;
            using (var context1 = new SamuraiContext())
                {
                    diconnectedBattles = _context.Battles.ToList();
                }
            diconnectedBattles.ForEach(b =>
                {
                    b.StartDate = new DateTime(1570, 01, 01);
                    b.EndDate = new DateTime(1570, 12, 01);
                });
            using (var context2 = new SamuraiContext())
                {
                    context2.UpdateRange(diconnectedBattles);
                    context2.SaveChanges();
                }

        }

        /// <summary>
        /// Get all Samurai from the DB returning their count and names
        /// </summary>
        private static void GetSamurais()
        {
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"Samurai count is {samurais.Count}");
            foreach(var samurai in samurais)
                Console.WriteLine(samurai.Name);
        }

        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Shiba Inu",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "Dogo to the rescue!" }
                }
            };

            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kubo",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "I am well animated!" },
                    new Quote { Text = "All will fear my 24 frames per second." }
                }
            };

            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "You are saved."
            });

            _context.SaveChanges();
        }

        private static void AddQuoteToExistingSamuraiWhileNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                //Text = "What's for dinner?"
                Text = "Thanks for dinner!"
            });

            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            }
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            //var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
            //var splitQuery = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
            //var filterInclude = _context.Samurais
            //    .Include(s => s.Quotes //Include returns the whole set, then you filter from there.
            //        .Where(q => q.Text
            //            .Contains("Thanks")))
            //    .ToList();

            //var filterOnlyOneValue = _context.Samurais
            //    .Where(s => s.Quotes //Where filters on the db level. Recommended approach for data filter efficency.
            //        .Any(q => q.Text
            //            .Contains("Thanks")))
            //    .ToList();

            var filterPrimaryEntityAndInclude = _context.Samurais
                .Where(s => s.Name.Contains("Alex"))
                    .Include(s => s.Quotes)
                .FirstOrDefault();
            //Where(i => i.Quotes.Any(j => j.Text.Contains("term")))
        }

        private static void ProjectLimitedProperties()
        {
            //var limitedProperties = _context.Samurais.Select(s => 
            //    new { 
            //            s.Id, 
            //            s.Name, 
            //        }
            //    ).ToList();

            //Filters and creates custom query directly in SQL.
            var customProperties = _context.Samurais.Select(s =>
                new 
                {
                    //s.Id,
                    //s.Name,
                    Samurai = s,
                    //NumberOfQuotes = s.Quotes.Count
                    dinnerQuotes = s.Quotes.Where(q => q.Text.Contains("Dinner"))
                }
                ).ToList();
        }

        private static void ExplicitLoadFromMemory()
        {
            _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Bullseye" });
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            ////////////////////////////////////////////////////////////////////////////
            var samurai = _context.Samurais.Find(1); //Get the samurai from DB
            _context.Entry(samurai).Collection(s => s.Quotes).Load(); //Exlpicitly use properties loaded into memory
            _context.Entry(samurai).Reference(s => s.Horse).Load(); //Exlpicitly use references loaded into memory
        }

        private static void LazyLoadQuotes()
        {
            var samurai = _context.Samurais.Find(2);
            var quoteCount = samurai.Quotes.Count(); //Need to enable lazy loading for this to work
            /*
             Steps to enable lazy loading:
                1. Every navigation must be set to virtual
                2. Add the Microsoft.EntityFramwork.Proxies package
                3. DbContext OnConfiguring optionsBuilder.UserLazyLoadingProxies()
             */
        }

        private static void ModifyRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                .FirstOrDefault(s => s.Id == 2);
            samurai.Quotes[0].Text = "Did you hear that?"; //Notice that the Tracked method can modify directly
            _context.Quotes.Remove(samurai.Quotes[2]);
            _context.SaveChanges();
        }

        private static void ModifyRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                .FirstOrDefault(s => s.Id == 2);
            var quote = samurai.Quotes[0];
            quote.Text += " Did you hear that again?";

            using (var newContext = new SamuraiContext())
            {
                //newContext.Quotes.Update(quote); //This will update all quotes because the context had set the entry state to modified before
                newContext.Entry(quote).State = EntityState.Modified; //State must be explicitly state when jumping context.
                newContext.SaveChanges();
            }
        }
    
        /// <summary>
        /// Demo of adding many to many single value. Updates values in Samurai, Battle, and SamuraiBattle tables.
        /// </summary>
        private static void AddNewSamuraiToExistingBattle()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.Samurais.Add(new Samurai { Name = "Seto Kaiba" });
            _context.SaveChanges();
        }

        private static void ReturnBattleWithSamurai()
        {
            var battle = _context.Battles.Include(b => b.Samurais).FirstOrDefault();
        }

        private static void ReturnBattleWithManySamurai()
        {
            var battle = _context.Battles.Include(b => b.Samurais).ToList();
        }

        private static void AddAllSamuraiToAllBattles()
        {
            var battles = _context.Battles.Include(b => b.Samurais).ToList();
            var samurais = _context.Samurais.ToList();

            foreach (var battle in battles)
            {
                battle.Samurais.AddRange(samurais);
            }

            _context.SaveChanges();
        }

        private static void RemoveSamuraiFromBattle()
        {
            var battleWithSamurai = _context.Battles
                    .Include(b => b.Samurais
                        .Where(s => s.Id == 12))
                    .Single(s => s.BattleId == 1);
            var samurai = battleWithSamurai.Samurais[0];
            
            battleWithSamurai.Samurais.Remove(samurai);
            
            _context.SaveChanges();
        }

        private static void RemoveSamuraiFromBattleExplicit()
        {
            var battleWithSamurai = _context.Set<BattleSamurai>()
                .SingleOrDefault(bs => bs.BattleId == 1 && bs.SamuraiId == 10);

            if (battleWithSamurai != null)
            {
                _context.Remove(battleWithSamurai);
                _context.SaveChanges();
            }
        }

        private static void AddNewSamuraiWithHorse()
        {
            var samurai = new Samurai { Name = "Joey Wheeler" };
            samurai.Horse = new Horse { Name = "Baby Dragon" };

            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void AddNewHorseToExistingSamurai()
        {
            var horse = new Horse { Name = "Bullseye", SamuraiId = 2 };

            _context.Add(horse);
            _context.SaveChanges();
        }

        private static void GetHorseWithSamurai()
        {
            var horseOnly = _context.Set<Horse>().Find(3);

            var horseWithSamurai = _context.Samurais.Include(s => s.Horse)
                .FirstOrDefault(s => s.Horse.Id == 3);

            var horseSamuraiPairs = _context.Samurais
                .Where(s => s.Horse != null)
                .Select(s => new { Horse = s.Horse, Samurai = s })
                .ToList();
        }

        private static void QuerySamuraiBattleStats() 
        {
            var stats = _context.SamuraiBattleStats.ToList();
            var firstStat = _context.SamuraiBattleStats.FirstOrDefault();
            var alexStat = _context.SamuraiBattleStats.FirstOrDefault(b => b.Name == "Alex");
        }

        private static void QueryRawSql()
        {
            //Note the SQL commands have been updated to new methods to make the ExecuteSQL command unnecessary. Review EF Core 3.1 for reference to the latest SQL commands
            //This must reflect the object it goes into. AKA no projection unless you create an anonymous object with only those properties.
            var samurais = _context.Samurais.FromSqlRaw("SELECT TOP 10 * FROM samurais").ToList();
        }

        private static void QueryRawIntrepolationSql()
        {
            //Behaves similar to raw sql but strings can be passed in. 
            //IMPORTANT: Use this when passing parameters, if raw sql is used with params it opens the request up to SQL Injection.
            var name = "Alex";
            var samurais = _context.Samurais
                .FromSqlInterpolated($"SELECT TOP 10 * FROM samurais WHERE Name = {name}")
                .ToList();
        }
    }
}
