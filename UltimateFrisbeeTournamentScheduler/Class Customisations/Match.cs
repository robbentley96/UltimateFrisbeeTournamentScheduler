using System;
using System.Collections.Generic;
using System.Text;

namespace UltimateFrisbeeTournamentScheduler
{
	public partial class Match
	{
		public override string ToString()
		{
			return $"{Team1.Name} vs {Team2.Name} - {Time.ToString("HH:mm")}";
		}
	}
}
