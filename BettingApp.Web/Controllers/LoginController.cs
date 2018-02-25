﻿using BettingApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BettingApp.Web.Controllers
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        public LoginController()
        {
            _authRepository = new AuthRepository();
        }
        private readonly AuthRepository _authRepository;

        [HttpPost]
        public IActionResult SignIn([FromBody]JObject usernameObject)
        {
            var username = usernameObject["username"].ToObject<string>();
            var signedInUser = _authRepository.SignIn(username);

            if (signedInUser == null)
                return NotFound();

            return Ok(signedInUser);
        }
    }
}