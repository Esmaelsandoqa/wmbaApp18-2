using Microsoft.EntityFrameworkCore;
using wmbaApp.Models;
using wmbaApp.ViewModels;

namespace wmbaApp.Data
{
    /// <summary>
    /// Context class for code first DB approach
    /// - Nadav Hilu
    /// </summary>
    public class WmbaContext : DbContext
    {
        public WmbaContext(DbContextOptions<WmbaContext> options) : base(options)
        { 
        }

        public DbSet<Division> Divisions { get; set; }
        public DbSet<DivisionCoach> DivisionCoaches { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<GameTeam> GameTeams { get; set; }
        public DbSet<PlayersLineUp> PlayersLineUp { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Statistic> Statistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Unique constraint for division names
            modelBuilder.Entity<Division>()
                .HasIndex(d => d.DivName)
                .IsUnique();

            //Unique constraint for team names
            modelBuilder.Entity<Team>()
                .HasIndex(t => t.TmName)
                .IsUnique();

            //Unique constraint for team abbreviations
            modelBuilder.Entity<Team>()
                .HasIndex(t => t.TmAbbreviation)
                .IsUnique();

            //Unique constraint for position names
            modelBuilder.Entity<Position>()
                .HasIndex(p => p.PosName)
                .IsUnique();

            //Many to many junction table
            modelBuilder.Entity<DivisionCoach>()
                .HasKey(dc => new { dc.DivisionID, dc.CoachID });

            //Many to many junction table
            modelBuilder.Entity<GameTeam>()
                .HasKey(gt => new { gt.TeamID, gt.GameID });

            //Many to many junction table
            modelBuilder.Entity<PlayerPosition>()
                .HasKey(pp => new { pp.PlayerID, pp.PositionID });

            //Prevent cascade delete from division to teams
            modelBuilder.Entity<Division>()
                .HasMany<Team>(d => d.Teams)
                .WithOne(t => t.Division)
                .HasForeignKey(t => t.DivisionID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent cascade delete from division to teams
            modelBuilder.Entity<Team>()
                .HasMany<Player>(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamID)
                .OnDelete(DeleteBehavior.Restrict);


            //Prevent cascade delete from division to coaches
            modelBuilder.Entity<Division>()
                .HasMany<DivisionCoach>(d => d.DivisionCoaches)
                .WithOne(c => c.Division)
                .HasForeignKey(c => c.DivisionID)
                .OnDelete(DeleteBehavior.Restrict);

            //Prevent cascade delete from position to player_position
            modelBuilder.Entity<Position>()
                .HasMany<PlayerPosition>(p => p.PlayerPositions)
                .WithOne(pl => pl.Position)
                .HasForeignKey(pl => pl.PositionID)
                .OnDelete(DeleteBehavior.Restrict);

            //Unique composite PK for division_coach
            modelBuilder.Entity<DivisionCoach>()
                .HasIndex(dc => new { dc.CoachID, dc.DivisionID})
                .IsUnique();

            //Unique composite PK for game_team
            modelBuilder.Entity<GameTeam>()
                .HasIndex(gt => new { gt.TeamID, gt.GameID })
                .IsUnique();

            //Unique composite PK for game_team
            modelBuilder.Entity<PlayerPosition>()
                .HasIndex(pp => new { pp.PlayerID })
                .IsUnique();

            //Unique composite PK for Player Jersey Number
            modelBuilder.Entity<Player>()
                .HasIndex(pp => new { pp.PlyrJerseyNumber, pp.TeamID })
                .IsUnique();

            modelBuilder.Entity<PlayersLineUp>()
       .HasMany(plu => plu.Players)
       .WithOne(p => p.playersLineUp)
       .HasForeignKey(p => p.playersLineUpId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayersLineUp>()
                .HasMany(plu => plu.CompetatorPlayers)
                .WithOne()
                .HasForeignKey(p => p.playersLineUpId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
