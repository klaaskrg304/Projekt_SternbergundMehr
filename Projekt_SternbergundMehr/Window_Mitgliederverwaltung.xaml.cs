using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekt_SternbergundMehr
{

    public partial class Window_Mitgliederverwaltung : Window
    {

        private Mitglieder mitgliedermanager;
        private MitgliedData selectedMitglied;

        public ObservableCollection<MitgliedData> Mitglied { get; private set; }

        public Window_Mitgliederverwaltung()
        {
            InitializeComponent();

            mitgliedermanager = new Mitglieder();
            LoadMitgliederFromDatabase();

        }

        private void btn_home_Click(object sender, RoutedEventArgs e)
        {


            this.Close();



        }

        private void brief_prnt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadMitgliederFromDatabase()
        {
            try
            {
                Mitglied = mitgliedermanager.LoadMitglieder();
                dataGrid_mitglieder.ItemsSource = Mitglied;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
            }
        }


        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void sponsor_list_prnt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }


        }

        private void dataGrid_mitglieder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_mitglieder.SelectedItem is MitgliedData mitglied)
            {
                selectedMitglied = mitglied;
                tbx_name.Text = mitglied.Name;
                tbx_telefon.Text = mitglied.Telefonnummer ;
                tbx_adresse.Text = mitglied.Adresse;
                tbx_beitrag.Text = mitglied.Beitrag.ToString();
            }
        }
    }
}
