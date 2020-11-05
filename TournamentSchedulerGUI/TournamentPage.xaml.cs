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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UltimateFrisbeeTournamentScheduler;

namespace TournamentSchedulerGUI
{
	/// <summary>
	/// Interaction logic for TournamentPage.xaml
	/// </summary>
	public partial class TournamentPage : Page
	{
		TournamentMethods _tournamentMethods = new TournamentMethods();
		Tournament selectedTournament;
		
		public TournamentPage(int id)
		{
			InitializeComponent();
			selectedTournament = _tournamentMethods.RetrieveTournament(id);
			TournamentName.Text = selectedTournament.ToString();
			TeamList.ItemsSource = _tournamentMethods.RetrieveTeams(selectedTournament.TournamentId);
		}
		private void AddTeamButton_Click(object sender, RoutedEventArgs e)
		{
			_tournamentMethods.AddTeam(selectedTournament.TournamentId, TeamInput.Text);
			TeamInput.Text = "";
			TeamList.ItemsSource = _tournamentMethods.RetrieveTeams(selectedTournament.TournamentId);

		}
		
		private void RemoveTeamButton_Click(object sender, RoutedEventArgs e)
		{
			Team selectedTeam = (Team)TeamList.SelectedItem;
			if (selectedTeam != null)
			{
				_tournamentMethods.RemoveTeam(selectedTeam.TeamId);
				TeamList.ItemsSource = _tournamentMethods.RetrieveTeams(selectedTournament.TournamentId);
				TeamInput.Text = "";
			}
		}

		private void UpdateTeamButton_Click(object sender, RoutedEventArgs e)
		{
			if (TeamList.SelectedItem != null)
			{
				Team selectedTeam = (Team)TeamList.SelectedItem;
				_tournamentMethods.UpdateTeamName(selectedTeam.TeamId, TeamInput.Text);
				TeamInput.Text = "";
				TeamList.ItemsSource = _tournamentMethods.RetrieveTeams(selectedTournament.TournamentId);
			}
			
		}

		private void TeamList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (TeamList.SelectedItem != null)
			{
				Team selectedTeam = (Team)TeamList.SelectedItem;
				TeamInput.Text = selectedTeam.ToString();
			}
			

		}

		private void SchedulePoolGamesButton_Click(object sender, RoutedEventArgs e)
		{

		}

		
	}
}
