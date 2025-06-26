using Bacnkend.Data;
using Bacnkend.Models;
using Bacnkend.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Threading.Tasks;

namespace Bacnkend.Tests
{
    public class TransactionServiceTests
    {
        private BankingContext GetContext()
        {
            var options = new DbContextOptionsBuilder<BankingContext>()
                .UseSqlite("Data Source=:memory:")  // Usamos SQLite en memoria para las pruebas
                .Options;

            var context = new BankingContext(options);
            context.Database.OpenConnection();  // Abre la conexión a la base de datos
            context.Database.EnsureCreated();  // Asegura que la base de datos esté creada

            return context;
        }

        [Fact]
        public async Task CanDepositAndWithdrawUsingService()
        {
            // Arrange
            using var context = GetContext();
            var transactionService = new TransactionService(context);

            var client = new Client
            {
                Name = "Carlos López",
                DateOfBirth = new DateTime(1985, 6, 10),
                Gender = "Masculino",
                Income = 5000
            };

            context.Clients.Add(client);
            context.SaveChanges();

            var bankAccount = new BankAccount
            {
                AccountNumber = "ACC67890",
                Balance = 2000,
                ClientId = client.Id
            };

            context.BankAccounts.Add(bankAccount);
            context.SaveChanges();

            // Act: Usar el servicio para realizar un depósito
            var depositTransaction = await transactionService.DepositAsync(bankAccount.Id, 500);

            // Assert: Verificar el saldo después del depósito
            var savedBankAccount = await context.BankAccounts.FindAsync(bankAccount.Id);
            Assert.NotNull(savedBankAccount);
            Assert.Equal(2500, savedBankAccount.Balance);  // 2000 + 500

            // Act: Usar el servicio para realizar un retiro
            var withdrawalTransaction = await transactionService.WithdrawAsync(bankAccount.Id, 300);

            // Assert: Verificar el saldo después del retiro
            savedBankAccount = await context.BankAccounts.FindAsync(bankAccount.Id);
            Assert.NotNull(savedBankAccount);
            Assert.Equal(2200, savedBankAccount.Balance);  // 2500 - 300
        }
    }
}
