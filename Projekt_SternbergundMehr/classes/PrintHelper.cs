using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

public class PrintHelper
{
    private DataGrid dataGrid_sponsoren;

    public PrintHelper(DataGrid dataGrid)
    {
        dataGrid_sponsoren = dataGrid;
    }

    ~PrintHelper() { }

    public List<SponsorData> ExtractDataFromDataGrid()
    {
        List<SponsorData> sponsorList = new List<SponsorData>();

        foreach (var item in dataGrid_sponsoren.Items)
        {
            if (item is SponsorData sponsorData)
            {
                sponsorList.Add(sponsorData);
            }
        }

        return sponsorList;
    }

    public FixedDocument CreatePrintableDocument(List<SponsorData> sponsorList)
    {
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
            Text = "Sponsorenliste",
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
        AddGridRows(grid, sponsorList);

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

        // Gesamtsumme der Beträge
        double totalAmount = sponsorList.Sum(sponsor => sponsor.Betrag);
        TextBlock totalAmountText = new TextBlock
        {
            Text = "Gesamtsumme: " + totalAmount.ToString("C", CultureInfo.GetCultureInfo("de-DE")),
            FontSize = 16,
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(0, 10, 0, 0),
            TextAlignment = TextAlignment.Right,
            Width = 727 // Breite des A4-Blattes minus Ränder (827 - 2*50)
        };
        printPanel.Children.Add(totalAmountText);

        fixedPage.Children.Add(printPanel);

        PageContent pageContent = new PageContent();
        ((IAddChild)pageContent).AddChild(fixedPage);
        fixedDoc.Pages.Add(pageContent);

        return fixedDoc;
    }

    private void AddGridHeader(Grid grid)
    {
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        TextBlock headerFirma = new TextBlock { Text = "Firma", FontWeight = FontWeights.Bold, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
        TextBlock headerAnsprechpartner = new TextBlock { Text = "Ansprechpartner", FontWeight = FontWeights.Bold, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
        TextBlock headerAdresse = new TextBlock { Text = "Adresse", FontWeight = FontWeights.Bold, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
        TextBlock headerBetrag = new TextBlock { Text = "Betrag", FontWeight = FontWeights.Bold, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };

        Grid.SetRow(headerFirma, 0);
        Grid.SetColumn(headerFirma, 0);
        Grid.SetRow(headerAnsprechpartner, 0);
        Grid.SetColumn(headerAnsprechpartner, 1);
        Grid.SetRow(headerAdresse, 0);
        Grid.SetColumn(headerAdresse, 2);
        Grid.SetRow(headerBetrag, 0);
        Grid.SetColumn(headerBetrag, 3);

        grid.Children.Add(headerFirma);
        grid.Children.Add(headerAnsprechpartner);
        grid.Children.Add(headerAdresse);
        grid.Children.Add(headerBetrag);
    }

    private void AddGridRows(Grid grid, List<SponsorData> sponsorList)
    {
        int rowIndex = 1;

        foreach (var sponsor in sponsorList)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            TextBlock textFirma = new TextBlock { Text = sponsor.Firma, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
            TextBlock textAnsprechpartner = new TextBlock { Text = sponsor.Ansprechpartner, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
            TextBlock textAdresse = new TextBlock { Text = sponsor.Adresse, Margin = new Thickness(5), TextAlignment = TextAlignment.Center };
            TextBlock textBetrag = new TextBlock { Text = sponsor.Betrag.ToString("N0") + " €", Margin = new Thickness(5), TextAlignment = TextAlignment.Center };

            Grid.SetRow(textFirma, rowIndex);
            Grid.SetColumn(textFirma, 0);
            Grid.SetRow(textAnsprechpartner, rowIndex);
            Grid.SetColumn(textAnsprechpartner, 1);
            Grid.SetRow(textAdresse, rowIndex);
            Grid.SetColumn(textAdresse, 2);
            Grid.SetRow(textBetrag, rowIndex);
            Grid.SetColumn(textBetrag, 3);

            grid.Children.Add(textFirma);
            grid.Children.Add(textAnsprechpartner);
            grid.Children.Add(textAdresse);
            grid.Children.Add(textBetrag);

            rowIndex++;
        }
    }
}
