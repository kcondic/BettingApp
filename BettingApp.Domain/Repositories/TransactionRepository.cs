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
    public class TransactionRepository
    {
        public void AddTransaction(int walletId, double transactionAmount, TransactionType transactionType)
        {
            using (var context = new BettingContext())
            {
                var transactionToAdd = new Transaction()
                {
                    WalletId = walletId,
                    TransactionType = transactionType,
                    TransactionAmount = transactionAmount,
                    TimeOfTransaction = DateTime.Now
                };
                context.Transactions.Add(transactionToAdd);
                context.SaveChanges();
            }
        }

        public IEnumerable<Transaction> GetWalletTransactions(int walletId)
        {
            using (var context = new BettingContext())
                return context.Transactions
                              .Where(transaction => transaction.WalletId == walletId)
                              .OrderByDescending(transaction => transaction.TimeOfTransaction)
                              .ToList();    
        }
    }
}
