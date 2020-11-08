using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UltimateFrisbeeTournamentScheduler;

namespace TournamentSchedulerGUI
{
	/// <summary>
	/// Interaction logic for PoolsWindow.xaml
	/// </summary>
	public partial class PoolsWindow : Window
	{
		TournamentMethods _tournamentMethods = new TournamentMethods();
		Tournament selectedTournament;
		public PoolsWindow(int id)
		{
			InitializeComponent();
			selectedTournament = _tournamentMethods.RetrieveTournament(id);
			ListPoolA.ItemsSource = _tournamentMethods.RetrievePoolTeams(selectedTournament.TournamentId)[0];
			ListPoolB.ItemsSource = _tournamentMethods.RetrievePoolTeams(selectedTournament.TournamentId)[1];
			ListPoolC.ItemsSource = _tournamentMethods.RetrievePoolTeams(selectedTournament.TournamentId)[2];
			ListPoolD.ItemsSource = _tournamentMethods.RetrievePoolTeams(selectedTournament.TournamentId)[3];
		}
	}
}
