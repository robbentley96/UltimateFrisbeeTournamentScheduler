using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
					Tournament newTournament = new Tournament() { Name = name, StartingTime = new DateTime(2020, 11, 09, 09, 00, 00), MatchLength = 22, BreakLength = 3 };
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
				List<Match> matchesToRemove1 = RetrieveMatches(selectedTournament.TournamentId, 1);
				List<Match> matchesToRemove2 = RetrieveMatches(selectedTournament.TournamentId, 2);

				foreach (Match match in matchesToRemove1)
				{
					RemoveMatch(match.MatchId);
				}
				foreach (Match match in matchesToRemove2)
				{
					RemoveMatch(match.MatchId);
				}
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
			int tournamentId;
			int teamSeed;
			Team selectedTeam;
			using (var db = new TournamentContext())
			{
				selectedTeam = db.Teams.Include(t => t.Tournament).Where(t => t.TeamId == teamId).FirstOrDefault();
			}
			if (selectedTeam != null)
			{
				List<Match> matchesToRemove1 = RetrieveMatches(selectedTeam.Tournament.TournamentId, 1);
				List<Match> matchesToRemove2 = RetrieveMatches(selectedTeam.Tournament.TournamentId, 2);
				foreach (Match match in matchesToRemove1)
				{
					RemoveMatch(match.MatchId);
				}
				foreach (Match match in matchesToRemove2)
				{
					RemoveMatch(match.MatchId);
				}

				using (var db = new TournamentContext())
				{
					tournamentId = selectedTeam.Tournament.TournamentId;
					teamSeed = selectedTeam.Seed;
					db.Teams.Remove(selectedTeam);
					db.SaveChanges();
				}

				List<Team> remainingTeams = RetrieveTeams(tournamentId);
				foreach (Team team in remainingTeams)
				{
					if (team.Seed > teamSeed)
					{
						using (var db = new TournamentContext())
						{
							Team teamToChange = db.Teams.Where(t => t.TeamId == team.TeamId).FirstOrDefault();
							teamToChange.Seed--;
							db.SaveChanges();
						}
					}
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

		public void AddMatch(int tournamentId, int teamId1, int teamId2, int pitchNum, DateTime time)
		{
			using (var db = new TournamentContext())
			{
				Team team1 = db.Teams.Where(t => t.TeamId == teamId1).FirstOrDefault();
				Team team2 = db.Teams.Where(t => t.TeamId == teamId2).FirstOrDefault();
				Tournament selectedTournament = db.Tournaments.Where(t => t.TournamentId == tournamentId).FirstOrDefault();
				Match matchToAdd = new Match() { Tournament = selectedTournament, Team1 = team1, Team2 = team2, PitchNumber = pitchNum, Time = time };
				db.Matches.Add(matchToAdd);
				db.SaveChanges();
			}
		}

		public void RemoveMatch(int matchId)
		{
			using (var db = new TournamentContext())
			{
				Match selectedMatch = db.Matches.Where(m => m.MatchId == matchId).FirstOrDefault();
				db.Remove(selectedMatch);
				db.SaveChanges();
			}
		}

		public List<Match> RetrieveMatches(int tournamentId, int pitchNum)
		{
			using (var db = new TournamentContext())
			{
				List<Match> allMatches = db.Matches.Include(m => m.Tournament).Include(m => m.Team1).Include(m => m.Team2).Where(m => m.Tournament.TournamentId == tournamentId && m.PitchNumber == pitchNum).OrderBy(m => m.Time).ToList();
				return allMatches;
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
				List<Team> pool1 = db.Teams.Include(t => t.Pool).Where(t => t.Pool.PoolId == pool1Id).OrderBy(t => t.Seed).ToList();
				int pool2Id = allPools[1].PoolId;
				List<Team> pool2 = db.Teams.Include(t => t.Pool).Where(t => t.Pool.PoolId == pool2Id).OrderBy(t => t.Seed).ToList();
				int pool3Id = allPools[2].PoolId;
				List<Team> pool3 = db.Teams.Include(t => t.Pool).Where(t => t.Pool.PoolId == pool3Id).OrderBy(t => t.Seed).ToList();
				int pool4Id = allPools[3].PoolId;
				List<Team> pool4 = db.Teams.Include(t => t.Pool).Where(t => t.Pool.PoolId == pool4Id).OrderBy(t => t.Seed).ToList();
				return new List<Team>[4] { pool1, pool2, pool3, pool4 };
			}
		}

		//SCHEDULING

		public void SchedulePoolMatches(int tournamentId)
		{
			PopulatePools(tournamentId);
			if (RetrieveTeams(tournamentId).Count == 16)
			{
				using (var db = new TournamentContext())
				{
					var matchesToClear = db.Matches.Include(m => m.Tournament).Where(m => m.Tournament.TournamentId == tournamentId);
					db.RemoveRange(matchesToClear);
					db.SaveChanges();
				}
				List<Team>[] poolTeams = RetrievePoolTeams(tournamentId);
				TimeSpan gameTime = new TimeSpan(0, 22, 0);
				TimeSpan breakTime = new TimeSpan(0, 3, 0);
				TimeSpan totalTime = gameTime + breakTime;
				DateTime startTime = new DateTime(2020, 05, 01, 09, 00, 00);
				for (int i = 0; i < poolTeams.Count(); i++)
				{
					DateTime matchTime = startTime + i * totalTime;
					AddMatch(tournamentId, poolTeams[i][0].TeamId, poolTeams[i][1].TeamId, 1, matchTime);
					AddMatch(tournamentId, poolTeams[i][2].TeamId, poolTeams[i][3].TeamId, 2, matchTime);
					matchTime += 4 * totalTime;
					AddMatch(tournamentId, poolTeams[i][0].TeamId, poolTeams[i][2].TeamId, 1, matchTime);
					AddMatch(tournamentId, poolTeams[i][1].TeamId, poolTeams[i][3].TeamId, 2, matchTime);
					matchTime += 4 * totalTime;
					AddMatch(tournamentId, poolTeams[i][0].TeamId, poolTeams[i][3].TeamId, 1, matchTime);
					AddMatch(tournamentId, poolTeams[i][1].TeamId, poolTeams[i][2].TeamId, 2, matchTime);
				}
			}

		}
		public List<string> ListSeedMatches(int pitchNum)
		{
			List<string> seededMatches;
			if (pitchNum == 1)
			{
				seededMatches = new List<string>()
			{
				"6 v 11 - 09:00",
				"8 v 9 - 09:25",
				"11 v 14 - 09:50",
				"12 v 13 - 10:15",
				"4 v 5 - 10:40",
				"3 v 6 - 11:05",
				"14 v 15 - 11:30",
				"10 v 11 - 11:55",
				"6 v 7 - 12:20",
				"2 v 3 - 12:45",
				"12 v 14 - 13:10",
				"9 v 10 - 13:35",
				"5 v 6 - 14:00",
				"3 v 4 - 14:25"
			};
			}
			else
			{
				seededMatches = new List<string>()
			{
				"7 v 10 - 09:00",
				"5 v 12 - 09:25",
				"10 v 15 - 09:50",
				"9 v 16 - 10:15",
				"1 v 8 - 10:40",
				"2 v 7 - 11:05",
				"13 v 16 - 11:30",
				"9 v 12 - 11:55",
				"5 v 8 - 12:20",
				"1 v 4 - 12:45",
				"15 v 16 - 13:10",
				"11 v 12 - 13:35",
				"7 v 8 - 14:00",
				"1 v 2 - 14:25"
			};
			}
			return seededMatches;
		}
	}
}
