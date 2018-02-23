using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BettingApp.Web.Controllers
{
    [Route("api/matches")]
    public class MatchesController : Controller
    {
        public MatchesController()
        {
            _matchRepository = new MatchRepository();
        }
        private readonly MatchRepository _matchRepository;

        [HttpGet]
        [Route("sport")]
        public IActionResult GetMatchesForSport(int sportId)
        {
            return Ok(_matchRepository.GetMatchesForSport(sportId));
        }

        [HttpGet]
        [Route("day")]
        public IActionResult GetMatchesForSpecificDay(DateTime dayOfMatches)
        {
            return Ok(_matchRepository.GetMatchesForSpecificDay(dayOfMatches));
        }
    }
}
