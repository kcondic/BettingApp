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
        public TransactionRepository(BettingContext context)
        {
            _context = context;
        }
        private readonly BettingContext _context;

        public void AddTransaction(int walletId, double transactionAmount, TransactionType transactionType)
        {
            var transactionToAdd = new Transaction()
            {
                WalletId = walletId,
                TransactionType = transactionType,
                TransactionAmount = transactionAmount,
                TimeOfTransaction = DateTime.Now
            };
            _context.Transactions.Add(transactionToAdd);
            _context.SaveChanges();
        }

        public List<Transaction> GetWalletTransactions(int walletId)
        {
            return _context.Transactions
                            .Where(transaction => transaction.WalletId == walletId)
                            .OrderByDescending(transaction => transaction.TimeOfTransaction)
                            .ToList();    
        }

        public void PayWinningBet(int winningTicketId)
        {
            var winningTicket = _context.Tickets
                                        .Include(ticket => ticket.Wallet)
                                        .SingleOrDefault(ticket => ticket.Id == winningTicketId);
            if (winningTicket == null || winningTicket.Wallet == null || winningTicket.Payout == null)
                return;
            winningTicket.Wallet.Funds += (double)winningTicket.Payout;
            _context.SaveChanges();
            AddTransaction(winningTicket.Wallet.Id, (double)winningTicket.Payout, TransactionType.Payout);
        }
    }
}
