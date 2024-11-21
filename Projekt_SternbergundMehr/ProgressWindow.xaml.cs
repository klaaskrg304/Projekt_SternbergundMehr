using System.Windows;

namespace Projekt_SternbergundMehr
{
    /// <summary>
    /// Interaktionslogik für ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        // Expose the ProgressBar for external updates
        public void SetProgress(double value)
        {
            progressBar.Value = value;
        }

        // Optionally, update the maximum value if needed
        public void SetMaximum(double max)
        {
            progressBar.Maximum = max;
        }
    }
}

