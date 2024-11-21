using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Projekt_SternbergundMehr
{
    /// <summary>
    /// Interaktionslogik für PrintPreviewWindow.xaml
    /// </summary>
    public partial class PrintPreviewWindow : Window
    {
        private FixedDocument _document;

        public PrintPreviewWindow(FixedDocument document)
        {
            InitializeComponent();
            _document = document;
            documentViewer.Document = _document;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintDocument(_document.DocumentPaginator, "Sponsorenliste Drucken");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

