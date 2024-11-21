
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Projekt_SternbergundMehr
{

    public partial class Window_mail : Window
    {

        private DataTable dt;
        public Window_mail()
        {
            InitializeComponent();

        }


        private async void recieve_mail()
        {
            ProgressWindow progressWindow = null;

            try
            {

                Outlook.Application _app = new Outlook.Application();
                Outlook.NameSpace _ns = _app.GetNamespace("MAPI");
                Outlook.MAPIFolder inbox = _ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderInbox);


                Outlook.Items unreadItems = inbox.Items.Restrict("[Unread] = true");

                int totalItems = unreadItems.Count;
                if (totalItems == 0)
                {
                    MessageBox.Show("Keine ungelesenen E-Mails gefunden.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Öffne das Progress-Fenster
                progressWindow = new ProgressWindow();
                progressWindow.SetMaximum(totalItems);
                progressWindow.Show();



                dt = new DataTable("Inbox");
                dt.Columns.Add("Betreff", typeof(string));
                dt.Columns.Add("Absender", typeof(string));
                dt.Columns.Add("Inhalt", typeof(string));
                dt.Columns.Add("Datum", typeof(DateTime));


                int processedItems = 0;

                foreach (object item in unreadItems)
                {
                    if (item is Outlook.MailItem mailItem)
                    {
                        // Füge die Mail-Daten in die DataTable ein
                        dt.Rows.Add(new object[]
                        {
                            mailItem.Subject,
                            mailItem.SenderName,
                            mailItem.Body,
                            mailItem.SentOn
                        });

                        // Aktualisiere den Fortschritt in der ProgressBar

                        processedItems++;
                        progressWindow.SetProgress(processedItems);


                        await Task.Delay(50);
                    }
                }

                // Sortiere die Mails nach Datum und aktualisiere das DataGrid
                dt.DefaultView.Sort = "Datum DESC";
                dataGrid_mail.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {

                if (progressWindow != null)
                {
                    progressWindow.Close();
                }
            }
        }











        private async void DisplayStyledContentInWebView2(string htmlContent)
        {
            if (WebView2 != null)
            {
                await WebView2.EnsureCoreWebView2Async(null);
                string styledHtml = $@"
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: transparent;
            margin: 0;
            padding: 20px;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }}
        .email-content {{
            background-color: ##FF2B2B2B;
            border: 1px solid #e0e0e0;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            max-width: auto;
            width: 100%;
        }}
        h1 {{
            color: #333333;
            margin-bottom: 20px;
            font-size: 24px;
        }}
        p {{
            color: #555555;
            line-height: 1.6;
            font-size: 16px;
        }}
        a {{
            color: #007bff;
            text-decoration: none;
        }}
        
    </style>
</head>
<body>
    <div class='email-content'>
        {htmlContent}
    </div>
</body>
</html>";

                WebView2.NavigateToString(styledHtml);
            }
        }



        private void btn_send_Click(object sender, RoutedEventArgs e)
        {



        }




        private void btn_recieve_Click(object sender, RoutedEventArgs e)
        {
            recieve_mail();
        }

        private void dataGrid_mail_Selected(object sender, RoutedEventArgs e)
        {


        }

        private void dataGrid_mail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid_mail.SelectedItem is DataRowView rowView)
            {
                string bodyContent = rowView["Inhalt"].ToString();

                DisplayStyledContentInWebView2(bodyContent);
            }
        }

        private void dataGrid_mail_Selected_1(object sender, RoutedEventArgs e)
        {
            if (dataGrid_mail.SelectedItem is DataRowView rowView)
            {
                string bodyContent = rowView["Inhalt"].ToString();
            }
        }





        private void btn_home_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void brief_prnt_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void btn_recieve_Click_1(object sender, RoutedEventArgs e)
        {
            recieve_mail();
        }

        private void update_mail_Click(object sender, RoutedEventArgs e)
        {

        }

        private void write_mail_Click(object sender, RoutedEventArgs e)
        {
            Window_mail02_ window_Mail02 = new Window_mail02_();
            window_Mail02.Show();

        }

        private void write_mail_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void sponsor_nav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Menu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }

        private void umzug_nav_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            search_dataGrid();
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

            foreach (var item in dataGrid_mail.Items)
            {
                if (item != null)
                {
                    // Hole die Zeile aus dem DataGrid-Item
                    var row = dataGrid_mail.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (row != null)
                    {
                        bool found = false;

                        // Durchlaufe alle Spalten (Zellen) der Zeile
                        foreach (var column in dataGrid_mail.Columns)
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
                            dataGrid_mail.ScrollIntoView(item); // Scrolle zur gefundenen Zeile
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
            foreach (var item in dataGrid_mail.Items)
            {
                if (item != null)
                {
                    var row = dataGrid_mail.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
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

        private void mail_nav_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}


