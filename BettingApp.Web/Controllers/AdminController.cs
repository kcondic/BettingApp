using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApp.Data.Models.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BettingApp.Web.Controllers
{
    [Route("api/admin")]
    public class AdminController : Controller
    {
        [HttpPost]
        [Route("matches")]
        public IActionResult AddNewMatch(Match matchToAdd)
        {

        }

        [HttpPost]
        [Route("matches/odds")]
        public IActionResult ChangeMatchOdds(int matchId, int oddToChange, double newOdd)
        {

        }

        [HttpPost]
        [Route("matches/time")]
        public IActionResult ChangeMatchDateTime(int matchId, DateTime newDateTime)
        {

        }

        [HttpPost]
        [Route("matches/outcome")]
        public IActionResult SetMatchOutcome(int matchId, int outcomeType)
        {

        }

        [HttpGet]
        [Route("teams")]
        public IActionResult GetTeamsBySport()
        {

        }

        [HttpPost]
        [Route("teams")]
        public IActionResult AddTeam(Team teamToAdd)
        {

        }

        [HttpGet]
        [Route("sports")]
        public IActionResult GetSports()
        {

        }

        [HttpPost]
        [Route("sports")]
        public IActionResult AddSport(Sport sportToAdd)
        {

        }

    }
}
