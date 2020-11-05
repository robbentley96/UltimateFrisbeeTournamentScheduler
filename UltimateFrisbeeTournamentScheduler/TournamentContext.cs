using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;

namespace UltimateFrisbeeTournamentScheduler
{
	public class TournamentContext : DbContext
	{
		public DbSet<Tournament> Tournaments { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Match> Matches { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=TournamentScheduling;");
	}

	public class Tournament
	{
		public int TournamentId { get; set; }
		public string Name { get; set; }
		public List<Team> Teams { get; set; } = new List<Team>();
		public List<Match> Matches { get; set; } = new List<Match>();
	}

	public class Team
	{
		public int TeamId { get; set; }
		public string Name { get; set; }
		public Tournament Tournament { get; set; }
	}

	public class Match
	{
		public int MatchId { get; set; }
		public Team Team1 { get; set; }
		public Team Team2 { get; set; }
		public DateTime Time { get; set; }
	}
}
