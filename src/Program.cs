using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

using ExampleEF.Data;
using ExampleEF.Models;

namespace ExampleEF
{
    class Program : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION")
                ?? throw new ArgumentException("SQL_CONNECTION");

            return new ApplicationDbContext(connectionString);
        }
        private static void Main(string[] args)
        {
            Program program = new();
            var context = program.CreateDbContext(args);

            try
            {
                //context.Database.EnsureCreated();
                context.Database.Migrate();
                SeedData(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Total products: {0}", context.Products.Count());
        }

        static int SeedData(ApplicationDbContext context)
        {
            if (context.Products.Any())
            {
                return -1;   // DB has been seeded
            }
            else
            {
                Console.WriteLine("Seeding the database.");

                var pizza1 = new Product() { Name = "Pepperoni", Price = 12.50M };
                var pizza2 = new Product() { Name = "Margherita", Price = 15.50M };
                var pizza3 = new Product() { Name = "Hawaiian", Price = 17.00M };

                context.Products.Add(pizza1);
                context.Products.Add(pizza2);
                context.Products.Add(pizza3);

                context.SaveChanges();
            }

            var products = context.Products
                                .Where(p => p.Price > 10.00M)
                                .OrderBy(p => p.Name).ToList();

            foreach (Product p in products)
                if (context.Products.Any())
                {
                    Console.WriteLine($"Id:    {p.Id}");
                    Console.WriteLine($"Name:  {p.Name}");
                    Console.WriteLine($"Price: {p.Price}");
                    Console.WriteLine(new string('-', 20));
                }
            return 0;
        }

    }
}
