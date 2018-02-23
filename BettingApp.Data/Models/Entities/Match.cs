using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BettingApp.Data.Enums;

namespace BettingApp.Data.Models.Entities
{
    public class Match
    {
        public int Id { get; set; }
        public Team HomeTeam { get; set; }
        public int HomeTeamId { get; set; }
        public Team AwayTeam { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime TimeOfStart { get; set; }
        public Outcome? Outcome { get; set; }
        [Range(1.01, Double.MaxValue)]
        public double? HomeWinOdd { get; set; }
        [Range(1.01, Double.MaxValue)]
        public double? DrawOdd { get; set; }
        [Range(1.01, Double.MaxValue)]
        public double? AwayWinOdd { get; set; }
        public ICollection<TicketMatch> TicketMatches { get; set; }
    }
}
