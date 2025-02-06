
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WpfWindow = System.Windows.Window;

namespace Projekt_SternbergundMehr
{

    public partial class Window_sponsors : WpfWindow
    {


        private Sponsors sponsorsManager;
        private SponsorData selectedSponsor;

        public ObservableCollection<SponsorData> Sponsors { get; set; }

        public Window_sponsors()
        {
            InitializeComponent();



            sponsorsManager = new Sponsors();
            RefreshDataGrid();
            betrag_add();
        }

        public DataGrid GetDataGrid()
        {
            return dataGrid_sponsoren;
        }


       
        public void LoadSponsorsFromDatabase()
        {
            try
            {
                Sponsors = sponsorsManager.LoadSponsors();
                dataGrid_sponsoren.ItemsSource = Sponsors;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
            }
        }


        public void RefreshDataGrid()
        {
            try
            {
                Sponsors = sponsorsManager.LoadSponsors();
                dataGrid_sponsoren.ItemsSource = Sponsors; // DataGrid aktualisieren
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
            }
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

            betrag_add();
            
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


        private bool isSearchActive = false; // Flag, um zu verfolgen, ob eine Suche aktiv ist

        private string search_dataGrid()
        {
            string searchText = tbx_search.Text?.Trim().ToLower(); // Trim entfernt Leerzeichen, ToLower für Case-Insensitive

            // Sonderfall abfangen: Wenn die TextBox leer ist oder nur aus Leerzeichen besteht, Suche nicht ausführen
            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Bitte geben Sie einen Suchbegriff ein.");
                return null;
            }

            // Wenn bereits eine Suche aktiv ist, setze den Zustand zurück
            if (isSearchActive)
            {
                ResetDataGridHighlighting();
                isSearchActive = false;
                return null; // Keine weitere Suche ausführen
            }

            // Suche im DataGrid durchführen
            bool anyMatchFound = false;

            foreach (var item in dataGrid_sponsoren.Items)
            {
                if (item != null)
                {
                    // Hole die Zeile aus dem DataGrid-Item
                    var row = dataGrid_sponsoren.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        bool found = false;

                        // Durchlaufe alle Spalten (Zellen) der Zeile
                        foreach (var column in dataGrid_sponsoren.Columns)
                        {
                            var cellContent = column.GetCellContent(item) as TextBlock;

                            if (cellContent != null && cellContent.Text.ToLower().Contains(searchText))
                            {
                                found = true;
                                break; // Sobald der Suchtext gefunden wird, Schleife abbrechen
                            }
                        }

                        // Wenn der Text gefunden wurde, Zeile hervorheben
                        if (found)
                        {
                            row.Background = Brushes.Yellow; // Zeile hervorheben
                            dataGrid_sponsoren.ScrollIntoView(item); // Scrolle zur gefundenen Zeile
                            anyMatchFound = true;
                        }
                        else
                        {
                            row.Background = Brushes.White; // Standardfarbe, falls nicht gefunden
                        }
                    }
                }
            }

            // Wenn Übereinstimmungen gefunden wurden, setze das Flag auf aktiv
            if (anyMatchFound)
            {
                isSearchActive = true;
            }
            else
            {
                MessageBox.Show("Kein Ergebnis gefunden.");
            }

            return searchText;
        }

