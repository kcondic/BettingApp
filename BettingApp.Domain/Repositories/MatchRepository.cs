using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using BettingApp.Data.Enums;
using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Domain.Repositories
{
    public class MatchRepository
    {
        public MatchRepository(BettingContext context)
        {
            _context = context;
        }
        private readonly BettingContext _context;

        public List<Match> GetMatchesForSport(int sportId)
        {
            return _context.Matches
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
            var currentTime = DateTime.Now;
            var tomorrow = DateTime.Today.AddDays(1);
            var dayAfterTomorrow = DateTime.Today.AddDays(2);

            if(dayOfMatches == "today")
                return _context.Matches
                    .Include(match => match.HomeTeam)
                    .Include(match => match.HomeTeam.Sport)
                    .Include(match => match.AwayTeam)
                    .Where(match => match.Outcome == null &&
                                    match.TimeOfStart > currentTime && 
                                    match.TimeOfStart < tomorrow)
                    .ToList()
                    .GroupBy(match => match.HomeTeam.Sport, match => match);
                
            return _context.Matches
                .Include(match => match.HomeTeam)
                .Include(match => match.HomeTeam.Sport)
                .Include(match => match.AwayTeam)
                .Where(match => match.Outcome == null &&
                                match.TimeOfStart > tomorrow && 
                                match.TimeOfStart < dayAfterTomorrow)
                .ToList()
                .GroupBy(match => match.HomeTeam.Sport, match => match);
        }

        public List<Match> GetMatchesWithoutOutcome()
        {
            return _context.Matches
                            .Include(match => match.HomeTeam)
                            .Include(match => match.HomeTeam.Sport)
                            .Include(match => match.AwayTeam)
                            .Where(match => match.Outcome == null)
                            .ToList();
        }

        public bool AddNewMatch(Match matchToAdd)
        {
            var sportOfMatch = _context.Sports.SingleOrDefault(sport => sport.Id == matchToAdd.HomeTeam.SportId);
            if (sportOfMatch == null)
                return false;
            if (matchToAdd.TimeOfStart < DateTime.Now
                || matchToAdd.HomeTeam.Id == matchToAdd.AwayTeam.Id
                || matchToAdd.AwayTeam.SportId != sportOfMatch.Id
                || !sportOfMatch.IsDrawPossible && matchToAdd.DrawOdd != null
                || matchToAdd.DrawOdd != null && matchToAdd.DrawOdd < 1.01
                || matchToAdd.HomeWinOdd != null && matchToAdd.HomeWinOdd < 1.01
                || matchToAdd.AwayWinOdd != null && matchToAdd.AwayWinOdd < 1.01)
                return false;
            _context.Teams.Attach(matchToAdd.HomeTeam);
            _context.Teams.Attach(matchToAdd.AwayTeam);

            _context.Matches.Add(matchToAdd);
            _context.SaveChanges();
            return true;
        }

        public bool ChangeMatchOdds(int matchId, int oddToChange, double newOdd)
        {
            var matchToChange = _context.Matches.Find(matchId);
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

            _context.SaveChanges();
            return true;
        }

        public bool SetMatchOutcome(int matchId, int outcomeType)
        {
            var matchToChange = _context.Matches
                                        .Include(match => match.HomeTeam)
                                        .Include(match => match.HomeTeam.Sport)
                                        .SingleOrDefault(match => match.Id == matchId);
            var outcome = (Outcome)outcomeType;
            if (matchToChange == null || matchToChange.Outcome != null || 
                !matchToChange.HomeTeam.Sport.IsDrawPossible && outcome == Outcome.Draw)
                return false;
            matchToChange.Outcome = (Outcome)outcomeType;
            _context.SaveChanges();
            return true;
        }
    }
}
