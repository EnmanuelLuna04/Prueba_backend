using System;
using System.Collections.Generic;

namespace Bacnkend.Models
{
    public class Client
    {
        public int Id { get; set; } 

       
        public string? Name { get; set; }

        
        public DateTime? DateOfBirth { get; set; }

        
        public string? Gender { get; set; }

        
        public decimal? Income { get; set; }

       
        public ICollection<BankAccount> BankAccounts { get; set; }
    }
}
