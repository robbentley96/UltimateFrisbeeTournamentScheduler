using System.Linq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using UltimateFrisbeeTournamentScheduler;

namespace TournamentSchedulerTests
{
	public class CRUDTests
	{

		TournamentMethods _tournamentMethods = new TournamentMethods();
		
		[SetUp]
		public void Setup()
		{
			using (var db = new TournamentContext())
			{
				var exampleTeams = db.Teams.Include(t => t.Tournament).Where(t => t.Name == "ExampleTeam123" && t.Tournament.Name == "ExampleTournament123");
				db.Teams.RemoveRange(exampleTeams);
				var exampleTournaments = db.Tournaments.Where(t => t.Name == "ExampleTournament123");
				db.Tournaments.RemoveRange(exampleTournaments);
				db.SaveChanges();
			}
		}

		[TearDown]
		public void TearDown()
		{
			using (var db = new TournamentContext())
			{
				var exampleTeams = db.Teams.Include(t => t.Tournament).Where(t => t.Name == "ExampleTeam123" && t.Tournament.Name == "ExampleTournament123");
				db.RemoveRange(exampleTeams);
				var exampleTournaments = db.Tournaments.Where(t => t.Name == "ExampleTournament123");
				db.RemoveRange(exampleTournaments);
				db.SaveChanges();
			}
		}


		[Test]
		public void WhenTournamentIsAdded_TheNumberOfTournamentsIncreasesBy1Test()
		{
			int tournamentCountBefore = _tournamentMethods.RetrieveTournaments().Count();
			_tournamentMethods.AddTournament("ExampleTournament123");
			int tournamentCountAfter = _tournamentMethods.RetrieveTournaments().Count();
			Assert.AreEqual(tournamentCountBefore + 1, tournamentCountAfter);
		}


		[Test]
		public void WhenTournamentNameIsEmptyString_NumberOfTournamentsDoesNotIncreaseTest()
		{
			int tournamentCountBefore = _tournamentMethods.RetrieveTournaments().Count();
			_tournamentMethods.AddTournament("");
			int tournamentCountAfter = _tournamentMethods.RetrieveTournaments().Count();
			Assert.AreEqual(tournamentCountBefore, tournamentCountAfter);
		}


		[Test]
		public void WhenTournamentIsRemoved_ItNoLongerAppearsInTheDatabaseTest()
		{
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				db.Tournaments.Add(newTournament);
				_tournamentMethods.RemoveTournament(newTournament.TournamentId);
				Tournament selectedTournament = db.Tournaments.Where(t => t.Name == "ExampleTournament123").FirstOrDefault();
				Assert.AreEqual(selectedTournament, null);
			}
		}

		[Test]
		public void WhenTeamIsAdded_TheNumberOfTeamsIncreasesBy1Test()
		{
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				db.Tournaments.Add(newTournament);
				db.SaveChanges();
				int teamCountBefore = _tournamentMethods.RetrieveTeams(newTournament.TournamentId).Count();
				_tournamentMethods.AddTeam(newTournament.TournamentId,"ExampleTeam123");
				int teamCountAfter = _tournamentMethods.RetrieveTeams(newTournament.TournamentId).Count();
				Assert.AreEqual(teamCountBefore + 1, teamCountAfter);

			}
		}


		[Test]
		public void WhenTeamNameIsEmptyString_NumberOfTeamsDoesNotIncreaseTest()
		{
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				db.Tournaments.Add(newTournament);
				db.SaveChanges();
				int teamCountBefore = _tournamentMethods.RetrieveTeams(newTournament.TournamentId).Count();
				_tournamentMethods.AddTeam(newTournament.TournamentId, "");
				int teamCountAfter = _tournamentMethods.RetrieveTeams(newTournament.TournamentId).Count();
				Assert.AreEqual(teamCountBefore, teamCountAfter);

			}
		}


		[Test]
		public void WhenTeamIsRemoved_ItNoLongerAppearsInTheDatabaseTest()
		{
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				db.Tournaments.Add(newTournament);
				Team newTeam = new Team() { Name = "ExampleTeam123", Tournament = newTournament };
				db.Teams.Add(newTeam);
				db.SaveChanges();
				_tournamentMethods.RemoveTeam(newTeam.TeamId);
				Team selectedTeam = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.Name == "ExampleTournament123" && t.Name == "ExampleTeam123").FirstOrDefault();
				Assert.AreEqual(selectedTeam, null);
			}
		}

		[Test]
		public void WhenTournamentIsAdded_NameIsCorrectTest()
		{
			using (var db = new TournamentContext())
			{
				_tournamentMethods.AddTournament("ExampleTournament123");
				Tournament selectedTournament = db.Tournaments.OrderByDescending(t => t.TournamentId).First();
				Assert.AreEqual(selectedTournament.Name, "ExampleTournament123");
			}
		}

		[Test]
		public void WhenTeamIsAdded_NameIsCorrectTest()
		{
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				db.Tournaments.Add(newTournament);
				db.SaveChanges();
				_tournamentMethods.AddTeam(newTournament.TournamentId, "ExampleTeam123");
				Team selectedTeam = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == newTournament.TournamentId).OrderByDescending(t => t.TeamId).First();
				Assert.AreEqual(selectedTeam.Name, "ExampleTeam123");
			}
		}

		

			[Test]
		public void WhenTournamentNameIsChanged_TheDatabaseUpdatesTest()
		{
			int id;
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament456" };
				db.Tournaments.Add(newTournament);
				db.SaveChanges();
				id = newTournament.TournamentId;
			}

			_tournamentMethods.UpdateTournamentName(id, "ExampleTournament123");
			using (var db = new TournamentContext())
			{
				Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == id).FirstOrDefault();
				Assert.AreEqual(selectedTournament.Name, "ExampleTournament123");
			}

		}

		[Test]
		public void WhenTeamNameIsChanged_TheDatabaseUpdatesTest()
		{
			int id;
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament456" };
				Team newTeam = new Team() { Name = "ExampleTeam456", Tournament = newTournament };
				db.Tournaments.Add(newTournament);
				db.Teams.Add(newTeam);
				db.SaveChanges();
				id = newTeam.TeamId;
			}

			_tournamentMethods.UpdateTeamName(id, "ExampleTeam123");
			using (var db = new TournamentContext())
			{
				Team selectedTeam = db.Teams.Where(t => t.TeamId == id).FirstOrDefault();
				Assert.AreEqual(selectedTeam.Name, "ExampleTeam123");
			}

		}


	}
}