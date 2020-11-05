using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace UltimateFrisbeeTournamentScheduler
{
	public class TournamentMethods
	{
		public void AddTournament(string name)
		{
			if (name != "")
			{
				using (var db = new TournamentContext())
				{
					Tournament newTournament = new Tournament() { Name = name };
					db.Tournaments.Add(newTournament);
					db.SaveChanges();
				}	
			}
		}

		public void RemoveTournament(int tournamentId)
		{
			List<Team> teamsToDelete = RetrieveTeams(tournamentId);
			foreach (Team team in teamsToDelete)
			{
				RemoveTeam(team.TeamId);
			}
			
			using (var db = new TournamentContext())
			{
				Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == tournamentId).FirstOrDefault();
				if (selectedTournament != null)
				{
					db.Tournaments.Remove(selectedTournament);
					db.SaveChanges();
				}
			}
		}

		public List<Tournament> RetrieveTournaments()
		{
			using (var db = new TournamentContext())
			{
				List<Tournament> allTournaments = db.Tournaments.ToList();
				return allTournaments;
			}
		}

		public void AddTeam(int tournamentId, string name)
		{
			if (name != "")
			{
				using (var db = new TournamentContext())
				{
					Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == tournamentId).FirstOrDefault();
					if (selectedTournament != null)
					{
						Team newTeam = new Team() { Name = name, Tournament = selectedTournament };
						db.Teams.Add(newTeam);
						db.SaveChanges();
					}
					
				}
			}
		}

		public void RemoveTeam(int teamId)
		{
			using (var db = new TournamentContext())
			{
				Team selectedTeam = db.Teams.Where(t => t.TeamId == teamId).FirstOrDefault();
				if (selectedTeam != null)
				{
					db.Teams.Remove(selectedTeam);
					db.SaveChanges();
				}
			}
		}

		public List<Team> RetrieveTeams(int tournamentId)
		{
			using (var db = new TournamentContext())
			{
				List<Team> allTeams = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == tournamentId).ToList();
				return allTeams;
			}
		}

		public void UpdateTournamentName(int tournamentId, string newName)
		{
			using (var db = new TournamentContext())
			{
				Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == tournamentId).FirstOrDefault();
				if (selectedTournament != null)
				{
					selectedTournament.Name = newName;
					db.SaveChanges();
				}
			}
		}

		public void UpdateTeamName(int teamId, string newName)
		{
			using (var db = new TournamentContext())
			{
				Team selectedTeam = db.Teams.Where(t => t.TeamId == teamId).FirstOrDefault();
				if (selectedTeam != null)
				{
					selectedTeam.Name = newName;
					db.SaveChanges();
				}
			}
		}
	}
}
