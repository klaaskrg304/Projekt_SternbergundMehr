using System;
using System.Collections.Generic;
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

