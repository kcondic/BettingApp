using BettingApp.Data.Models.Entities;
using BettingApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
            _ticketRepository = new TicketRepository();
        }
        private readonly MatchRepository _matchRepository;
        private readonly TeamRepository _teamRepository;
        private readonly SportRepository _sportRepository;
        private readonly TicketRepository _ticketRepository;

        [HttpGet]
        [Route("matches")]
        public IActionResult GetMatchesWithoutOutcome()
        {
            return Ok(_matchRepository.GetMatchesWithoutOutcome());
        }

        [HttpPost]
        [Route("matches")]
        public IActionResult AddNewMatch([FromBody]Match matchToAdd)
        {
            var wasMatchAdded = _matchRepository.AddNewMatch(matchToAdd);
            if (!wasMatchAdded)
                return Forbid();
            return Ok(true);
        }

        [HttpPost]
        [Route("matches/odds")]
        public IActionResult ChangeMatchOdd([FromBody]JObject oddToChangeObject)
        {
            var matchId = oddToChangeObject["matchId"].ToObject<int>();
            var oddToChange = oddToChangeObject["oddToChange"].ToObject<int>();
            var newOdd = oddToChangeObject["newOdd"].ToObject<double>();
            var wasOddChanged = _matchRepository.ChangeMatchOdds(matchId, oddToChange, newOdd);
            if (!wasOddChanged)
                return Forbid();
            return Ok(true);
        }

        [HttpPost]
        [Route("matches/outcome")]
        public IActionResult SetMatchOutcome([FromBody]JObject matchOutcomeObject)
        {
            var matchId = matchOutcomeObject["matchId"].ToObject<int>();
            var outcome = matchOutcomeObject["outcome"].ToObject<int>();
            var wasOutcomeSet = _matchRepository.SetMatchOutcome(matchId, outcome);
            if (!wasOutcomeSet)
                return Forbid();
            _ticketRepository.CheckWinningLosingTickets(matchId);
            return Ok(true);
        }

        [HttpPost]
        [Route("teams")]
        public IActionResult AddTeam([FromBody]Team teamToAdd)
        {
            var wasTeamAdded = _teamRepository.AddTeam(teamToAdd);
            if (!wasTeamAdded)
                return Forbid();
            return Ok(true);
        }

        [HttpPost]
        [Route("sports")]
        public IActionResult AddSport([FromBody]Sport sportToAdd)
        {
            _sportRepository.AddSport(sportToAdd);
            return Ok(true);
        }

    }
}
