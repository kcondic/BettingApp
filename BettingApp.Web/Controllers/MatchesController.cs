using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BettingApp.Web.Controllers
{
    [Route("api/matches")]
    public class MatchesController : Controller
    {
        [HttpGet]
        [Route("sport")]
        public IActionResult GetMatchesForSport()
        {

        }

        [HttpGet]
        [Route("day")]
        public IActionResult GetMatchesForSpecificDay()
        {

        }
    }
}
