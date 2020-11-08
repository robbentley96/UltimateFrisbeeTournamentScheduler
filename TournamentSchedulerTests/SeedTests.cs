using System.Linq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using UltimateFrisbeeTournamentScheduler;

namespace TournamentSchedulerTests
{
	public class SeedTests
	{

		TournamentMethods _tournamentMethods = new TournamentMethods();

		[SetUp]
		public void Setup()
		{
			using (var db = new TournamentContext())
			{
				var exampleTournaments = db.Tournaments.Where(t => t.Name == "ExampleTournament123");
				foreach (Tournament exampleTournament in exampleTournaments)
				{
					_tournamentMethods.RemoveTournament(exampleTournament.TournamentId);
				}
			}
				
			
		}

		[TearDown]
		public void TearDown()
		{
			using (var db = new TournamentContext())
			{
				var exampleTournaments = db.Tournaments.Where(t => t.Name == "ExampleTournament123");
				foreach (Tournament exampleTournament in exampleTournaments)
				{
					_tournamentMethods.RemoveTournament(exampleTournament.TournamentId);
				}
			}
		}


		[Test]
		public void WhenTeamsAreAddedToTournament_TheTeamsSeedIsSet1HigherThanTheLastTest()
		{
			_tournamentMethods.AddTournament("ExampleTournament123");
			int selectedTournamentId;
			using (var db = new TournamentContext())
			{
				selectedTournamentId = db.Tournaments.OrderByDescending(t => t.TournamentId).First().TournamentId;
			}
			for (int i = 1; i <= 16; i++)
			{
				_tournamentMethods.AddTeam(selectedTournamentId, $"ExampleTeam{i}");
			}
			var teamsToCheck = _tournamentMethods.RetrieveTeams(selectedTournamentId);
			Assert.AreEqual(teamsToCheck[0].Seed, 1);
			Assert.AreEqual(teamsToCheck[1].Seed, 2);
			Assert.AreEqual(teamsToCheck[2].Seed, 3);
			Assert.AreEqual(teamsToCheck[3].Seed, 4);
			Assert.AreEqual(teamsToCheck[4].Seed, 5);
			Assert.AreEqual(teamsToCheck[5].Seed, 6);
			Assert.AreEqual(teamsToCheck[6].Seed, 7);
			Assert.AreEqual(teamsToCheck[7].Seed, 8);
			Assert.AreEqual(teamsToCheck[8].Seed, 9);
			Assert.AreEqual(teamsToCheck[9].Seed, 10);
			Assert.AreEqual(teamsToCheck[10].Seed, 11);
			Assert.AreEqual(teamsToCheck[11].Seed, 12);
			Assert.AreEqual(teamsToCheck[12].Seed, 13);
			Assert.AreEqual(teamsToCheck[13].Seed, 14);
			Assert.AreEqual(teamsToCheck[14].Seed, 15);
			Assert.AreEqual(teamsToCheck[15].Seed, 16);
		}

		[Test]
		public void NoMoreThan16TeamsCanBeAddedToATournamentTest()
		{
			_tournamentMethods.AddTournament("ExampleTournament123");
			int selectedTournamentId;
			using (var db = new TournamentContext())
			{
				selectedTournamentId = db.Tournaments.OrderByDescending(t => t.TournamentId).First().TournamentId;
			}
			for (int i = 0; i < 20; i++)
			{
				_tournamentMethods.AddTeam(selectedTournamentId, $"ExampleTeam{i}");
			}
			Assert.AreEqual(_tournamentMethods.RetrieveTeams(selectedTournamentId).Count(), 16);
		}

