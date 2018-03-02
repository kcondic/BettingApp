using System.Linq;
using System.Data.Entity;
using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class AuthRepository
    {
        public AuthRepository(BettingContext context)
        {
            _context = context;
        }
        private readonly BettingContext _context;

        public User SignIn(string username)
        {
            return _context.Users
                            .Include(user => user.Wallet)
                            .SingleOrDefault(user => user.UserName == username);
        }
    }
}
