using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class TestRepository
    {
        public List<Team> GetTeams()
        {
            var bla = new List<Team>();
            using (var context = new BettingContext())
                //try
                //{
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges
                    return context.Teams.ToList();
            //    }
            //    catch (DbEntityValidationException e)
            //    {
            //        foreach (var eve in e.EntityValidationErrors)
            //        {
            //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //            foreach (var ve in eve.ValidationErrors)
            //            {
            //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                    ve.PropertyName, ve.ErrorMessage);
            //            }
            //        }
            //        throw;
            //    }

            //return bla;
        }
    }
}