		[Test]
		public void GainSeedFrom16thMakesTeam15thTest()
		{
			_tournamentMethods.AddTournament("ExampleTournament123");
			int selectedTournamentId;
			int selectedTeamId;
			int updatedSeed;
			using (var db = new TournamentContext())
			{
				selectedTournamentId = db.Tournaments.OrderByDescending(t => t.TournamentId).First().TournamentId;
			}
			for (int i = 1; i <= 16; i++)
			{
				_tournamentMethods.AddTeam(selectedTournamentId, $"ExampleTeam{i}");
			}
			using (var db = new TournamentContext())
			{
				selectedTeamId = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == selectedTournamentId && t.Name == "ExampleTeam16").FirstOrDefault().TeamId;
			}
			_tournamentMethods.GainSeed(selectedTeamId);
			using (var db = new TournamentContext())
			{
				updatedSeed = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == selectedTournamentId && t.Name == "ExampleTeam16").FirstOrDefault().Seed;
			}
			Assert.AreEqual(updatedSeed, 15);
		}

		[Test]
		public void DropSeedFrom1stMakesTeam2ndTest()
		{
			_tournamentMethods.AddTournament("ExampleTournament123");
			int selectedTournamentId;
			int selectedTeamId;
			int updatedSeed;
			using (var db = new TournamentContext())
			{
				selectedTournamentId = db.Tournaments.OrderByDescending(t => t.TournamentId).First().TournamentId;
			}
			for (int i = 1; i <= 16; i++)
			{
				_tournamentMethods.AddTeam(selectedTournamentId, $"ExampleTeam{i}");
			}
			using (var db = new TournamentContext())
			{
				selectedTeamId = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == selectedTournamentId && t.Name == "ExampleTeam1").FirstOrDefault().TeamId;
			}
			_tournamentMethods.DropSeed(selectedTeamId);
			using (var db = new TournamentContext())
			{
				updatedSeed = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == selectedTournamentId && t.Name == "ExampleTeam1").FirstOrDefault().Seed;
			}
			Assert.AreEqual(updatedSeed, 2);
		}

		[Test]
		public void GainSeedFrom1stMakesTeam1stTest()
		{
			_tournamentMethods.AddTournament("ExampleTournament123");
			int selectedTournamentId;
			int selectedTeamId;
			int updatedSeed;
			using (var db = new TournamentContext())
			{
				selectedTournamentId = db.Tournaments.OrderByDescending(t => t.TournamentId).First().TournamentId;
			}
			for (int i = 1; i <= 16; i++)
			{
				_tournamentMethods.AddTeam(selectedTournamentId, $"ExampleTeam{i}");
			}
			using (var db = new TournamentContext())
			{
				selectedTeamId = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == selectedTournamentId && t.Name == "ExampleTeam1").FirstOrDefault().TeamId;
			}
			_tournamentMethods.GainSeed(selectedTeamId);
			using (var db = new TournamentContext())
			{
				updatedSeed = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == selectedTournamentId && t.Name == "ExampleTeam1").FirstOrDefault().Seed;
			}
			Assert.AreEqual(updatedSeed, 1);
		}

		[Test]
		public void DropSeedFrom16thMakesTeam16thTest()
		{
			_tournamentMethods.AddTournament("ExampleTournament123");
			int selectedTournamentId;
			int selectedTeamId;
			int updatedSeed;
			using (var db = new TournamentContext())
			{
				selectedTournamentId = db.Tournaments.OrderByDescending(t => t.TournamentId).First().TournamentId;
			}
			for (int i = 1; i <= 16; i++)
			{
				_tournamentMethods.AddTeam(selectedTournamentId, $"ExampleTeam{i}");
			}
			using (var db = new TournamentContext())
			{
				selectedTeamId = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == selectedTournamentId && t.Name == "ExampleTeam16").FirstOrDefault().TeamId;
			}
			_tournamentMethods.DropSeed(selectedTeamId);
			using (var db = new TournamentContext())
			{
				updatedSeed = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == selectedTournamentId && t.Name == "ExampleTeam16").FirstOrDefault().Seed;
			}
			Assert.AreEqual(updatedSeed, 16);
		}
	}
}