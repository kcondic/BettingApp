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
    public class TicketRepository
    {
        public IEnumerable<Ticket> GetTickets(int walletId)
        {
            using (var context = new BettingContext())
                return context.Tickets
                    .Include(ticket => ticket.TicketMatches)
                    .Include(ticket => ticket.TicketMatches.Select(ticketMatch => ticketMatch.Match))
                    .Include(ticket => ticket.TicketMatches.Select(ticketMatch => ticketMatch.Match.HomeTeam))
                    .Include(ticket => ticket.TicketMatches.Select(ticketMatch => ticketMatch.Match.AwayTeam))
                    .Where(ticket => ticket.WalletId == walletId).ToList();

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
    }
}
