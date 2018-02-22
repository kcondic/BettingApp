using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingApp.Data.Models.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double Funds { get; set; }
        public User Owner { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
