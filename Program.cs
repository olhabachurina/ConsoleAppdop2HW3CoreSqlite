using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using static ConsoleAppdop2HW3CoreSqlite.Program;

namespace ConsoleAppdop2HW3CoreSqlite;

class Program
{
  static void Main()
    {
        //InitializeDatabase();
        //AddData();
        //DisplayData();
        //DeleteUser(2);
        //AddProduct("Smartphone", 800.00);
        //DisplayData();
    }

    private static void AddProduct(string name, double price)
    {
        using (var context = new ApplicationContext())
        {
            var product = new Product { Name = name, Price = price };
            context.Products.Add(product);
            context.SaveChanges();
            Console.WriteLine($"Product '{name}' with price ${price} has been added.");
        }
    }

    private static void DeleteUser(int userId)
    {
        using (var context = new ApplicationContext())
        {
            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                
                var ordersToDelete = context.Orders.Where(o => o.UserId == userId).ToList();
                if (ordersToDelete.Any())
                {
                    context.Orders.RemoveRange(ordersToDelete);
                }

                context.Users.Remove(user);
                context.SaveChanges();
                Console.WriteLine($"User with ID {userId} has been deleted.");
            }
            else
            {
                Console.WriteLine($"User with ID {userId} not found.");
            }
        }
    }

    private static void DisplayData()
    {
        using (var context = new ApplicationContext())
        {
            var users = context.Users.ToList();
            Console.WriteLine("Users:");
            foreach (var user in users)
            {
                Console.WriteLine($"- {user.Name} ({user.Email})");
            }

            var products = context.Products.ToList();
            Console.WriteLine("\nProducts:");
            foreach (var product in products)
            {
                Console.WriteLine($"- {product.Name}: ${product.Price}");
            }

            var orders = context.Orders.Include(o => o.User).Include(o => o.Product).ToList();
            Console.WriteLine("\nOrders:");
            foreach (var order in orders)
            {
                Console.WriteLine($"- {order.User.Name} ordered {order.Product.Name}");
            }
        }
    }

    private static void AddData()
    {
        using (var context = new ApplicationContext())
        {
            var user = new User { Name = "Alice", Email = "alice@ukr.net" };
            context.Users.Add(user);

            var product = new Product { Name = "Laptop", Price = 1200.00 };
            context.Products.Add(product);

            context.SaveChanges();

            context.Orders.Add(new Order { UserId = user.Id, ProductId = product.Id });

            context.SaveChanges();
        }
    }

    private static void InitializeDatabase()
    {
        using (var context = new ApplicationContext())
        {
            
            context.Database.Migrate();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string dbPath = @"C:\Users\Acer\Desktop\MyDatabase.db";
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}

