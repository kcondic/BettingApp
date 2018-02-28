using System.Collections.Generic;

namespace BettingApp.Data.Models.Entities
{
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDrawPossible { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}
