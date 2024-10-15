using Npgsql;

using System;
using System.Collections.Generic;
using System.Data;
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
    

    public partial class login_window : Window
    {

        private DBConnection dbConnection;

        public login_window()
        {
            InitializeComponent();
            hash hash = new hash();
            dbConnection = new DBConnection();


            

        }

        private int attempts = 0; // Variable, um die Anzahl der Versuche zu zählen
        private const int maxAttempts = 3; // Maximale Anzahl der erlaubten Versuche




        private void btn_hash_Click(object sender, RoutedEventArgs e)
        {
            // hash.HashPassword(tbx_hash.Text);
            //tbx_hash.Text = hash.HashPassword("passwort");
            string enteredpasswored = tbx_hash.Text;
            string storedhash = dbConnection.get_Hash();
            hash.VerifyPassword(enteredpasswored, storedhash);

            if(hash.VerifyPassword(enteredpasswored, storedhash)==true)
            {
                this.DialogResult = true;
                MessageBox.Show($"Login erfolgreich!");
            }

            else
            {
                attempts++; 
                if (attempts >= maxAttempts)
                {
                    MessageBox.Show("Zu viele fehlgeschlagene Versuche! Der Zugang ist gesperrt.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close(); // Fenster schließen oder andere Sperrmaßnahmen ergreifen
                }
                else
                {
                    MessageBox.Show($"Falsches Passwort! Versuche verbleibend: {maxAttempts - attempts}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                }

               
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
