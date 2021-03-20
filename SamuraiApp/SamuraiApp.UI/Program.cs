using Microsoft.EntityFrameworkCore;
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
            AddSamurai("Bri","Alex");
            GetSamurais();
            Console.Write("Press any key...");
            Console.ReadKey();
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
    }
}
