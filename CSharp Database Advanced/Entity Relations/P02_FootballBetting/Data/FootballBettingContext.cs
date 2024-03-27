using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {

        }

        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<PlayerStatistic> PlayersStatistics { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.SqlConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigTeamEntity(modelBuilder);

            ConfigColorEntity(modelBuilder);

            ConfigTownEntity(modelBuilder);

            ConfigCountryEntity(modelBuilder);

            ConfigPlayerEntity(modelBuilder);

            ConfigPositionEntity(modelBuilder);

            ConfigPlayerStatisticEntity(modelBuilder);

            ConfigGameEntity(modelBuilder);

            ConfigBetEntity(modelBuilder);

            ConfigUserEntity(modelBuilder);
        }

        private void ConfigUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>(entity =>
                {
                    entity
                        .HasKey(u => u.UserId);

                    entity
                        .Property(u => u.Username)
                        .HasMaxLength(30)
                        .IsRequired();

                    entity
                        .Property(p => p.Password)
                        .HasMaxLength(15)
                        .IsRequired();

                    entity
                        .Property(e => e.Email)
                        .HasMaxLength(80)
                        .IsRequired();

                    entity
                        .Property(n => n.Name)
                        .HasMaxLength(50)
                        .IsRequired();
                });
        }

        private void ConfigBetEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Bet>(entity =>
                {
                    entity
                        .HasKey(b => b.BetId);

                    entity
                        .HasOne(g => g.Game)
                        .WithMany(b => b.Bets);

                    entity
                        .HasOne(u => u.User)
                        .WithMany(b => b.Bets);

                    entity
                        .Property(p => p.Prediction)
                        .IsRequired();

                    entity
                        .Property(u => u.UserId)
                        .IsRequired();

                    entity
                        .Property(g => g.GameId)
                        .IsRequired();
                });
        }

        private void ConfigGameEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Game>(entity =>
                {
                    entity
                        .HasKey(g => g.GameId);
                });
        }

        private void ConfigPlayerStatisticEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PlayerStatistic>()
                .HasKey(ps => new { ps.GameId, ps.PlayerId });

            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(g => g.Game)
                .WithMany(ps => ps.PlayersStatistics)
                .HasForeignKey(ps => ps.GameId);

            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(p => p.Player)
                .WithMany(ps => ps.PlayersStatistics)
                .HasForeignKey(ps => ps.PlayerId);
        }

        private void ConfigPositionEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Position>(entity =>
                {
                    entity
                        .HasKey(p => p.PositionId);

                    entity
                        .Property(n => n.Name)
                        .HasMaxLength(50)
                        .IsRequired();
                });
        }

        private void ConfigPlayerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Player>(entity =>
                {
                    entity
                        .HasKey(p => p.PlayerId);

                    entity
                        .HasOne(t => t.Team)
                        .WithMany(p => p.Players);

                    entity
                        .HasOne(p => p.Position)
                        .WithMany(p => p.Players);

                    entity
                        .Property(n => n.Name)
                        .HasMaxLength(50)
                        .IsRequired()
                        .IsUnicode();

                    entity
                        .Property(sn => sn.SquadNumber)
                        .IsRequired();

                    entity
                      .HasOne(t => t.Town)
                      .WithMany(p => p.Players)
                      .HasForeignKey(p => p.TownId);
                });
        }

        private void ConfigCountryEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Country>(entity =>
                {
                    entity
                        .HasKey(c => c.CountryId);

                    entity
                        .Property(n => n.Name)
                        .HasMaxLength(30)
                        .IsRequired();
                });
        }

        private void ConfigTownEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Town>(entity =>
                {
                    entity
                        .HasKey(t => t.TownId);

                    entity
                        .HasOne(c => c.Country)
                        .WithMany(t => t.Towns);

                    entity
                        .Property(n => n.Name)
                        .HasMaxLength(50)
                        .IsRequired();
                });
        }

        private void ConfigColorEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Color>(entity =>
                {
                    entity
                        .HasKey(c => c.ColorId);

                    entity
                        .Property(n => n.Name)
                        .HasMaxLength(20)
                        .IsRequired();
                });
        }

        private void ConfigTeamEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Team>(entity =>
                {
                    entity
                        .HasKey(t => t.TeamId);

                    entity
                        .HasOne(t => t.Town)
                        .WithMany(t => t.Teams);

                    entity
                        .HasOne(pkc => pkc.PrimaryKitColor)
                        .WithMany(pkt => pkt.PrimaryKitTeams)
                        .HasForeignKey(pkc => pkc.PrimaryKitColorId)
                        .OnDelete(DeleteBehavior.Restrict);

                    entity
                        .HasOne(skc => skc.SecondaryKitColor)
                        .WithMany(ski => ski.SecondaryKitTeams)
                        .HasForeignKey(skc => skc.SecondaryKitColorId)
                        .OnDelete(DeleteBehavior.Restrict);

                    entity
                        .Property(n => n.Name)
                        .HasMaxLength(25)
                        .IsUnicode(false)
                        .IsRequired();

                    entity
                        .Property(lu => lu.LogoUrl)
                        .IsRequired();

                    entity
                        .Property(i => i.Initials)
                        .HasColumnType("CHAR(3)")
                        .IsRequired();
                });
        }
    }
}