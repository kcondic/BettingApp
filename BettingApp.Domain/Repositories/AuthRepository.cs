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
    public class AuthRepository
    {
        public User SignIn(string username)
        {
            using (var context = new BettingContext())
                return context.Users
                              .Include(user => user.Wallet)
                              .SingleOrDefault(user => user.UserName == username);
        }
    }
}
