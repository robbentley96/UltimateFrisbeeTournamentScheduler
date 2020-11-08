using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace UltimateFrisbeeTournamentScheduler
{
	public class TournamentMethods
	{
		//CRUD
		
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
				int tournamentId;
				using (var db = new TournamentContext())
				{
					tournamentId = db.Tournaments.OrderByDescending(t => t.TournamentId).First().TournamentId;
				}
					
				AddPools(tournamentId);
			}
		}

		public void RemoveTournament(int tournamentId)
		{
			Tournament selectedTournament;
			using (var db = new TournamentContext())
			{
				selectedTournament = db.Tournaments.Where(t => t.TournamentId == tournamentId).FirstOrDefault();
			}
			if (selectedTournament != null)
			{
				List<Team> teamsToRemove = RetrieveTeams(selectedTournament.TournamentId);
				List<Pool> poolsToRemove = RetrievePools(selectedTournament.TournamentId);

				foreach (Team team in teamsToRemove)
				{
					RemoveTeam(team.TeamId);
				}
				foreach (Pool pool in poolsToRemove)
				{
					RemovePool(pool.PoolId);
				}
			
			
				using (var db = new TournamentContext())
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

		public Tournament RetrieveTournament(int tournamentId)
		{
			using (var db = new TournamentContext())
			{
				Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == tournamentId).FirstOrDefault();
				return selectedTournament;
			}
		}

		public void AddTeam(int tournamentId, string name)
		{
			if (name != "" && RetrieveTeams(tournamentId).Count() < 16)
			{
				using (var db = new TournamentContext())
				{
					Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == tournamentId).FirstOrDefault();
					if (selectedTournament != null)
					{
						Team newTeam = new Team() { Name = name, Tournament = selectedTournament, Seed = RetrieveTeams(tournamentId).Count() + 1 };
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
				List<Team> allTeams = db.Teams.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == tournamentId).OrderBy(t => t.Seed).ToList();
				return allTeams;
			}
		}

		public void UpdateTournamentName(int tournamentId, string newName)
		{
			using (var db = new TournamentContext())
			{
				Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == tournamentId).FirstOrDefault();
				if (selectedTournament != null && newName != "")
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
				if (selectedTeam != null && newName != "")
				{
					selectedTeam.Name = newName;
					db.SaveChanges();
				}
			}
		}

		public void AddPools(int tournamentId)
		{
			
			using (var db = new TournamentContext())
			{
				Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == tournamentId).FirstOrDefault();
				if (selectedTournament != null)
				{
					for (int i = 0; i < 4; i++)
					{
						db.Pools.Add(new Pool { Tournament = selectedTournament });
						db.SaveChanges();
					}
				}
			}
		}

		public void RemovePool(int poolId)
		{
			using (var db = new TournamentContext())
			{
				Pool selectedPool = db.Pools.Where(t => t.PoolId == poolId).FirstOrDefault();
				if (selectedPool != null)
				{
					db.Pools.Remove(selectedPool);
					db.SaveChanges();
				}
			}
		}

		public List<Pool> RetrievePools(int tournamentId)
		{
			using (var db = new TournamentContext())
			{
				List<Pool> allPools = db.Pools.Include(t => t.Tournament).Where(t => t.Tournament.TournamentId == tournamentId).ToList();
				return allPools;
			}
		}

		//SEEDING

		public void GainSeed(int teamId)
		{
			using (var db = new TournamentContext())
			{
				Team selectedTeam = db.Teams.Include(t => t.Tournament).Where(t => t.TeamId == teamId).FirstOrDefault();
				int tournamentId = selectedTeam.Tournament.TournamentId;
				if (selectedTeam != null && selectedTeam.Seed > 1)
				{
					Team teamToSwitch = db.Teams.Include(t => t.Tournament).Where(t => t.Seed == (selectedTeam.Seed - 1) && t.Tournament.TournamentId == tournamentId).FirstOrDefault();
					int tempSeed = teamToSwitch.Seed;
					teamToSwitch.Seed = selectedTeam.Seed;
					selectedTeam.Seed = tempSeed;
					db.SaveChanges();
				}
			}
		}

		public void DropSeed(int teamId)
		{
			using (var db = new TournamentContext())
			{
				Team selectedTeam = db.Teams.Include(t => t.Tournament).Where(t => t.TeamId == teamId).FirstOrDefault();
				int tournamentId = selectedTeam.Tournament.TournamentId;
				if (selectedTeam != null && selectedTeam.Seed < RetrieveTeams(tournamentId).Count())
				{
					Team teamToSwitch = db.Teams.Include(t => t.Tournament).Where(t => t.Seed == (selectedTeam.Seed + 1) && t.Tournament.TournamentId == tournamentId).FirstOrDefault();
					int tempSeed = teamToSwitch.Seed;
					teamToSwitch.Seed = selectedTeam.Seed;
					selectedTeam.Seed = tempSeed;
					db.SaveChanges();
				}
			}
		}

		//Pooling

		public void PopulatePools(int tournamentId)
		{
			if (RetrieveTeams(tournamentId).Count == 16)
			{
				List<Team> allTeams = RetrieveTeams(tournamentId);
				List<Pool> allPools = RetrievePools(tournamentId);
				using (var db = new TournamentContext())
				{
					for (int i = 0; i < 16; i++)
					{
						int allocatedPool = i % 4;
						Team selectedTeam = db.Teams.Where(t => t.TeamId == allTeams[i].TeamId).FirstOrDefault();
						Pool selectedPool = db.Pools.Where(p => p.PoolId == allPools[allocatedPool].PoolId).FirstOrDefault();
						selectedTeam.Pool = selectedPool;
						db.SaveChanges();
					}
				}
			}
			
		}

		public List<Team>[] RetrievePoolTeams(int tournamentId)
		{
			List<Pool> allPools = RetrievePools(tournamentId);
			using (var db = new TournamentContext())
			{
				int pool1Id = allPools[0].PoolId;
				List<Team> pool1 = db.Teams.Include(t => t.Pool).Where(t => t.Pool.PoolId == pool1Id).ToList();
				int pool2Id = allPools[1].PoolId;
				List<Team> pool2 = db.Teams.Include(t => t.Pool).Where(t => t.Pool.PoolId == pool2Id).ToList();
				int pool3Id = allPools[2].PoolId;
				List<Team> pool3 = db.Teams.Include(t => t.Pool).Where(t => t.Pool.PoolId == pool3Id).ToList();
				int pool4Id = allPools[3].PoolId;
				List<Team> pool4 = db.Teams.Include(t => t.Pool).Where(t => t.Pool.PoolId == pool4Id).ToList();
				return new List<Team>[4] { pool1, pool2, pool3, pool4 };
			}
		}





	}
}
