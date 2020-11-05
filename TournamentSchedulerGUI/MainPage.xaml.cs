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
	/// Interaction logic for MainPage.xaml
	/// </summary>
	public partial class MainPage : Page
	{
		TournamentMethods _tournamentMethods = new TournamentMethods();
		public MainPage()
		{
			InitializeComponent();
			TournamentList.ItemsSource = _tournamentMethods.RetrieveTournaments();
		}

		private void AddTournamentButton_Click(object sender, RoutedEventArgs e)
		{
			_tournamentMethods.AddTournament(TournamentInput.Text);
			TournamentInput.Text = "";
			TournamentList.ItemsSource = _tournamentMethods.RetrieveTournaments();
		}

		private void RemoveTournamentButton_Click(object sender, RoutedEventArgs e)
		{
			
			Tournament selectedTournament = (Tournament)TournamentList.SelectedItem;
			if (selectedTournament != null)
			{
				_tournamentMethods.RemoveTournament(selectedTournament.TournamentId);
				TournamentList.ItemsSource = _tournamentMethods.RetrieveTournaments();
			}
			
			
			
		}

		private void ViewTournamentButton_Click(object sender, RoutedEventArgs e)
		{
			Tournament selectedTournament = (Tournament)TournamentList.SelectedItem;
			if (selectedTournament != null)
			{
				this.NavigationService.Navigate(new TournamentPage(selectedTournament.TournamentId));
			}
			

		}

		private void TournamentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (TournamentList.SelectedItem != null)
			{
				Tournament selectedTournament = (Tournament)TournamentList.SelectedItem;
				TournamentInput.Text = selectedTournament.ToString();
			}
			
		}

		private void UpdateTournamentButton_Click(object sender, RoutedEventArgs e)
		{
			if (TournamentList.SelectedItem != null)
			{
				Tournament selectedTournament = (Tournament)TournamentList.SelectedItem;
				_tournamentMethods.UpdateTournamentName(selectedTournament.TournamentId, TournamentInput.Text);
				TournamentInput.Text = "";
				TournamentList.ItemsSource = _tournamentMethods.RetrieveTournaments();
			}
			
		}
	}
}
