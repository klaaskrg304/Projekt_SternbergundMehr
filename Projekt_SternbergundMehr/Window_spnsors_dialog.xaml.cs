
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Projekt_SternbergundMehr
{
    /// <summary>
    /// Interaktionslogik für Window_spnsors_dialog.xaml
    /// </summary>
    public partial class Window_spnsors_dialog : Window
    {

        private Sponsors sponsorsManager;
        private SponsorData selectedSponsor;

        public ObservableCollection<SponsorData> Sponsors { get; set; }

        private Window_sponsors mainSponsorsWindow;

        public Window_spnsors_dialog(Window_sponsors parentWindow)
        {
           
            InitializeComponent();
            sponsorsManager = new Sponsors();
            mainSponsorsWindow = parentWindow; // Referenz auf das Hauptfenster
            LoadSponsorsFromDatabase();
        }

        private void ClearInputFields()
        {
            tbx_Firma.Clear();
            tbx_anspr.Clear();
            tbx_adress.Clear();
            tbx_betrag.Clear();
            selectedSponsor = null;
        }


        private void LoadSponsorsFromDatabase()
        {


            Window_sponsors window_Sponsors = new Window_sponsors();
            window_Sponsors.Hide();

            DataGrid dataGrid_sponsoren = window_Sponsors.GetDataGrid();
            try
            {
                Sponsors = sponsorsManager.LoadSponsors();
                dataGrid_sponsoren.ItemsSource = Sponsors;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
            }
            window_Sponsors.Close();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SponsorData newSponsor = new SponsorData
                {
                    Firma = tbx_Firma.Text,
                    Ansprechpartner = tbx_anspr.Text,
                    Adresse = tbx_adress.Text,
                    Betrag = double.Parse(tbx_betrag.Text)
                };

                sponsorsManager.AddSponsor(newSponsor);
                LoadSponsorsFromDatabase();
                ClearInputFields();
                Window_sponsors window_Sponsors = new Window_sponsors();
                window_Sponsors.Hide();
                window_Sponsors.betrag_add();
                LoadSponsorsFromDatabase();
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Einfügen der Daten: {ex.Message}");
            }

            
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSponsor != null)
            {
                try
                {
                    sponsorsManager.DeleteSponsor(selectedSponsor.Firma);
                    LoadSponsorsFromDatabase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Löschen der Daten: {ex.Message}");
                }
            }
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSponsor != null)
            {
                try
                {
                    SponsorData updatedSponsor = new SponsorData
                    {
                        Firma = tbx_Firma.Text,
                        Ansprechpartner = tbx_anspr.Text,
                        Adresse = tbx_adress.Text,
                        Betrag = double.Parse(tbx_betrag.Text)
                    };

                    sponsorsManager.UpdateSponsor(updatedSponsor, selectedSponsor.Firma);
                    LoadSponsorsFromDatabase();
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

        private void Window_Closed(object sender, EventArgs e)
        {
            mainSponsorsWindow.RefreshDataGrid();
            mainSponsorsWindow.betrag_add();
        }
    }
}
