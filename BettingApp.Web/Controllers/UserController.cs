using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BettingApp.Data.Enums;
using BettingApp.Data.Models.Entities;
using BettingApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BettingApp.Web.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        public UserController()
        {
            _walletRepository = new WalletRepository();
            _transactionRepository = new TransactionRepository();
        }
        private readonly WalletRepository _walletRepository;
        private readonly TransactionRepository _transactionRepository;

        [HttpGet]
        public IActionResult GetWallet(int userId)
        {
            return Ok(_walletRepository.GetWallet(userId));
        }

        [HttpPost]
        [Route("wallet")]
        public IActionResult FundsPayment(int walletId, double fundsToGrant)
        {
            var wereFundsGranted = _walletRepository.FundsPayment(walletId, fundsToGrant);
            if (!wereFundsGranted)
                return NotFound();
            _transactionRepository.AddTransaction(walletId, fundsToGrant, TransactionType.Payment);
            return Ok(true);
        }

        [HttpGet]
        [Route("wallet")]
        public IActionResult GetWalletTransactions(int walletId)
        {
            return Ok(_transactionRepository.GetWalletTransactions(walletId));
        }

        //[HttpGet]
        //[Route("bet")]
        //public IActionResult GetBets(int userId)
        //{

        //}

        //[HttpPost]
        //[Route("bet")]
        //public IActionResult MakeBet(int userId, Ticket ticketToPlace)
        //{

        //}


    }
}
