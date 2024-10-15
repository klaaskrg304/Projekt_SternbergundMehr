using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace Projekt_SternbergundMehr
{
    public class PrinterHelper_Umzug : PrintHelper
    {
        private DataGrid dataGrid_umzug;

        public PrinterHelper_Umzug(DataGrid dataGrid) : base(dataGrid)
        {
            dataGrid_umzug = dataGrid;
        }

        ~PrinterHelper_Umzug() { }

        public new List<ParticipantsData>  ExtractDataFromDataGrid()
        {
            List<ParticipantsData> participantlist = new List<ParticipantsData>();

            foreach (var item in dataGrid_umzug.Items)
            {
                if (item is ParticipantsData participantsData)
                {
                    participantlist.Add(participantsData);
                }
            }

            return participantlist;
        }

        public FixedDocument CreatePrintableDocument(List<ParticipantsData> participantlist)
        {

            DateTime now = DateTime.Now;
            string shortDateTime = now.ToString("yyyy-MM-dd");

            FixedDocument fixedDoc = new FixedDocument();
            FixedPage fixedPage = new FixedPage
            
            {
                Width = 827, // A4 Papierbreite in Punkte (210 mm)
                Height = 1169 // A4 Papierhöhe in Punkte (297 mm)
            };

           

            StackPanel printPanel = new StackPanel
            {
                Margin = new Thickness(50),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 727 // Breite des A4-Blattes minus Ränder (827 - 2*50)
            };

            TextBlock title = new TextBlock
            {
                Text = "Teilnehmerliste"+   "//" + shortDateTime,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20),
                TextAlignment = TextAlignment.Center
            };

            Border horizontalBar1 = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 2, 0, 0),
                Margin = new Thickness(0, 20, 0, 10),
                Width = 727 // Breite des A4-Blattes minus Ränder (827 - 2*50)
            };

            Border horizontalBar2 = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 2, 0, 0),
                Margin = new Thickness(0, 20, 0, 10),
                Width = 727 // Breite des A4-Blattes minus Ränder (827 - 2*50)
            };

            printPanel.Children.Add(title);
            printPanel.Children.Add(horizontalBar1);

            Grid grid = new Grid
            {
                Margin = new Thickness(0, 0, 0, 20),
                Width = 727 // Breite des A4-Blattes minus Ränder (827 - 2*50)
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            AddGridHeader(grid);
            AddGridRows(grid, participantlist);
            
            printPanel.Children.Add(grid);

           


            // Horizontaler Balken
            Border horizontalBar = new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 2, 0, 0),
                Margin = new Thickness(0, 20, 0, 10),
                Width = 727 // Breite des A4-Blattes minus Ränder (827 - 2*50)
            };
            printPanel.Children.Add(horizontalBar);

            
            

            fixedPage.Children.Add(printPanel);

            PageContent pageContent = new PageContent();
            ((IAddChild)pageContent).AddChild(fixedPage);
            fixedDoc.Pages.Add(pageContent);
          

            return fixedDoc;
        }

        private void AddGridHeader(Grid grid)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            TextBlock headerPosition = new TextBlock { Text = "Position", FontWeight = FontWeights.Bold, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
            TextBlock headerFirma = new TextBlock { Text = "Firma", FontWeight = FontWeights.Bold, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
            TextBlock headerAnsprechpartner = new TextBlock { Text = "Ansprechpartner", FontWeight = FontWeights.Bold, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
            TextBlock headerAdresse = new TextBlock { Text = "Adresse", FontWeight = FontWeights.Bold, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };



            Grid.SetRow(headerPosition, 0);
            Grid.SetColumn(headerPosition, 0);
            Grid.SetRow(headerFirma, 0);
            Grid.SetColumn(headerFirma, 2);
            Grid.SetRow(headerAnsprechpartner, 0);
            Grid.SetColumn(headerAnsprechpartner, 1);
            Grid.SetRow(headerAdresse, 0);
            Grid.SetColumn(headerAdresse, 3);


            grid.Children.Add(headerPosition);
            grid.Children.Add(headerFirma);
            grid.Children.Add(headerAnsprechpartner);
            grid.Children.Add(headerAdresse);
            
        }

        private void AddGridRows(Grid grid, List<ParticipantsData> participantlist)
        {
            int rowIndex = 1;

            foreach (var sponsor in participantlist)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                TextBlock textPosition = new TextBlock { Text = sponsor.Position.ToString("N0"), Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
                TextBlock textFirma = new TextBlock { Text = sponsor.Firma, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
                TextBlock textAnsprechpartner = new TextBlock { Text = sponsor.Ansprechpartner, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
                TextBlock textAdresse = new TextBlock { Text = sponsor.Adresse, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };


                Grid.SetRow(textPosition, rowIndex);
                Grid.SetColumn(textPosition, 0);
                Grid.SetRow(textFirma, rowIndex);
                Grid.SetColumn(textFirma, 2);
                Grid.SetRow(textAnsprechpartner, rowIndex);
                Grid.SetColumn(textAnsprechpartner, 1);
                Grid.SetRow(textAdresse, rowIndex);
                Grid.SetColumn(textAdresse, 3);

                grid.Children.Add(textPosition);
                grid.Children.Add(textFirma);
                grid.Children.Add(textAnsprechpartner);
                grid.Children.Add(textAdresse);
                

                rowIndex++;
            }
        }
    }
}
