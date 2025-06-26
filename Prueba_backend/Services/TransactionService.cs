using Bacnkend.Data;
using Bacnkend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Bacnkend.Services
{
    public class TransactionService
    {
        private readonly BankingContext _context;

        // Inyección de dependencias: el contexto de la base de datos es inyectado en el servicio
        public TransactionService(BankingContext context)
        {
            _context = context;
        }

        // Método para registrar una transacción de depósito
        public async Task<Transaction> DepositAsync(int bankAccountId, decimal amount)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(bankAccountId);

            if (bankAccount == null)
                throw new ArgumentException("Cuenta bancaria no encontrada.");

            var transaction = new Transaction
            {
                Type = "Deposit",
                Amount = amount,
                BalanceAfterTransaction = bankAccount.Balance + amount,
                Date = DateTime.Now,
                BankAccountId = bankAccount.Id
            };

            bankAccount.Balance += amount;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        // Método para registrar una transacción de retiro
        public async Task<Transaction> WithdrawAsync(int bankAccountId, decimal amount)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(bankAccountId);

            if (bankAccount == null)
                throw new ArgumentException("Cuenta bancaria no encontrada.");

            if (bankAccount.Balance < amount)
                throw new InvalidOperationException("Saldo insuficiente para realizar el retiro.");

            var transaction = new Transaction
            {
                Type = "Withdraw",
                Amount = amount,
                BalanceAfterTransaction = bankAccount.Balance - amount,
                Date = DateTime.Now,
                BankAccountId = bankAccount.Id
            };

            bankAccount.Balance -= amount;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        // Método para aplicar intereses a la cuenta bancaria
        public async Task<Transaction> ApplyInterestAsync(int bankAccountId, decimal interestRate)
        {
            var bankAccount = await _context.BankAccounts.FindAsync(bankAccountId);

            if (bankAccount == null)
                throw new ArgumentException("Cuenta bancaria no encontrada.");

            decimal interestAmount = bankAccount.Balance * interestRate;
            bankAccount.Balance += interestAmount;

            var transaction = new Transaction
            {
                Type = "Interest",
                Amount = interestAmount,
                BalanceAfterTransaction = bankAccount.Balance,
                Date = DateTime.Now,
                BankAccountId = bankAccount.Id
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }
    }
}
