using BettingApp.Data.Models;
using BettingApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BettingApp.Web.Controllers
{
    [Route("api/matches")]
    public class MatchesController : Controller
    {
        public MatchesController(BettingContext context)
        {
            _matchRepository = new MatchRepository(context);
            _sportRepository = new SportRepository(context);
        }
        private readonly MatchRepository _matchRepository;
        private readonly SportRepository _sportRepository;

        [HttpGet]
        [Route("sport")]
        public IActionResult GetMatchesForSport(int sportId)
        {
            return Ok(_matchRepository.GetMatchesForSport(sportId));
        }

        [HttpGet]
        [Route("day")]
        public IActionResult GetMatchesForSpecificDay(string dayOfMatches)
        {
            return Ok(_matchRepository.GetMatchesForSpecificDay(dayOfMatches));
        }

        [HttpGet]
        [Route("sports")]
        public IActionResult GetSports()
        {
            return Ok(_sportRepository.GetSports());
        }
    }
}
