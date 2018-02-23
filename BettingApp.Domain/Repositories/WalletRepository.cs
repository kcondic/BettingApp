using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using BettingApp.Data.Enums;
using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class WalletRepository
    {
        public Wallet GetWallet(int userId)
        {
            using (var context = new BettingContext())
            {
                var userToGetWallet = context.Users
                                             .Include(user => user.Wallet)
                                             .FirstOrDefault(user => user.Id == userId);
                if (userToGetWallet == null || userToGetWallet.Role == Role.Admin)
                    return null;

                return userToGetWallet.Wallet;
            }
        }

        public bool FundsPayment(int walletId, double fundsToGrant)
        {
            using (var context = new BettingContext())
            {
                var wallet = context.Wallets.Find(walletId);
                if (wallet == null)
                    return false;
                wallet.Funds += fundsToGrant;
                context.SaveChanges();
                return true;
            }
        }
    }
}
