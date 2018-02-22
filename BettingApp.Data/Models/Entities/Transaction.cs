using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BettingApp.Data.Enums;

namespace BettingApp.Data.Models.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public Wallet Wallet { get; set; }
        public int WalletId { get; set; }
        public TransactionType TransactionType { get; set; }
        public double TransactionAmount { get; set; }
        public DateTime TimeOfTransaction { get; set; }
    }
}
