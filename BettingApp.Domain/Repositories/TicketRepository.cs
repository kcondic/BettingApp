﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using BettingApp.Data.Enums;
using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class TicketRepository
    {
        public TicketRepository()
        {
         _transactionRepository = new TransactionRepository();       
        }
        private readonly TransactionRepository _transactionRepository;
        public List<Ticket> GetTickets(int walletId)
        {
            using (var context = new BettingContext())
                return context.Tickets
                    .Include(ticket => ticket.TicketMatches)
                    .Include(ticket => ticket.TicketMatches.Select(ticketMatch => ticketMatch.Match))
                    .Include(ticket => ticket.TicketMatches.Select(ticketMatch => ticketMatch.Match.HomeTeam))
                    .Include(ticket => ticket.TicketMatches.Select(ticketMatch => ticketMatch.Match.AwayTeam))
                    .Where(ticket => ticket.WalletId == walletId)
                    .ToList();
        }

        public bool PlaceBet(Ticket ticketToPlace)
        {
            using (var context = new BettingContext())
            {
                context.Wallets.Attach(ticketToPlace.Wallet);
                if (ticketToPlace.Stake < 2 || ticketToPlace.Wallet.Funds < ticketToPlace.Stake)
                    return false;
                var totalOdd = 1.0;
                foreach (var ticketMatch in ticketToPlace.TicketMatches)
                {
                    var match = context.Matches.Find(ticketMatch.MatchId);
                    if (match == null)
                        return false;
                    switch (ticketMatch.Tip)
                    {
                        case Outcome.HomeWin:
                            totalOdd *= (double) match.HomeWinOdd;
                            break;
                        case Outcome.Draw:
                            totalOdd *= (double) match.DrawOdd;
                            break;
                        case Outcome.AwayWin:
                            totalOdd *= (double) match.AwayWinOdd;
                            break;
                        default:
                            return false;
                    }
                }
                if (Math.Abs(totalOdd - ticketToPlace.TotalOdd) > 0.1)
                    return false;
                ticketToPlace.Wallet.Funds -= ticketToPlace.Stake;
                context.Tickets.Add(ticketToPlace);
                context.SaveChanges();
                return true;
            }
        }

        public void CheckWinningLosingTickets(int matchId)
        {
            using (var context = new BettingContext())
            {
                var unProcessedTicketsWithThatMatch = context.Tickets
                                                             .Include(ticket => ticket.TicketMatches)
                                                             .Include(ticket => ticket.TicketMatches.Select(ticketMatch => ticketMatch.Match))
                                                             .Where(ticket => ticket.Payout == null && ticket.TicketMatches
                                                                       .Any(ticketMatch => ticketMatch.MatchId == matchId));
                var ticketIdsForPayout = new List<int>();
                foreach (var ticket in unProcessedTicketsWithThatMatch)
                {
                    var ticketMatchToCheck = ticket.TicketMatches.SingleOrDefault(ticketMatch => ticketMatch.MatchId == matchId);
                    if (ticketMatchToCheck == null)
                        continue;
                    if (ticketMatchToCheck.Match.Outcome == Outcome.WalkOver)
                        ticket.TotalOdd /= ticketMatchToCheck.PlacedOdd;
                    else if (ticketMatchToCheck.Match.Outcome != ticketMatchToCheck.Tip)
                        ticket.Payout = 0;


                    if (ticket.TicketMatches.All(ticketMatch => ticketMatch.Match.Outcome == ticketMatch.Tip
                                                                || ticketMatch.Match.Outcome == Outcome.WalkOver))
                    {
                        ticket.Payout = Math.Round(ticket.Stake * ticket.TotalOdd, 2);
                        ticketIdsForPayout.Add(ticket.Id);
                    }
                }
                context.SaveChanges();

                foreach(var ticketId in ticketIdsForPayout)
                    _transactionRepository.PayWinningBet(ticketId);
            }
        }
    }
}
