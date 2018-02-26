using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class SportRepository
    {
        public IEnumerable<Sport> GetSports()
        {
            using (var context = new BettingContext())
                return context.Sports
                              .Include(sport => sport.Teams)
                              .ToList();
        }

        public void AddSport(Sport sportToAdd)
        {
            using (var context = new BettingContext())
            {
                context.Sports.Add(sportToAdd);
                context.SaveChanges();
            }
        }
    }
}
