using System.Collections.Generic;

namespace Bacnkend.Models
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } 
        public decimal Balance { get; set; }

        // Clave foránea hacia Client
        public int ClientId { get; set; }
        public Client Client { get; set; }

        // Relación 1:N con transacciones
        public ICollection<Transaction> Transactions { get; set; }

        // Constructor para inicializar la colección de transacciones
        public BankAccount()
        {
            Transactions = new List<Transaction>(); 
        }
    }
}
