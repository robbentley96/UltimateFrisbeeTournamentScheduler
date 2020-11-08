using System;
using System.Collections.Generic;
using System.Text;

namespace UltimateFrisbeeTournamentScheduler
{
	public partial class Team
	{
		public override string ToString()
		{
			return $"{Seed}) {Name}";
		}
	}
}
