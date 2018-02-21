using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingApp.Data.Models.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public Wallet Wallet { get; set; }
        [Range(0.0, Double.MaxValue)]
        public double Stake { get; set; }
        [Range(1.0, Double.MaxValue)]
        public double TotalOdd { get; set; }
        public ICollection<TicketMatch> TicketMatches { get; set; }
    }
}
