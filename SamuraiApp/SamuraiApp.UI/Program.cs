﻿using Microsoft.EntityFrameworkCore;
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
            //AddSamurai("Bri3","Alex3", "Ashlyn", "Wes");
            //AddVariousTypes();
            //GetSamurais();
            //QueryFilters();
            QueryAggregates();
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

        private static void QueryAggregates()
        {
            var name = "Alex2";
            //var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);//LINQ special syntax for where top 1
            var samurai = _context.Samurais.Find(2);//Finds objects stored in memory so we do not need to query DB.
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
