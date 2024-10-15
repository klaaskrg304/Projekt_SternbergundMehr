
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


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


        public Window_spnsors_dialog()
        {
            InitializeComponent();
            sponsorsManager = new Sponsors();
            LoadSponsorsFromDatabase();
            Window_sponsors window_Sponsors = new Window_sponsors();
            window_Sponsors.Hide();
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
                window_Sponsors.Close();

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
    }
}
