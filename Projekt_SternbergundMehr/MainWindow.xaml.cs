using System.Windows;

namespace Projekt_SternbergundMehr
{

    public partial class MainWindow : Window
    {

        private int attempts = 0; // Variable, um die Anzahl der Versuche zu zählen
        private const int maxAttempts = 3; // Maximale Anzahl der erlaubten Versuche
        public MainWindow()
        {

            /*  Login-function
            login_window login_Window = new login_window();


            if (login_Window.ShowDialog() == true)
            {
                InitializeComponent();
            }

            else
            {

                Application.Current.Shutdown();
            }
            */

            InitializeComponent();


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

        private void mitlgieder_nav_Click(object sender, RoutedEventArgs e)
        {
            Window mitglied_window = new Window_Mitgliederverwaltung();

            mitglied_window.Show();
        }

        private void help_nav_Kopieren_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
