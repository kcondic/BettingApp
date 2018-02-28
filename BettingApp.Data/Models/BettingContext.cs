using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using BettingApp.Data.Initialization;
using BettingApp.Data.Models.Entities;

namespace BettingApp.Data.Models
{
    public class BettingContext : DbContext
    {
        public BettingContext() : base("BettingDatabase")
        {
            Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer(new BettingModelDbInitialization());
        }

        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketMatch> TicketMatches { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Match>()
                .HasRequired(x => x.HomeTeam)
                .WithMany(x => x.HomeMatches)
                .HasForeignKey(x => x.HomeTeamId);
            modelBuilder.Entity<Match>()
                .HasRequired(x => x.AwayTeam)
                .WithMany(x => x.AwayMatches)
                .HasForeignKey(x => x.AwayTeamId);

            modelBuilder.Entity<TicketMatch>()
                .HasKey(x => new {x.TicketId, x.MatchId});
            modelBuilder.Entity<TicketMatch>()
                .HasRequired(x => x.Ticket)
                .WithMany(x => x.TicketMatches)
                .HasForeignKey(x => x.TicketId);
            modelBuilder.Entity<TicketMatch>()
                .HasRequired(x => x.Match)
                .WithMany(x => x.TicketMatches)
                .HasForeignKey(x => x.MatchId);

            modelBuilder.Entity<User>()
                .HasOptional(x => x.Wallet)
                .WithRequired(x => x.Owner);

            base.OnModelCreating(modelBuilder);
        }
    }
}
