using BettingApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BettingApp.Web.Controllers
{
    [Route("test")]
    public class TestController : Controller
    {
        public TestController()
        {
            _testRepository = new TestRepository();
        }

        private readonly TestRepository _testRepository;

        [HttpGet]
        [Route("")]
        public IActionResult GetAllTeams()
        {
            return Ok(_testRepository.GetTeams());
        }
    }
}
