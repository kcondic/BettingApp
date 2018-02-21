using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
