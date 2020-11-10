using System.Linq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using UltimateFrisbeeTournamentScheduler;
using System;

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
				var exampleTournament = db.Tournaments.Where(t => t.Name == "ExampleTournament123").FirstOrDefault();
				var exampleTeams = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.Name == "ExampleTournament123");
				if (exampleTournament != null)
				{
					int tournamentId = exampleTournament.TournamentId;
					var examplePools = db.Pools.Include(t => t.Tournament).Where(p => p.Tournament.TournamentId == tournamentId);
					var exampleMatches = db.Matches.Include(t => t.Tournament).Where(m => m.Tournament.TournamentId == tournamentId);
					db.RemoveRange(exampleTeams);
					db.RemoveRange(exampleMatches);
					db.RemoveRange(examplePools);
					db.RemoveRange(exampleTournament);
					db.SaveChanges();
				}
				
				
			}
		}

		[TearDown]
		public void TearDown()
		{
			using (var db = new TournamentContext())
			{
				var exampleTournament = db.Tournaments.Where(t => t.Name == "ExampleTournament123").FirstOrDefault();
				var exampleTeams = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.Name == "ExampleTournament123");
				if (exampleTournament != null)
				{
					int tournamentId = exampleTournament.TournamentId;
					var examplePools = db.Pools.Include(t => t.Tournament).Where(p => p.Tournament.TournamentId == tournamentId);
					var exampleMatches = db.Matches.Include(t => t.Tournament).Where(m => m.Tournament.TournamentId == tournamentId);
					db.RemoveRange(exampleTeams);
					db.RemoveRange(exampleMatches);
					db.RemoveRange(examplePools);
					db.RemoveRange(exampleTournament);
					db.SaveChanges();
				}


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
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
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


		[Test]
		public void InvalidTournamentRemoved_NoExceptionIsThrownTest()
		{
			_tournamentMethods.RemoveTournament(-1);
		}

		[Test]
		public void InvalidTeamRemoved_NoExceptionIsThrownTest()
		{
			_tournamentMethods.RemoveTeam(-1);
		}


		[Test]
		public void WhenTournamentNameIsChangedToEmpty_TheDatabaseDoesNotUpdateTest()
		{
			int id;
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				db.Tournaments.Add(newTournament);
				db.SaveChanges();
				id = newTournament.TournamentId;
			}

			_tournamentMethods.UpdateTournamentName(id, "");
			using (var db = new TournamentContext())
			{
				Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == id).FirstOrDefault();
				Assert.AreEqual(selectedTournament.Name, "ExampleTournament123");
			}

		}

		[Test]
		public void WhenTeamNameIsChangedToEmptyString_TheDatabaseDoesNotUpdateTest()
		{
			int id;
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				Team newTeam = new Team() { Name = "ExampleTeam123", Tournament = newTournament };
				db.Tournaments.Add(newTournament);
				db.Teams.Add(newTeam);
				db.SaveChanges();
				id = newTeam.TeamId;
			}

			_tournamentMethods.UpdateTeamName(id, "");
			using (var db = new TournamentContext())
			{
				Team selectedTeam = db.Teams.Where(t => t.TeamId == id).FirstOrDefault();
				Assert.AreEqual(selectedTeam.Name, "ExampleTeam123");
			}

		}

		[Test]
		public void WhenMatchIsAdded_DetailsAreCorrectTest()
		{
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				Team team1 = new Team() { Name = "ExampleTeam123" };
				Team team2 = new Team() { Name = "ExampleTeam456" };
				db.Tournaments.Add(newTournament);
				db.Teams.Add(team1);
				db.Teams.Add(team2);
				db.SaveChanges();
				_tournamentMethods.AddMatch(newTournament.TournamentId,team1.TeamId,team2.TeamId,1, new DateTime(2020,5,1,8,30,0));
				Match selectedMatch = db.Matches.Include(m => m.Tournament).Where(m => m.Tournament.TournamentId == newTournament.TournamentId).OrderByDescending(m => m.MatchId).First();
				Assert.AreEqual(selectedMatch.Team1.Name, "ExampleTeam123");
				Assert.AreEqual(selectedMatch.Team2.Name, "ExampleTeam456");
				Assert.AreEqual(selectedMatch.PitchNumber, 1);
				Assert.AreEqual(selectedMatch.Time.ToString("HH:mm"), "08:30");
			}
		}

		[Test]
		public void WhenMatchIsRemoved_ItIsNoLongerInTheDatabase()
		{
			int matchId;
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				Team team1 = new Team() { Name = "ExampleTeam123" };
				Team team2 = new Team() { Name = "ExampleTeam456" };
				Match exampleMatch = new Match(){ Team1 = team1, Team2 = team2, Tournament = newTournament};
				db.Tournaments.Add(newTournament);
				db.Teams.Add(team1);
				db.Teams.Add(team2);
				db.Matches.Add(exampleMatch);
				db.SaveChanges();
				matchId = exampleMatch.MatchId;
				
			}
			_tournamentMethods.RemoveMatch(matchId);
			using (var db = new TournamentContext())
			{
				Match selectedMatch = db.Matches.Include(m => m.Team1).Where(m => m.Team1.Name == "ExampleTeam123").FirstOrDefault();
				Assert.AreEqual(selectedMatch, null);
			}
		}
	}
}