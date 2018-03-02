using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class SportRepository
    {
        public SportRepository(BettingContext context)
        {
            _context = context;
        }
        private readonly BettingContext _context;

        public List<Sport> GetSports()
        {
            return _context.Sports
                            .Include(sport => sport.Teams)
                            .ToList();
        }

        public void AddSport(Sport sportToAdd)
        {
            _context.Sports.Add(sportToAdd);
            _context.SaveChanges();
        }
    }
}
