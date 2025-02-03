
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Projekt_SternbergundMehr
{

    public partial class Window_Umzug : Window
    {

        private participants participantsManager;
        private ParticipantsData selectedParticipant;

        public ObservableCollection<ParticipantsData> Participants { get; set; }

        public Window_Umzug()
        {
            InitializeComponent();
            participantsManager = new participants();
            LoadParticipantsFromDatabase();
            anzahl_teilnehmer();
        }

        public DataGrid GetDataGrid()
        {
            return dataGrid_umzug;
        }

       public void LoadParticipantsFromDatabase()
        {
            try
            {
                Participants = participantsManager.Loadparticipants();
                dataGrid_umzug.ItemsSource = Participants;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
            }
        }

        private bool isSearchActive = false;

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

            foreach (var item in dataGrid_umzug.Items)
            {
                if (item != null)
                {
                    // Hole die Zeile aus dem DataGrid-Item
                    var row = dataGrid_umzug.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        bool found = false;

                        // Durchlaufe alle Spalten (Zellen) der Zeile
                        foreach (var column in dataGrid_umzug.Columns)
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
                            dataGrid_umzug.ScrollIntoView(item); // Scrolle zur gefundenen Zeile
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
            foreach (var item in dataGrid_umzug.Items)
            {
                if (item != null)
                {
                    var row = dataGrid_umzug.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
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

        public void anzahl_teilnehmer()
        {
            try
            {
                int totalAmount = participantsManager.SearchParticipant();
                string formattedAmount = totalAmount.ToString(CultureInfo.GetCultureInfo("de-DE"));
                // MessageBox.Show($"Die Gesamtsumme der Teilnehmer beträgt: {formattedAmount}");
                tbx_sum.Text = formattedAmount;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Berechnen der Gesamtsumme: {ex.Message}");
            }
        }



        private void dataGrid_umzug_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_umzug.SelectedItem is ParticipantsData participant)
            {
                selectedParticipant = participant;
                tbx_Firma.Text = participant.Firma;
                tbx_anspr.Text = participant.Ansprechpartner;
                tbx_adress.Text = participant.Adresse;
                tbx_pos.Text = participant.Position.ToString();
            }
        }

        private void btn_print_Click(object sender, RoutedEventArgs e)
        {

            PrinterHelper_Umzug printerHelper_Umzug = new PrinterHelper_Umzug(dataGrid_umzug);
            List<ParticipantsData> participantslist = printerHelper_Umzug.ExtractDataFromDataGrid();
            FixedDocument document = printerHelper_Umzug.CreatePrintableDocument(participantslist);

            PrintPreviewWindow previewWindow = new PrintPreviewWindow(document);
            previewWindow.ShowDialog();
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

        private void ClearInputFields()
        {
            tbx_Firma.Clear();
            tbx_anspr.Clear();
            tbx_adress.Clear();
            tbx_pos.Clear();
            selectedParticipant = null;
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
                    Position = int.Parse(tbx_pos.Text)
                };

                participantsManager.AddParticipant(newParticipant);
                LoadParticipantsFromDatabase();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Einfügen der Daten: {ex.Message}");
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
                        Position = int.Parse(tbx_pos.Text)
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

        private void btn_planung_Click(object sender, RoutedEventArgs e)
        {
            // Logik für die Planung hinzufügen
        }

        private void btn_kosten_Click(object sender, RoutedEventArgs e)
        {
            // Logik für die Kosten hinzufügen
        }

        private void btn_print_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btn_home_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_mailmerge_Click(object sender, RoutedEventArgs e)
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

                document.SaveAs2(FileName: @"C:\\Users\\Klaas\\Desktop\\Testbrief.docx");
                document.Close();

                application.Quit();
            }
        }

        private void sponsor_list_prnt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGrid_sponsoren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void participants_list_prnt_Click(object sender, RoutedEventArgs e)
        {
            PrinterHelper_Umzug printerHelper = new PrinterHelper_Umzug(dataGrid_umzug);
            List<ParticipantsData> participantslist = printerHelper.ExtractDataFromDataGrid();
            FixedDocument document = printerHelper.CreatePrintableDocument(participantslist);

            PrintPreviewWindow previewWindow = new PrintPreviewWindow(document);
            previewWindow.ShowDialog();
        }

        private void dataGrid_umzug_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_umzug.SelectedItem is ParticipantsData participant)
            {
                selectedParticipant = participant;
                tbx_Firma.Text = participant.Firma;
                tbx_anspr.Text = participant.Ansprechpartner;
                tbx_adress.Text = participant.Adresse;
                tbx_pos.Text = participant.Position.ToString();
            }
        }

        private void mail_prnt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            PrinterHelper_Umzug printerHelper = new PrinterHelper_Umzug(dataGrid_umzug);
            List<ParticipantsData> participantslist = printerHelper.ExtractDataFromDataGrid();
            FixedDocument document = printerHelper.CreatePrintableDocument(participantslist);

            PrintPreviewWindow previewWindow = new PrintPreviewWindow(document);
            previewWindow.ShowDialog();
        }

        private void btn_delete_Click_1(object sender, RoutedEventArgs e)
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

            RefreshDataGrid();
        }
        public void RefreshDataGrid()
        {
            try
            {
                Participants = participantsManager.Loadparticipants();
                dataGrid_umzug.ItemsSource = Participants; // DataGrid aktualisieren
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Daten: {ex.Message}");
            }
        }

        private void btn_dialog_Click(object sender, RoutedEventArgs e)
        {
            var dialogWindow = new umzug_dialog(this); // Übergibt das Hauptfenster
            dialogWindow.ShowDialog();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            search_dataGrid();
        }
    }
}





