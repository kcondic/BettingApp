using System.Linq;
using System.Data.Entity;
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
