using BettingApp.Data.Enums;

namespace BettingApp.Data.Models.Entities
{
    public class TicketMatch
    {
        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }
        public Match Match { get; set; }
        public int MatchId { get; set; }
        public Outcome Tip { get; set; }
        public double PlacedOdd { get; set; }
    }
}
