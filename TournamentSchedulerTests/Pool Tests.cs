using System.Linq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using UltimateFrisbeeTournamentScheduler;

namespace TournamentSchedulerTests
{
	public class PoolTests
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


		[TestCase("ExampleTeam1",0,0)]
		[TestCase("ExampleTeam5", 0, 1)]
		[TestCase("ExampleTeam9", 0, 2)]
		[TestCase("ExampleTeam13", 0, 3)]
		[TestCase("ExampleTeam2", 1, 0)]
		[TestCase("ExampleTeam6", 1, 1)]
		[TestCase("ExampleTeam10", 1, 2)]
		[TestCase("ExampleTeam14", 1, 3)]
		[TestCase("ExampleTeam3", 2, 0)]
		[TestCase("ExampleTeam7", 2, 1)]
		[TestCase("ExampleTeam11", 2, 2)]
		[TestCase("ExampleTeam15", 2, 3)]
		[TestCase("ExampleTeam4", 3, 0)]
		[TestCase("ExampleTeam8", 3, 1)]
		[TestCase("ExampleTeam12", 3, 2)]
		[TestCase("ExampleTeam16", 3, 3)]
		public void PoolsArePopulatedCorrectlyTest(string teamName, int poolNum,int poolPosition)
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
			var poolsToCheck = _tournamentMethods.RetrievePoolTeams(selectedTournamentId);
			Assert.AreEqual(poolsToCheck[poolNum][poolPosition].Name, teamName);
		}
	}
}