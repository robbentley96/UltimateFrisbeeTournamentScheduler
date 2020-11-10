using System.Linq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using UltimateFrisbeeTournamentScheduler;

namespace TournamentSchedulerTests
{
	public class ScheduleTests
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
		public void ScheduleIsGeneratedCorrectlyTest()
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
			_tournamentMethods.PopulatePools(selectedTournamentId);
			_tournamentMethods.SchedulePoolMatches(selectedTournamentId);
			var pitch1Matches = _tournamentMethods.RetrieveMatches(selectedTournamentId, 1);
			var pitch2Matches = _tournamentMethods.RetrieveMatches(selectedTournamentId, 2);

			Assert.AreEqual(pitch1Matches[0].ToString(), "ExampleTeam1 vs ExampleTeam5 - 09:00");
			Assert.AreEqual(pitch1Matches[1].ToString(), "ExampleTeam2 vs ExampleTeam6 - 09:25");
			Assert.AreEqual(pitch1Matches[2].ToString(), "ExampleTeam3 vs ExampleTeam7 - 09:50");
			Assert.AreEqual(pitch1Matches[3].ToString(), "ExampleTeam4 vs ExampleTeam8 - 10:15");
			Assert.AreEqual(pitch1Matches[4].ToString(), "ExampleTeam1 vs ExampleTeam9 - 10:40");
			Assert.AreEqual(pitch1Matches[5].ToString(), "ExampleTeam2 vs ExampleTeam10 - 11:05");
			Assert.AreEqual(pitch1Matches[6].ToString(), "ExampleTeam3 vs ExampleTeam11 - 11:30");
			Assert.AreEqual(pitch1Matches[7].ToString(), "ExampleTeam4 vs ExampleTeam12 - 11:55");
			Assert.AreEqual(pitch1Matches[8].ToString(), "ExampleTeam1 vs ExampleTeam13 - 12:20");
			Assert.AreEqual(pitch1Matches[9].ToString(), "ExampleTeam2 vs ExampleTeam14 - 12:45");
			Assert.AreEqual(pitch1Matches[10].ToString(), "ExampleTeam3 vs ExampleTeam15 - 13:10");
			Assert.AreEqual(pitch1Matches[11].ToString(), "ExampleTeam4 vs ExampleTeam16 - 13:35");

			Assert.AreEqual(pitch2Matches[0].ToString(), "ExampleTeam9 vs ExampleTeam13 - 09:00");
			Assert.AreEqual(pitch2Matches[1].ToString(), "ExampleTeam10 vs ExampleTeam14 - 09:25");
			Assert.AreEqual(pitch2Matches[2].ToString(), "ExampleTeam11 vs ExampleTeam15 - 09:50");
			Assert.AreEqual(pitch2Matches[3].ToString(), "ExampleTeam12 vs ExampleTeam16 - 10:15");
			Assert.AreEqual(pitch2Matches[4].ToString(), "ExampleTeam5 vs ExampleTeam13 - 10:40");
			Assert.AreEqual(pitch2Matches[5].ToString(), "ExampleTeam6 vs ExampleTeam14 - 11:05");
			Assert.AreEqual(pitch2Matches[6].ToString(), "ExampleTeam7 vs ExampleTeam15 - 11:30");
			Assert.AreEqual(pitch2Matches[7].ToString(), "ExampleTeam8 vs ExampleTeam16 - 11:55");
			Assert.AreEqual(pitch2Matches[8].ToString(), "ExampleTeam5 vs ExampleTeam9 - 12:20");
			Assert.AreEqual(pitch2Matches[9].ToString(), "ExampleTeam6 vs ExampleTeam10 - 12:45");
			Assert.AreEqual(pitch2Matches[10].ToString(), "ExampleTeam7 vs ExampleTeam11 - 13:10");
			Assert.AreEqual(pitch2Matches[11].ToString(), "ExampleTeam8 vs ExampleTeam12 - 13:35");
		}

		[Test]
		public void SeededScheduleIsGeneratedCorrectlyTest()
		{
			var pitch1Matches = _tournamentMethods.ListSeedMatches(1);
			var pitch2Matches = _tournamentMethods.ListSeedMatches(2);

			Assert.AreEqual(pitch1Matches[0].ToString(), "6 v 11 - 09:00");
			Assert.AreEqual(pitch1Matches[1].ToString(), "8 v 9 - 09:25");
			Assert.AreEqual(pitch1Matches[2].ToString(), "11 v 14 - 09:50");
			Assert.AreEqual(pitch1Matches[3].ToString(), "12 v 13 - 10:15");
			Assert.AreEqual(pitch1Matches[4].ToString(), "4 v 5 - 10:40");
			Assert.AreEqual(pitch1Matches[5].ToString(), "3 v 6 - 11:05");
			Assert.AreEqual(pitch1Matches[6].ToString(), "14 v 15 - 11:30");
			Assert.AreEqual(pitch1Matches[7].ToString(), "10 v 11 - 11:55");
			Assert.AreEqual(pitch1Matches[8].ToString(), "6 v 7 - 12:20");
			Assert.AreEqual(pitch1Matches[9].ToString(), "2 v 3 - 12:45");
			Assert.AreEqual(pitch1Matches[10].ToString(), "12 v 14 - 13:10");
			Assert.AreEqual(pitch1Matches[11].ToString(), "9 v 10 - 13:35");
			Assert.AreEqual(pitch1Matches[12].ToString(), "5 v 6 - 14:00");
			Assert.AreEqual(pitch1Matches[13].ToString(), "3 v 4 - 14:25");

			Assert.AreEqual(pitch2Matches[0].ToString(), "7 v 10 - 09:00");
			Assert.AreEqual(pitch2Matches[1].ToString(), "5 v 12 - 09:25");
			Assert.AreEqual(pitch2Matches[2].ToString(), "10 v 15 - 09:50");
			Assert.AreEqual(pitch2Matches[3].ToString(), "9 v 16 - 10:15");
			Assert.AreEqual(pitch2Matches[4].ToString(), "1 v 8 - 10:40");
			Assert.AreEqual(pitch2Matches[5].ToString(), "2 v 7 - 11:05");
			Assert.AreEqual(pitch2Matches[6].ToString(), "13 v 16 - 11:30");
			Assert.AreEqual(pitch2Matches[7].ToString(), "9 v 12 - 11:55");
			Assert.AreEqual(pitch2Matches[8].ToString(), "5 v 8 - 12:20");
			Assert.AreEqual(pitch2Matches[9].ToString(), "1 v 4 - 12:45");
			Assert.AreEqual(pitch2Matches[10].ToString(), "15 v 16 - 13:10");
			Assert.AreEqual(pitch2Matches[11].ToString(), "11 v 12 - 13:35");
			Assert.AreEqual(pitch2Matches[12].ToString(), "7 v 8 - 14:00");
			Assert.AreEqual(pitch2Matches[13].ToString(), "1 v 2 - 14:25");
		}
	}
}