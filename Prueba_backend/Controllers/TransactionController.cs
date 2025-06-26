using Microsoft.AspNetCore.Mvc;
using Bacnkend.Data;
using Bacnkend.Models;

namespace Bacnkend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly BankingContext _context;

        public TransactionController(BankingContext context)
        {
            _context = context;
        }

        [HttpPost("deposit")]
        public IActionResult Deposit(int accountId, decimal amount)
        {
            var account = _context.BankAccounts.Find(accountId);
            if (account == null) return NotFound();

            account.Balance += amount;

            var transaction = new Transaction
            {
                Type = "Deposit",
                Amount = amount,
                Date = DateTime.Now,
                BankAccountId = account.Id,
                BalanceAfterTransaction = account.Balance
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(transaction);
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw(int accountId, decimal amount)
        {
            var account = _context.BankAccounts.Find(accountId);
            if (account == null) return NotFound();

            if (account.Balance < amount)
                return BadRequest("Fondos insuficientes.");

            account.Balance -= amount;

            var transaction = new Transaction
            {
                Type = "Withdraw",
                Amount = amount,
                Date = DateTime.Now,
                BankAccountId = account.Id,
                BalanceAfterTransaction = account.Balance
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(transaction);
        }

        [HttpGet("history/{accountId}")]
        public IActionResult GetTransactionHistory(int accountId)
        {
            var transactions = _context.Transactions
                .Where(t => t.BankAccountId == accountId)
                .OrderBy(t => t.Date)
                .ToList();

            return Ok(transactions);
        }
    }
}
