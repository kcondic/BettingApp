using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class TeamRepository
    {
        public TeamRepository(BettingContext context)
        {
            _context = context;
        }
        private readonly BettingContext _context;

        public bool AddTeam(Team teamToAdd)
        {
            if (teamToAdd.Sport == null)
                return false;
            _context.Sports.Attach(teamToAdd.Sport);
            _context.Teams.Add(teamToAdd);
            _context.SaveChanges();
            return true;
        }
    }
}
