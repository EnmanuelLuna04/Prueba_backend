using Microsoft.AspNetCore.Mvc;
using Bacnkend.Data;
using Bacnkend.Models;

namespace Bacnkend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankAccountController : ControllerBase
    {
        private readonly BankingContext _context;

        public BankAccountController(BankingContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(BankAccount account)
        {
            _context.BankAccounts.Add(account);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetByAccountNumber), new { accountNumber = account.AccountNumber }, account);
        }

        [HttpGet("{accountNumber}")]
        public IActionResult GetByAccountNumber(string accountNumber)
        {
            var account = _context.BankAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null) return NotFound();
            return Ok(account);
        }
    }
}
