using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BettingApp.Domain.Repositories;

namespace BettingApp.Web.Controllers
{
    [RoutePrefix("test")]
    public class TestController : ApiController
    {
        public TestController()
        {
            _testRepository = new TestRepository();
        }

        private readonly TestRepository _testRepository;

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAllTeams()
        {
            return Ok(_testRepository.GetTeams());
        }
    }
}
