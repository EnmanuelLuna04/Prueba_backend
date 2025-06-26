using Microsoft.EntityFrameworkCore;
using Bacnkend.Models;

namespace Bacnkend.Data
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Clave única para número de cuenta
            modelBuilder.Entity<BankAccount>()
                .HasIndex(b => b.AccountNumber)
                .IsUnique();

            // Relación 1:N: Cliente -> Cuentas
            modelBuilder.Entity<Client>()
                .HasMany(c => c.BankAccounts)
                .WithOne(b => b.Client)
                .HasForeignKey(b => b.ClientId);

            // Relación 1:N: Cuenta -> Transacciones
            modelBuilder.Entity<BankAccount>()
                .HasMany(b => b.Transactions)
                .WithOne(t => t.BankAccount)
                .HasForeignKey(t => t.BankAccountId);
        }
    }
}
