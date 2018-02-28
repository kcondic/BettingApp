using System.Collections.Generic;

namespace BettingApp.Data.Models.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Match> HomeMatches { get; set; }
        public ICollection<Match> AwayMatches { get; set; }
        public Sport Sport { get; set; }
        public int SportId { get; set; }
    }
}
