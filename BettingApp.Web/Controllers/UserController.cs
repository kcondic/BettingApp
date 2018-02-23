using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingApp.Data.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BettingApp.Web.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult GetWallet(int userId)
        {

        }

        [HttpPost]
        [Route("wallet")]
        public IActionResult FundsPayment(int walletId, double fundsToGrant)
        {

        }

        [HttpGet]
        [Route("wallet")]
        public IActionResult GetWalletTransactions(int walletId)
        {

        }

        [HttpPost]
        [Route("bet")]
        public IActionResult MakeBet(Ticket ticketToPlace)
        {

        }


    }
}
