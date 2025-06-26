using System;

namespace Bacnkend.Models
{
    public class Transaction
    {
        public int Id { get; set; }

       
        public string? Type { get; set; } 


        public decimal Amount { get; set; }
        public decimal? BalanceAfterTransaction { get; set; } 

        public DateTime Date { get; set; }

        
        public int BankAccountId { get; set; }
        public BankAccount? BankAccount { get; set; } 
    }
}
