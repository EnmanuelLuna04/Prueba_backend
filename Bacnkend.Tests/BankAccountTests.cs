using Bacnkend.Data;
using Bacnkend.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;

namespace Bacnkend.Tests
{
    public class BankAccountTests
    {
        // Método para obtener el contexto de SQLite
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
        public void CanCreateBankAccount()
        {
            // Arrange
            using var context = GetContext();

            var client = new Client
            {
                Name = "Juan Pérez",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = "Masculino",
                Income = 3000
            };

            context.Clients.Add(client);
            context.SaveChanges();

            var bankAccount = new BankAccount
            {
                AccountNumber = "ACC12345",
                Balance = 1000,
                ClientId = client.Id
            };

            // Act
            context.BankAccounts.Add(bankAccount);
            context.SaveChanges();

            // Assert
            var savedBankAccount = context.BankAccounts.Find(bankAccount.Id);
            Assert.NotNull(savedBankAccount);
            Assert.Equal("ACC12345", savedBankAccount.AccountNumber);
            Assert.Equal(1000, savedBankAccount.Balance);
        }

        [Fact]
        public void CanDepositAndWithdraw()
        {
            // Arrange
            using var context = GetContext();

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

            // Act: Depósito
            var deposit = new Transaction
            {
                Type = "Deposit",
                Amount = 500,
                BalanceAfterTransaction = bankAccount.Balance + 500,
                Date = DateTime.Now,
                BankAccountId = bankAccount.Id
            };
            context.Transactions.Add(deposit);
            bankAccount.Balance += deposit.Amount;
            context.SaveChanges();

            // Act: Retiro
            var withdrawal = new Transaction
            {
                Type = "Withdraw",
                Amount = 300,
                BalanceAfterTransaction = bankAccount.Balance - 300,
                Date = DateTime.Now,
                BankAccountId = bankAccount.Id
            };
            context.Transactions.Add(withdrawal);
            bankAccount.Balance -= withdrawal.Amount;
            context.SaveChanges();

            // Assert: Verificar saldo después de depósito y retiro
            var savedBankAccount = context.BankAccounts.Find(bankAccount.Id);
            Assert.NotNull(savedBankAccount);
            Assert.Equal(2200, savedBankAccount.Balance); 
        }

        [Fact]
        public void CanApplyInterestToBalance()
        {
            // Arrange
            using var context = GetContext();

            var client = new Client
            {
                Name = "Ana Rodríguez",
                DateOfBirth = new DateTime(1992, 4, 5),
                Gender = "Femenino",
                Income = 6000
            };

            context.Clients.Add(client);
            context.SaveChanges();

            var bankAccount = new BankAccount
            {
                AccountNumber = "ACC99999",
                Balance = 1000,
                ClientId = client.Id
            };

            context.BankAccounts.Add(bankAccount);
            context.SaveChanges();

            // Act: Aplicar interés
            var interestRate = 0.05;  // 5% de interés
            decimal interestAmount = bankAccount.Balance * (decimal)interestRate; 
            bankAccount.Balance += interestAmount;

            var interestTransaction = new Transaction
            {
                Type = "Interest",
                Amount = interestAmount,
                BalanceAfterTransaction = bankAccount.Balance,
                Date = DateTime.Now,
                BankAccountId = bankAccount.Id
            };

            context.Transactions.Add(interestTransaction);
            context.SaveChanges();

            // Assert: Verificar el saldo después de aplicar interés
            var savedBankAccount = context.BankAccounts.Find(bankAccount.Id);
            Assert.NotNull(savedBankAccount);
            Assert.Equal(1050, savedBankAccount.Balance);  // 1000 + 50 (5% de 1000)
        }

        [Fact]
        public void CanGetBalanceAndTransactionHistory()
        {
            // Arrange
            using var context = GetContext();

            var client = new Client
            {
                Name = "Luis García",
                DateOfBirth = new DateTime(1980, 9, 22),
                Gender = "Masculino",
                Income = 4000
            };

            context.Clients.Add(client);
            context.SaveChanges();

            var bankAccount = new BankAccount
            {
                AccountNumber = "ACC11223",
                Balance = 5000,
                ClientId = client.Id
            };

            context.BankAccounts.Add(bankAccount);
            context.SaveChanges();

            // Act: Crear una transacción
            var transaction = new Transaction
            {
                Type = "Deposit",
                Amount = 1000,
                BalanceAfterTransaction = bankAccount.Balance + 1000,
                Date = DateTime.Now,
                BankAccountId = bankAccount.Id
            };
            context.Transactions.Add(transaction);
            bankAccount.Balance += transaction.Amount;
            context.SaveChanges();

            // Assert: Consultar saldo y verificar transacciones
            var savedBankAccount = context.BankAccounts.Find(bankAccount.Id);
            Assert.NotNull(savedBankAccount);
            Assert.Equal(6000, savedBankAccount.Balance);

            var transactions = context.Transactions.Where(t => t.BankAccountId == bankAccount.Id).ToList();
            Assert.NotEmpty(transactions);
            Assert.Equal(1, transactions.Count);
            Assert.Equal(1000, transactions[0].Amount);
        }
    }
}
