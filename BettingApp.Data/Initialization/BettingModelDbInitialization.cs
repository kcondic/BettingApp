using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BettingApp.Data.Enums;
using BettingApp.Data.Models;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Data.Initialization
{
    public class BettingModelDbInitialization : CreateDatabaseIfNotExists<BettingContext>
    {
        protected override void Seed(BettingContext context)
        {
            var sports = new List<Sport>()
            {
                new Sport() {Name = "Nogomet", IsDrawPossible = true},
                new Sport() {Name = "Košarka", IsDrawPossible = true},
                new Sport() {Name = "Rukomet", IsDrawPossible = true},
                new Sport() {Name = "Tenis", IsDrawPossible = false}
            };

            var teams = new List<Team>()
            {
                new Team() {Name = "HNK Hajduk š.d.d.", Sport = sports[0]},
                new Team() {Name = "Gornik Zabrze", Sport = sports[0]},
                new Team() {Name = "SL Benfica", Sport = sports[0]},
                new Team() {Name = "AS Saint Etienne", Sport = sports[0]},
                new Team() {Name = "KK Split", Sport = sports[1]},
                new Team() {Name = "KK Zadar", Sport = sports[1]},
                new Team() {Name = "KK Cibona", Sport = sports[1]},
                new Team() {Name = "KK Cedevita", Sport = sports[1]},
                new Team() {Name = "PPD Zagreb", Sport = sports[2]},
                new Team() {Name = "RK Vardar Skopje", Sport = sports[2]},
                new Team() {Name = "HC Rhein-Neckar Löwen", Sport = sports[2]},
                new Team() {Name = "Vive Targi Kielce", Sport = sports[2]},
                new Team() {Name = "Marin Čilić", Sport = sports[3]},
                new Team() {Name = "Roger Federer", Sport = sports[3]},
                new Team() {Name = "Rafael Nadal", Sport = sports[3]},
                new Team() {Name = "Juan Martin Del Potro", Sport = sports[3]}
            };

            var matches = new List<Match>()
            {
                new Match()
                {
                    HomeTeam = teams[0],
                    AwayTeam = teams[1],
                    HomeWinOdd = 1.5,
                    DrawOdd = 2.2,
                    AwayWinOdd = 3.0,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(2)
                },
                new Match()
                {
                    HomeTeam = teams[3],
                    AwayTeam = teams[0],
                    HomeWinOdd = 1.3,
                    DrawOdd = 2.5,
                    AwayWinOdd = 3.75,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(2)
                },
                new Match()
                {
                    HomeTeam = teams[1],
                    AwayTeam = teams[2],
                    HomeWinOdd = 2.5,
                    DrawOdd = 2.8,
                    AwayWinOdd = 2.5,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(1)
                },
                new Match()
                {
                    HomeTeam = teams[4],
                    AwayTeam = teams[6],
                    HomeWinOdd = 2.2,
                    DrawOdd = 18,
                    AwayWinOdd = 1.6,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(2)
                },
                new Match()
                {
                    HomeTeam = teams[5],
                    AwayTeam = teams[7],
                    HomeWinOdd = 2.2,
                    DrawOdd = 18,
                    AwayWinOdd = 1.6,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(1)
                },
                new Match()
                {
                    HomeTeam = teams[9],
                    AwayTeam = teams[8],
                    HomeWinOdd = 1.15,
                    DrawOdd = 2.5,
                    AwayWinOdd = 5,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(1)
                },
                new Match()
                {
                    HomeTeam = teams[10],
                    AwayTeam = teams[8],
                    HomeWinOdd = 1.45,
                    DrawOdd = 2.2,
                    AwayWinOdd = 3.5,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(6)
                },
                new Match()
                {
                    HomeTeam = teams[12],
                    AwayTeam = teams[13],
                    HomeWinOdd = 2.6,
                    AwayWinOdd = 1.8,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(1)
                },
                new Match()
                {
                    HomeTeam = teams[14],
                    AwayTeam = teams[15],
                    HomeWinOdd = 1.9,
                    AwayWinOdd = 1.9,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(1)
                },
                new Match()
                {
                    HomeTeam = teams[12],
                    AwayTeam = teams[15],
                    HomeWinOdd = 2.0,
                    AwayWinOdd = 1.8,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(3)
                },
                new Match()
                {
                    HomeTeam = teams[13],
                    AwayTeam = teams[14],
                    HomeWinOdd = 1.5,
                    AwayWinOdd = 2.25,
                    TimeOfStart = DateTime.Now + TimeSpan.FromDays(3)
                }
            };

            var wallets = new List<Wallet>()
            {
                new Wallet() {Funds = 150},
                new Wallet() {Funds = 2000}
            };

            var users = new List<User>()
            {
                new User() {FirstName = "Krešimir", LastName = "Čondić", UserName = "kcondic", Role = Role.User, Wallet = wallets[0]},
                new User() {FirstName = "Mario", LastName = "Čeprnja", UserName = "mceprnja", Role = Role.User, Wallet = wallets[1]},
                new User() {FirstName = "Vladimir", LastName = "Vrankulj", UserName = "vvrankulj", Role = Role.Admin}
            };

            context.Sports.AddRange(sports);
            context.Teams.AddRange(teams);
            context.Matches.AddRange(matches);
            context.Wallets.AddRange(wallets);
            context.Users.AddRange(users);
            base.Seed(context);
        }
    }
}
