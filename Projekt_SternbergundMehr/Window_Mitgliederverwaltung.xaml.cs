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

        }
    }
}
