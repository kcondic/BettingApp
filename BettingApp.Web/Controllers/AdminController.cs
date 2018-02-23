using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApp.Data.Models.Entities;
using BettingApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BettingApp.Web.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        public AdminController()
        {
            _matchRepository = new MatchRepository();
            _teamRepository = new TeamRepository();
            _sportRepository = new SportRepository();
        }
        private readonly MatchRepository _matchRepository;
        private readonly TeamRepository _teamRepository;
        private readonly SportRepository _sportRepository;

        [HttpPost]
        [Route("matches")]
        public IActionResult AddNewMatch(Match matchToAdd)
        {
            var wasMatchAdded = _matchRepository.AddNewMatch(matchToAdd);
            if (!wasMatchAdded)
                return Forbid();
            return Ok(true);
        }

        [HttpPost]
        [Route("matches/odds")]
        public IActionResult ChangeMatchOdds(int matchId, int oddToChange, double newOdd)
        {
            var wasOddChanged = _matchRepository.ChangeMatchOdds(matchId, oddToChange, newOdd);
            if (!wasOddChanged)
                return Forbid();
            return Ok(true);
        }

        [HttpPost]
        [Route("matches/time")]
        public IActionResult ChangeMatchDateTime(int matchId, DateTime newDateTime)
        {
            var wasTimeChanged = _matchRepository.ChangeMatchDateTime(matchId, newDateTime);
            if (!wasTimeChanged)
                return Forbid();
            return Ok(true);
        }

        [HttpPost]
        [Route("matches/outcome")]
        public IActionResult SetMatchOutcome(int matchId, int outcomeType)
        {
            var wasOutcomeSet = _matchRepository.SetMatchOutcome(matchId, outcomeType);
            if (!wasOutcomeSet)
                return Forbid();
            return Ok(true);
        }

        [HttpGet]
        [Route("teams")]
        public IActionResult GetTeamsBySport()
        {
            return Ok(_teamRepository.GetTeamsBySport());
        }

        [HttpPost]
        [Route("teams")]
        public IActionResult AddTeam(Team teamToAdd)
        {
            var wasTeamAdded = _teamRepository.AddTeam(teamToAdd);
            if (!wasTeamAdded)
                return Forbid();
            return Ok(true);
        }

        [HttpGet]
        [Route("sports")]
        public IActionResult GetSports()
        {
            return Ok(_sportRepository.GetSports());
        }

        [HttpPost]
        [Route("sports")]
        public IActionResult AddSport(Sport sportToAdd)
        {
            _sportRepository.AddSport(sportToAdd);
            return Ok(true);
        }

    }
}
