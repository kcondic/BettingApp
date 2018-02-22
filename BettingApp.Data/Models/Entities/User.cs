using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BettingApp.Data.Enums;

namespace BettingApp.Data.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
        public Wallet Wallet { get; set; }
    }
}
