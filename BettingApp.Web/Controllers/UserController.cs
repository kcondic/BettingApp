﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BettingApp.Data.Enums;
using BettingApp.Data.Models.Entities;
using BettingApp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BettingApp.Web.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        public UserController()
        {
            _walletRepository = new WalletRepository();
            _transactionRepository = new TransactionRepository();
            _ticketRepository = new TicketRepository();
        }
        private readonly WalletRepository _walletRepository;
        private readonly TransactionRepository _transactionRepository;
        private readonly TicketRepository _ticketRepository;

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

        [HttpGet]
        [Route("bet")]
        public IActionResult GetTickets(int walletId)
        {
            return Ok(_ticketRepository.GetTickets(walletId));
        }

        [HttpPost]
        [Route("bet")]
        public IActionResult PlaceBet([FromBody]Ticket ticketToPlace)
        {
            var wasTicketPlaced = _ticketRepository.PlaceBet(ticketToPlace);
            if (!wasTicketPlaced)
                return Forbid();
            return Ok();
        }
    }
}