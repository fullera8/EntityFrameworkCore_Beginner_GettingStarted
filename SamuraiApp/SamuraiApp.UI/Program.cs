using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void Main(string[] args)
        {
            _context.Database.EnsureCreated();
            GetSamurais("Before");
            AddSamurai();
            GetSamurais("After");
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        /// <summary>
        /// Add a new Samurai named "Sampson" to the Samurai DB
        /// </summary>
        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "Alex" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        /// <summary>
        /// Get all Samurai from the DB returning their count and names
        /// </summary>
        /// <param name="text">Custom text to put befroe the samurai count</param>
        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach(var samurai in samurais)
                Console.WriteLine(samurai.Name);
        }
    }
}
