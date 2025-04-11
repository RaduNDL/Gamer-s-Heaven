using Microsoft.EntityFrameworkCore;
using PAW.Models;

namespace PAW.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public ApplicationDbContext() { }

        public DbSet<Game> Games { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Game relationships
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Reviews)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameID);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.CartItems)
                .WithOne(ci => ci.Game)
                .HasForeignKey(ci => ci.GameID);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.OrderItems)
                .WithOne(oi => oi.Game)
                .HasForeignKey(oi => oi.GameID);

            modelBuilder.Entity<Game>()
                .HasMany(g => g.WishListItems)
                .WithOne(wli => wli.Game)
                .HasForeignKey(wli => wli.GameID);

            // Wishlist relationships
            modelBuilder.Entity<WishList>()
                .HasMany(wl => wl.WishListItems)
                .WithOne(wli => wli.WishList)
                .HasForeignKey(wli => wli.WhishListID);

            // ShoppingCart relationships
            modelBuilder.Entity<ShoppingCart>()
                .HasMany(sc => sc.CartItems)
                .WithOne(ci => ci.ShoppingCart)
                .HasForeignKey(ci => ci.ShoppingCartID);

            // Order relationships
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderID);

            // Price precision
            modelBuilder.Entity<Game>()
                .Property(g => g.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(10, 2);

            // GameTitle property on WishListItem
            modelBuilder.Entity<WishListItem>()
                .Property(wli => wli.GameTitle)
                .HasMaxLength(255); // Optional, but recommended
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=GamersHeavenDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}
