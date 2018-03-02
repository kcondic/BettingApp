using System.Linq;
using System.Data.Entity;
using BettingApp.Data.Enums;
using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class WalletRepository
    {
        public WalletRepository(BettingContext context)
        {
            _context = context;
        }
        private readonly BettingContext _context;

        public Wallet GetWallet(int userId)
        {
            var userToGetWallet = _context.Users
                                            .Include(user => user.Wallet)
                                            .FirstOrDefault(user => user.Id == userId);
            if (userToGetWallet == null || userToGetWallet.Role == Role.Admin)
                return null;

            return userToGetWallet.Wallet;
        }

        public bool FundsPayment(int walletId, double fundsToGrant)
        {
            var wallet = _context.Wallets.Find(walletId);
            if (wallet == null || fundsToGrant < 10)
                return false;
            wallet.Funds += fundsToGrant;
            _context.SaveChanges();
            return true;
        }
    }
}
