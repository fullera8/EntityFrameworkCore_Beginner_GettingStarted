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
            EagerLoadSamuraiWithQuotes();
            //Console.Write("Press any key...");
            //Console.ReadKey();
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
    }
}
