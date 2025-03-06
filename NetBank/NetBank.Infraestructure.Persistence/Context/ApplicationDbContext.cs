using Microsoft.EntityFrameworkCore;
using NetBank.Core.Domain.Entities;

namespace NetBank.Infraestructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } 

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Beneficiarie> Beneficiaries { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            #region

            modelBuilder.Entity<User>().ToTable("Usuarios");
            modelBuilder.Entity<Product>().ToTable("Productos");
            modelBuilder.Entity<Beneficiarie>().ToTable("Beneficiarios");
            modelBuilder.Entity<Transaction>().ToTable("Transacciones");

            #endregion

            #region Primary Keys and AutoIncrement

            modelBuilder.Entity<User>(x =>
            {
                x.HasKey(k => k.Id);
                x.Property(k => k.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Product>(x =>
            {
                x.HasKey(k => k.Id);
                x.Property(k => k.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Beneficiarie>(x =>
            {
                x.HasKey(k => k.Id);
                x.Property(k => k.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Transaction>(x =>
            {
                x.HasKey(k => k.Id);
                x.Property(k => k.Id).ValueGeneratedOnAdd();
            });

            #endregion


            #region Configuration properties

            #region Users
            modelBuilder.Entity<User>(config =>
            {
                config.HasIndex(k => k.Email).IsUnique();
                config.HasIndex(k => k.Identification).IsUnique();

                config.Property(k => k.InitialAmount).HasColumnType("decimal(14,4)");

                config.Property(k => k.UserType).HasConversion<byte>();

            });
            #endregion

            #region Product
            modelBuilder.Entity<Product>(config =>
            {

                config.Property(k => k.Balance).HasColumnType("decimal(14,4)");
                config.Property(k => k.CreditLimit).HasColumnType("decimal(14,4)");

                config.Property(k => k.ProductType).HasConversion<byte>();

            });
            #endregion

            #region Transaction
            modelBuilder.Entity<Transaction>(config =>
            {

                config.Property(k => k.Amount).HasColumnType("decimal(14,4)");
                config.Property(k => k.TransactionType).HasConversion<byte>();

            });
            #endregion

            #endregion


            #region Foreign keys

            modelBuilder.Entity<User>(config =>
            {

                config.HasMany<Beneficiarie>(x => x.Beneficiaries)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);


                config.HasMany<Product>(x => x.Products)
               .WithOne(u => u.User)
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            });


            modelBuilder.Entity<Product>(config =>
            {

                config.HasMany<Transaction>(x => x.OriginTransactions)
                .WithOne(u => u.OriginProduct)
                .HasForeignKey(u => u.OriginProductId)
                .OnDelete(DeleteBehavior.Restrict);


                config.HasMany<Transaction>(x => x.DestinationTransactions)
               .WithOne(u => u.DestinationProduct)
               .HasForeignKey(u => u.DestinationProductId)
               .OnDelete(DeleteBehavior.Restrict);

            });

            #endregion

        }

    }
}
