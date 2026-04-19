using Microsoft.EntityFrameworkCore;
using Mundialito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Infrastructure.Persistence
{
    public class MundialitoDbContext : DbContext
    {
        public MundialitoDbContext(DbContextOptions<MundialitoDbContext> options) : base(options)
        {
        }

        public DbSet<Tournament> Tournaments => Set<Tournament>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Match> Matches => Set<Match>();
        public DbSet<GoalMatch> GoalMatches => Set<GoalMatch>();
        public DbSet<TeamTournament> TeamTournaments => Set<TeamTournament>();
        public DbSet<IdempotencyRequest> IdempotencyRequests => Set<IdempotencyRequest>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamTournament>().HasKey(teamTournament => new { teamTournament.Id, teamTournament.TeamId });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasOne(match => match.HomeTeam)
                .WithMany()
                .HasForeignKey(match => match.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(match => match.VisitingTeam)
                .WithMany()
                .HasForeignKey(match => match.VisitingTeamId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
