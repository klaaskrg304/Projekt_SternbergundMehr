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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_SternbergundMehr
{
    
    public partial class MainWindow : Window
    {

        private int attempts = 0; // Variable, um die Anzahl der Versuche zu zählen
        private const int maxAttempts = 3; // Maximale Anzahl der erlaubten Versuche
        public MainWindow()
        {
           

            login_window login_Window = new login_window();
            

            if(login_Window.ShowDialog()==true)
            {
                InitializeComponent();
            }

            else
            {

                Application.Current.Shutdown();
            }
            

           
        }

        private void sponsor_nav_Click(object sender, RoutedEventArgs e)
        {
            

            Window_sponsors window_Sponsors = new Window_sponsors();

            

            window_Sponsors.Show();

           

           
                
            
        }

        private void umzug_nav_Click(object sender, RoutedEventArgs e)
        {
            Window_Umzug window_Umzug = new Window_Umzug();
            
            window_Umzug.Show();
        }

        private void mail_nav_Click(object sender, RoutedEventArgs e)
        {
           
            Window_mail window_mail = new Window_mail();
            window_mail.Show();

            
            
        }

        private void login_nav_Click(object sender, RoutedEventArgs e)
        {
            login_window login_Window = new login_window();
            login_Window.ShowDialog();
        }

        private void btn_home_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
