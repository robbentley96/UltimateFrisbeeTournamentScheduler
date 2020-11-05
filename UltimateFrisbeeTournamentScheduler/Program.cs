using System;
using System.Linq;

namespace UltimateFrisbeeTournamentScheduler
{
	class Program
	{
		static void Main(string[] args)
		{
			TournamentMethods _tournamentMethods = new TournamentMethods();
			
			using (var db = new TournamentContext())
			{
				Tournament newTournament = new Tournament() { Name = "ExampleTournament123" };
				db.Add(newTournament);
				db.SaveChanges();
				int teamCountBefore = _tournamentMethods.RetrieveTeams(newTournament.TournamentId).Count();
				_tournamentMethods.AddTeam(newTournament.TournamentId, "ExampleTeam123");
				int teamCountAfter = _tournamentMethods.RetrieveTeams(newTournament.TournamentId).Count();
			

			}
		}
	}
}
 