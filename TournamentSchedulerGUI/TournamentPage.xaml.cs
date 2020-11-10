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
			ViewDay1Button.Visibility = Visibility.Hidden;
			ViewDay2Button.Visibility = Visibility.Hidden;
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
				TeamInput.Text = selectedTeam.Name;
			}
			

		}

		private void SchedulePoolGamesButton_Click(object sender, RoutedEventArgs e)
		{
			if (_tournamentMethods.RetrieveTeams(selectedTournament.TournamentId).Count == 16)
			{
				_tournamentMethods.SchedulePoolMatches(selectedTournament.TournamentId);
				Pitch1List.ItemsSource = _tournamentMethods.RetrieveMatches(selectedTournament.TournamentId, 1);
				Pitch2List.ItemsSource = _tournamentMethods.RetrieveMatches(selectedTournament.TournamentId, 2);
				Pitch1List.Visibility = Visibility.Visible;
				Pitch2List.Visibility = Visibility.Visible;
				TitlePitch1.Visibility = Visibility.Visible;
				TitlePitch2.Visibility = Visibility.Visible;
				AddTeamButton.Visibility = Visibility.Hidden;
				UpdateTeamButton.Visibility = Visibility.Hidden;
				RemoveTeamButton.Visibility = Visibility.Hidden;
				GainSeedButton.Visibility = Visibility.Hidden;
				DropSeedButton.Visibility = Visibility.Hidden;
				ViewDay2Button.Visibility = Visibility.Visible;
				PoolsWindow pw = new PoolsWindow(selectedTournament.TournamentId);
				pw.Top = Application.Current.MainWindow.Top + 100;
				pw.Left = Application.Current.MainWindow.Left + 100;
				pw.Show();
			}
			else
			{
				MessageBox.Show("You must have 16 teams to schedule a tournament.");
			}
		}

		private void GainSeedButton_Click(object sender, RoutedEventArgs e)
		{
			if (TeamList.SelectedItem != null)
			{
				Team selectedTeam = (Team)TeamList.SelectedItem;
				int selectedIndex = TeamList.SelectedIndex;
				_tournamentMethods.GainSeed(selectedTeam.TeamId);
				TeamList.ItemsSource = _tournamentMethods.RetrieveTeams(selectedTournament.TournamentId);
				TeamList.UpdateLayout();
				if (selectedIndex != 0)
				{
					ListBoxItem lbi = (ListBoxItem)TeamList.ItemContainerGenerator.ContainerFromIndex(selectedIndex - 1);
					lbi.IsSelected = true;
				}
				else
				{
					ListBoxItem lbi = (ListBoxItem)TeamList.ItemContainerGenerator.ContainerFromIndex(selectedIndex);
					lbi.IsSelected = true;
				}
			}
		}

		private void DropSeedButton_Click(object sender, RoutedEventArgs e)
		{
			if (TeamList.SelectedItem != null)
			{
				Team selectedTeam = (Team)TeamList.SelectedItem;
				int selectedIndex = TeamList.SelectedIndex;
				_tournamentMethods.DropSeed(selectedTeam.TeamId);
				TeamList.ItemsSource = _tournamentMethods.RetrieveTeams(selectedTournament.TournamentId);
				TeamList.UpdateLayout();
				if (selectedIndex != _tournamentMethods.RetrieveTeams(selectedTournament.TournamentId).Count - 1)
				{
					ListBoxItem lbi = (ListBoxItem)TeamList.ItemContainerGenerator.ContainerFromIndex(selectedIndex + 1);
					lbi.IsSelected = true;
				}
				else
				{
					ListBoxItem lbi = (ListBoxItem)TeamList.ItemContainerGenerator.ContainerFromIndex(selectedIndex);
					lbi.IsSelected = true;
				}
			}
		}

		private void HomeButton_Click(object sender, RoutedEventArgs e)
		{
			this.NavigationService.Navigate(new MainPage());
		}

		private void ViewDay2Button_Click(object sender, RoutedEventArgs e)
		{
			ViewDay2Button.Visibility = Visibility.Hidden;
			ViewDay1Button.Visibility = Visibility.Visible;
			Pitch1List.ItemsSource = _tournamentMethods.ListSeedMatches(1);
			Pitch2List.ItemsSource = _tournamentMethods.ListSeedMatches(2);
		}

		private void ViewDay1Button_Click(object sender, RoutedEventArgs e)
		{
			ViewDay1Button.Visibility = Visibility.Hidden;
			ViewDay2Button.Visibility = Visibility.Visible;
			Pitch1List.ItemsSource = _tournamentMethods.RetrieveMatches(selectedTournament.TournamentId, 1);
			Pitch2List.ItemsSource = _tournamentMethods.RetrieveMatches(selectedTournament.TournamentId, 2);
		}
	}
}
