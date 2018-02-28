using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class TeamRepository
    {
        public bool AddTeam(Team teamToAdd)
        {
            using (var context = new BettingContext())
            {
                if (teamToAdd.Sport == null)
                    return false;
                context.Sports.Attach(teamToAdd.Sport);
                context.Teams.Add(teamToAdd);
                context.SaveChanges();
                return true;
            }
        }
    }
}
