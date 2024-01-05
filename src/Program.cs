using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

using ContosoPizza.Data;
using ContosoPizza.Models;

namespace ContosoPizza
{
    // https://stackoverflow.com/questions/48363173/how-to-allow-migration-for-a-console-application/48372990#48372990
    class Program : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION") ?? throw new ArgumentException("SQL_CONNECTION");
            return new ApplicationDbContext(connectionString);
        }
        private static void Main(string[] args)
        {
            Program program = new Program();
            var context = program.CreateDbContext(args);

            Product veggieSpecial = new Product()
            {
                Name = "Veggie Special Pizza",
                Price = 9.99M
            };
            context.Add(veggieSpecial);
            context.SaveChanges();

            Product deluxeMeat = new Product()
            {
                Name = "Deluxe Meat Pizza",
                Price = 12.99M
            };
            context.Add(deluxeMeat);
            context.SaveChanges();

            var products = context.Products
                                .Where(p => p.Price > 10.00M)
                                .OrderBy(p => p.Name).ToList();

            foreach (Product p in products)
            {
                Console.WriteLine($"Id:    {p.Id}");
                Console.WriteLine($"Name:  {p.Name}");
                Console.WriteLine($"Price: {p.Price}");
                Console.WriteLine(new string('-', 20));
            }
        }
    }
}
