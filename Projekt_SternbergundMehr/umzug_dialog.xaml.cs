using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Projekt_SternbergundMehr
{
    /// <summary>
    /// Interaktionslogik für umzug_dialog.xaml
    /// </summary>
    public partial class umzug_dialog : Window
    {
        private participants participantsManager;
        private ParticipantsData selectedParticipant;

        public ObservableCollection<ParticipantsData> Participants { get; set; }


        public umzug_dialog()
        {
            InitializeComponent();
            participantsManager = new participants();
            LoadParticipantsFromDatabase();
            Window_Umzug window_Umzug = new Window_Umzug();
            window_Umzug.Hide();
        }

        private void ClearInputFields()
        {
            tbx_Firma.Clear();
            tbx_anspr.Clear();
            tbx_adress.Clear();
            tbx_betrag.Clear();
            selectedParticipant = null;
        }


        private void LoadParticipantsFromDatabase()
        {


            Window_Umzug window_Umzug = new Window_Umzug();
            window_Umzug.Hide();

            DataGrid dataGrid_umzug = window_Umzug.GetDataGrid();
            try
            {
                Participants = participantsManager.Loadparticipants();
                dataGrid_umzug.ItemsSource = Participants;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
            }
            window_Umzug.Close();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParticipantsData newParticipant = new ParticipantsData
                {
                    Firma = tbx_Firma.Text,
                    Ansprechpartner = tbx_anspr.Text,
                    Adresse = tbx_adress.Text,
                    Position = int.Parse(tbx_betrag.Text)
                };

                participantsManager.AddParticipant(newParticipant);
                LoadParticipantsFromDatabase();
                ClearInputFields();
                Window_Umzug window_Umzug = new Window_Umzug();
                window_Umzug.Hide();
                window_Umzug.anzahl_teilnehmer();
                window_Umzug.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Einfügen der Daten: {ex.Message}");
            }


        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedParticipant != null)
            {
                try
                {
                    participantsManager.DeleteParticipant(selectedParticipant.Firma);
                    LoadParticipantsFromDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Löschen der Daten: {ex.Message}");
                }
            }
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedParticipant != null)
            {
                try
                {
                    ParticipantsData updatedParticipant = new ParticipantsData
                    {
                        Firma = tbx_Firma.Text,
                        Ansprechpartner = tbx_anspr.Text,
                        Adresse = tbx_adress.Text,
                        Position = int.Parse(tbx_betrag.Text)
                    };

                    participantsManager.UpdateParticipant(updatedParticipant, selectedParticipant.Firma);

                    LoadParticipantsFromDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Aktualisieren der Daten: {ex.Message}");
                }
            }
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

