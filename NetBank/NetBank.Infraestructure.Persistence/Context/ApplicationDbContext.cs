using Microsoft.EntityFrameworkCore;
using NetBank.Core.Domain.Entities;

namespace NetBank.Infraestructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } 

        public DbSet<Product> Products { get; set; }

        public DbSet<Beneficiarie> Beneficiaries { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            #region tables names

            modelBuilder.Entity<Product>().ToTable("Productos");
            modelBuilder.Entity<Beneficiarie>().ToTable("Beneficiarios");
            modelBuilder.Entity<Transaction>().ToTable("Transacciones");
            modelBuilder.Entity<Payment>().ToTable("Pagos");

            #endregion

            #region Primary Keys and AutoIncrement


            modelBuilder.Entity<Product>(x =>
            {
                x.HasKey(x => x.AccountNumber);
                x.Property(K => K.AccountNumber).UseIdentityColumn(780000001, 1);

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

            modelBuilder.Entity<Payment>(x =>
            {
                x.HasKey(k => k.Id);
                x.Property(k => k.Id).ValueGeneratedOnAdd();
            });

            #endregion


            #region Configuration properties



            #region Product
            modelBuilder.Entity<Product>(config =>
            {

                config.Property(k => k.Balance).HasColumnType("decimal(14,4)");
                config.Property(k => k.CreditLimit).HasColumnType("decimal(14,4)");
                config.Property(k => k.AmountOwed).HasColumnType("decimal(14,4)");

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

            #region Beneficiarie
            modelBuilder.Entity<Beneficiarie>(config =>
            {
                config.Property(k => k.AccountNumber).HasColumnType("Int");
            });
            #endregion

            #region Payment

            modelBuilder.Entity<Payment>(config =>
            {

                config.Property(k => k.Amonut).HasColumnType("decimal(14,4)");
                config.Property(k => k.PaymentType).HasConversion<byte>();

            });

            #endregion

            #endregion


            #region Foreign keys


            modelBuilder.Entity<Product>(config =>
            {

                config.HasMany<Transaction>(x => x.OriginTransactions)
                .WithOne(u => u.OriginProduct)
                .HasForeignKey(u => u.OriginProductId)
                .OnDelete(DeleteBehavior.Cascade);


                config.HasMany<Transaction>(x => x.DestinationTransactions)
               .WithOne(u => u.DestinationProduct)
               .HasForeignKey(u => u.DestinationProductId)
               .OnDelete(DeleteBehavior.NoAction);

            });



            modelBuilder.Entity<Product>(config =>
            {

                config.HasMany<Payment>(x => x.OriginPayments)
                .WithOne(u => u.OriginProduct)
                .HasForeignKey(u => u.OriginAccountNumber)
                .OnDelete(DeleteBehavior.NoAction);


                config.HasMany<Payment>(x => x.DestinationPayments)
               .WithOne(u => u.DestinationProduct)
               .HasForeignKey(u => u.DestinationAccountNumber)
               .OnDelete(DeleteBehavior.NoAction);

            });


            #endregion

        }

    }
}