        // Methode, um das Highlighting im DataGrid zurückzusetzen
        private void ResetDataGridHighlighting()
        {
            int index = 0;
            foreach (var item in dataGrid_sponsoren.Items)
            {
                if (item != null)
                {
                    var row = dataGrid_sponsoren.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        // Stelle den alternierenden Hintergrund wieder her
                        if (index % 2 == 0)
                        {
                            row.Background = Brushes.White; // Standardfarbe für gerade Zeilen
                        }
                        else
                        {
                            row.Background = (Brush)new BrushConverter().ConvertFromString("#FFE0DFDF");
                        }
                        index++;
                    }
                }
            }
        }

        private void dataGrid_sponsoren_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Prüfe, ob die Spalte den Namen "Betrag" hat (ersetze "Betrag" durch den tatsächlichen Namen der Spalte)
            if (e.PropertyName == "Betrag")
            {
                if (e.Column is DataGridTextColumn textColumn)
                {
                    // Style für die Spalte setzen
                    textColumn.ElementStyle = new Style(typeof(TextBlock));
                    textColumn.ElementStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Green));
                }
            }
        }




        private void btn_print_Click_1(object sender, RoutedEventArgs e)
        {
            PrintHelper printHelper = new PrintHelper(dataGrid_sponsoren); // dataGrid_sponsoren wird verwendet, um SponsorData zu extrahieren
            List<SponsorData> sponsorList = printHelper.ExtractDataFromDataGrid(); // Extrahiere SponsorData
            FixedDocument document = printHelper.CreatePrintableDocument(sponsorList); // Erstelle das Dokument mit SponsorData

            PrintPreviewWindow previewWindow = new PrintPreviewWindow(document); // Vorschaufenster öffnen
            previewWindow.ShowDialog();
        }


        private void dataGrid_sponsoren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_sponsoren.SelectedItem is SponsorData sponsor)
            {
                selectedSponsor = sponsor;
                tbx_Firma.Text = sponsor.Firma;
                tbx_anspr.Text = sponsor.Ansprechpartner;
                tbx_adress.Text = sponsor.Adresse;
                tbx_betrag.Text = sponsor.Betrag.ToString();
            }
        }

        private void btn_betragaddieren_Click(object sender, RoutedEventArgs e)
        {

        }

        public void betrag_add()
        {
            try
            {
                double totalAmount = sponsorsManager.GetTotalAmount();
                string formattedAmount = totalAmount.ToString("N2", CultureInfo.GetCultureInfo("de-DE")) + " €";
                // MessageBox.Show($"Die Gesamtsumme der Spenden beträgt: {formattedAmount}");
                tbx_sum.Text = formattedAmount;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Berechnen der Gesamtsumme: {ex.Message}");
            }
        }

        private void ClearInputFields()
        {
            tbx_Firma.Clear();
            tbx_anspr.Clear();
            tbx_adress.Clear();
            tbx_betrag.Clear();
            selectedSponsor = null;
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
        }

        private void btn_umzug_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_umzug_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btn_mailmerge_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_mailmerge_main_Click(object sender, RoutedEventArgs e)
        {


        }

        private void btn_mailmerge_main_DragOver(object sender, DragEventArgs e)
        {

        }

        private void btn_mailmerge_main_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void btn_mailmerge_main_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void btn_mailmerge_main_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void btn_brief_main_Click(object sender, RoutedEventArgs e)
        {
            var application = new Microsoft.Office.Interop.Word.Application();
            var document = new Microsoft.Office.Interop.Word.Document();

            document = application.Documents.Add(Template: @"C:\\Users\\Klaas\\Desktop\\Briefvorlage Sternberg und MEHR e.V..docx");

            application.Visible = true;

            foreach (Microsoft.Office.Interop.Word.Field field in document.Fields)
            {
                if (field.Code.Text.Contains("Firma"))
                {
                    field.Select();
                    application.Selection.TypeText(tbx_Firma.Text);
                }

                else if (field.Code.Text.Contains("Name"))
                {
                    field.Select();
                    application.Selection.TypeText(tbx_anspr.Text);
                }

                else if (field.Code.Text.Contains("Adresse"))
                {
                    field.Select();
                    application.Selection.TypeText(tbx_adress.Text);
                }

            }

            document.SaveAs2(FileName: @"C:\\Users\\Klaas\\Desktop\\Testbrief.docx");
            document.Close();

            application.Quit();
        }

        private void sub_mail_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void btn_dialaog_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_formular_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sponsor_list_prnt_Click(object sender, RoutedEventArgs e)
        {
            PrintHelper printHelper = new PrintHelper(dataGrid_sponsoren);
            List<SponsorData> sponsorList = printHelper.ExtractDataFromDataGrid();
            FixedDocument document = printHelper.CreatePrintableDocument(sponsorList);

            PrintPreviewWindow previewWindow = new PrintPreviewWindow(document);
            previewWindow.ShowDialog();
        }

        private void brief_prnt_Click(object sender, RoutedEventArgs e)
        {
            var application = new Microsoft.Office.Interop.Word.Application();
            var document = new Microsoft.Office.Interop.Word.Document();

            document = application.Documents.Add(Template: @"C:\\Users\\Klaas\\Desktop\\Briefvorlage Sternberg und MEHR e.V..docx");

            application.Visible = true;

            foreach (Microsoft.Office.Interop.Word.Field field in document.Fields)
            {
                if (field.Code.Text.Contains("Firma"))
                {
                    field.Select();
                    application.Selection.TypeText(tbx_Firma.Text);
                }

                else if (field.Code.Text.Contains("Name"))
                {
                    field.Select();
                    application.Selection.TypeText(tbx_anspr.Text);
                }

                else if (field.Code.Text.Contains("Adresse"))
                {
                    field.Select();
                    application.Selection.TypeText(tbx_adress.Text);
                }

            }

            document.SaveAs2(FileName: @"C:\\Users\\Klaas\\Desktop\\Testbrief.docx");
            document.Close();

            application.Quit();
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_home_Click(object sender, RoutedEventArgs e)
        {


            this.Close();



        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }


        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        

        private void btn_delete_Click_1(object sender, RoutedEventArgs e)
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
            betrag_add();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {

            search_dataGrid();
        }

        private void tbx_sum_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_dialog_Click(object sender, RoutedEventArgs e)
        {
            var dialogWindow = new Window_spnsors_dialog(this); // Übergibt das Hauptfenster
            dialogWindow.ShowDialog();
        }

        private void tbx_adress_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

public class ParticipantsData
{
    public int Position { get; set; }

    public string Firma { get; set; }
    public string Ansprechpartner { get; set; }
    public string Adresse { get; set; }
}





public class SponsorData
{
    public string Firma { get; set; }
    public string Ansprechpartner { get; set; }
    public string Adresse { get; set; }
    public double Betrag { get; set; }
}








