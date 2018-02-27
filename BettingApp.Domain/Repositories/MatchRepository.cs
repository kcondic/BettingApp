using System;
using System.Collections.Generic;
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
    public class MatchRepository
    {
        public List<Match> GetMatchesForSport(int sportId)
        {
            using (var context = new BettingContext())
                return context.Matches
                              .Include(match => match.HomeTeam)
                              .Include(match => match.AwayTeam)
                              .Where(match => DateTime.Now < match.TimeOfStart 
                                            && match.HomeTeam.SportId == sportId
                                            && match.Outcome == null)
                              .OrderBy(match => match.TimeOfStart)
                              .ToList();
        }

        public IEnumerable<IGrouping<Sport, Match>> GetMatchesForSpecificDay(string dayOfMatches)
        {
            using (var context = new BettingContext())
            {
                var currentTime = DateTime.Now;
                var tomorrow = DateTime.Today.AddDays(1);
                var dayAfterTomorrow = DateTime.Today.AddDays(2);

                if(dayOfMatches == "today")
                    return context.Matches
                        .Include(match => match.HomeTeam)
                        .Include(match => match.HomeTeam.Sport)
                        .Include(match => match.AwayTeam)
                        .Where(match => match.Outcome == null &&
                                        match.TimeOfStart > currentTime && 
                                        match.TimeOfStart < tomorrow)
                        .ToList()
                        .GroupBy(match => match.HomeTeam.Sport, match => match);
                
                return context.Matches
                    .Include(match => match.HomeTeam)
                    .Include(match => match.HomeTeam.Sport)
                    .Include(match => match.AwayTeam)
                    .Where(match => match.Outcome == null &&
                                    match.TimeOfStart > tomorrow && 
                                    match.TimeOfStart < dayAfterTomorrow)
                    .ToList()
                    .GroupBy(match => match.HomeTeam.Sport, match => match);
            }
        }

        public List<Match> GetMatchesWithoutOutcome()
        {
            using (var context = new BettingContext())
                return context.Matches
                              .Include(match => match.HomeTeam)
                              .Include(match => match.AwayTeam)
                              .Where(match => match.Outcome == null)
                              .ToList();
        }

        public bool AddNewMatch(Match matchToAdd)
        {
            using (var context = new BettingContext())
            {
                if (matchToAdd.TimeOfStart < DateTime.Now || matchToAdd.HomeTeam.Sport != matchToAdd.AwayTeam.Sport
                    || !matchToAdd.HomeTeam.Sport.IsDrawPossible && matchToAdd.DrawOdd != null)
                    return false;
                context.Teams.Attach(matchToAdd.HomeTeam);
                context.Teams.Attach(matchToAdd.AwayTeam);

                context.Matches.Add(matchToAdd);
                context.SaveChanges();
                return true;
            }
        }

        public bool ChangeMatchOdds(int matchId, int oddToChange, double newOdd)
        {
            using (var context = new BettingContext())
            {
                var matchToChange = context.Matches.Find(matchId);
                if (matchToChange == null || matchToChange.TimeOfStart < DateTime.Now || newOdd < 1.01)
                    return false;

                switch (oddToChange)
                {
                    case 0:
                        matchToChange.HomeWinOdd = newOdd;
                        break;
                    case 1:
                        matchToChange.DrawOdd = newOdd;
                        break;
                    case 2:
                        matchToChange.AwayWinOdd = newOdd;
                        break;
                    default:
                        return false;
                }

                context.SaveChanges();
                return true;
            }
        }

        public bool SetMatchOutcome(int matchId, int outcomeType)
        {
            using (var context = new BettingContext())
            {
                var matchToChange = context.Matches
                                           .Include(match => match.HomeTeam)
                                           .Include(match => match.HomeTeam.Sport)
                                           .SingleOrDefault(match => match.Id == matchId);
                var outcome = (Outcome)outcomeType;
                if (matchToChange == null || matchToChange.Outcome != null || 
                    !matchToChange.HomeTeam.Sport.IsDrawPossible && outcome == Outcome.Draw)
                    return false;
                matchToChange.Outcome = (Outcome)outcomeType;
                context.SaveChanges();
                return true;
            }
        }
    }
}
