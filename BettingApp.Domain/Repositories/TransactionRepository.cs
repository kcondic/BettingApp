using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
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

        public List<Transaction> GetWalletTransactions(int walletId)
        {
            using (var context = new BettingContext())
                return context.Transactions
                              .Where(transaction => transaction.WalletId == walletId)
                              .OrderByDescending(transaction => transaction.TimeOfTransaction)
                              .ToList();    
        }

        public void PayWinningBet(int winningTicketId)
        {
            using (var context = new BettingContext())
            {
                var winningTicket = context.Tickets
                                           .Include(ticket => ticket.Wallet)
                                           .SingleOrDefault(ticket => ticket.Id == winningTicketId);
                if (winningTicket == null || winningTicket.Wallet == null || winningTicket.Payout == null)
                    return;
                winningTicket.Wallet.Funds += (double)winningTicket.Payout;
                context.SaveChanges();
                AddTransaction(winningTicket.Wallet.Id, (double)winningTicket.Payout, TransactionType.Payout);
            }
        }
    }
}
