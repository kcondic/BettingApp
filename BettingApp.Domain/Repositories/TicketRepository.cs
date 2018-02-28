using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;
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
                    .Where(ticket => ticket.WalletId == walletId).ToList();
        }

        public Bonus.Bonus GetTicketBonuses(ICollection<TicketMatch> ticketMatchesToCheck)
        {
            using (var context = new BettingContext())
            {
                var bonus = new Bonus.Bonus()
                {
                    Messages = new List<string>(),
                    BonusOdd = 0
                };
                var matchesOnTicket = context.Matches
                                             .Include(match => match.HomeTeam)
                                             .ToList()
                                             .Where(match => ticketMatchesToCheck
                                                    .Any(ticketMatch => ticketMatch.MatchId == match.Id));

                var matchesSportCounts = matchesOnTicket.GroupBy(match => match.HomeTeam.SportId)
                                                        .Select(group => group.Count())
                                                        .Count(sportCount => sportCount >= 3);
                if (matchesSportCounts > 0)
                {
                    bonus.Messages.Add($"Bonus +5: 3 para iz istog sporta ({matchesSportCounts} sportova)");
                    bonus.BonusOdd += matchesSportCounts * 5;
                }

                var numberOfSportsWithMatches = context.Matches
                                                       .Include(match => match.HomeTeam)
                                                       .Where(match => DateTime.Now < match.TimeOfStart && match.Outcome == null)
                                                       .Select(match => match.HomeTeam.SportId)
                                                       .Distinct()
                                                       .Count();
                var numberOfSportsOnTicket = matchesOnTicket.Select(match => match.HomeTeam.SportId)
                                                            .Distinct()
                                                            .Count();
                if (numberOfSportsWithMatches == numberOfSportsOnTicket)
                {
                    bonus.Messages.Add("Bonus +10: odabrani parovi iz svih sportova");
                    bonus.BonusOdd += 10;
                }

                return bonus;
            }
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
                            totalOdd = Math.Round(totalOdd*(double) match.HomeWinOdd, 2, MidpointRounding.AwayFromZero);
                            break;
                        case Outcome.Draw:
                            totalOdd = Math.Round(totalOdd * (double)match.DrawOdd, 2, MidpointRounding.AwayFromZero);
                            break;
                        case Outcome.AwayWin:
                            totalOdd = Math.Round(totalOdd * (double)match.AwayWinOdd, 2, MidpointRounding.AwayFromZero);
                            break;
                        default:
                            return false;
                    }
                }
                var bonus = GetTicketBonuses(ticketToPlace.TicketMatches);
                if (Math.Abs(totalOdd + bonus.BonusOdd - ticketToPlace.TotalOdd) > 0.1)
                    return false;
                ticketToPlace.TotalOdd = Math.Round(ticketToPlace.TotalOdd, 2, MidpointRounding.AwayFromZero);
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
                    {
                        var bonus = GetTicketBonuses(ticket.TicketMatches);
                        ticket.TotalOdd -= bonus.BonusOdd;
                        ticket.TotalOdd /= ticketMatchToCheck.PlacedOdd;
                        ticket.TotalOdd += bonus.BonusOdd;
                        ticket.TotalOdd = Math.Round(ticket.TotalOdd, 2, MidpointRounding.AwayFromZero);
                    }
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
