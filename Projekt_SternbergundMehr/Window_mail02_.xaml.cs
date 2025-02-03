using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Projekt_SternbergundMehr
{
    /// <summary>
    /// Interaktionslogik für Window_mail02_.xaml
    /// </summary>
    public partial class Window_mail02_ : Window
    {
        private string attachmentPath = string.Empty; 

        public Window_mail02_()
        {
            InitializeComponent();
                  

    }

    private void dataGrid_mail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_send_Click(object sender, RoutedEventArgs e)
        {
            send_mail();
        }

        private void send_mail()
        {
            try
            {
                Outlook._Application _app = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                mail.To = tbx_adress.Text;
                mail.Subject = tbx_subject.Text;
                mail.Body = tbx_message.Text;
                mail.Importance = Outlook.OlImportance.olImportanceNormal;

                // Anhang hinzufügen, falls vorhanden
                if (!string.IsNullOrEmpty(attachmentPath))
                {
                    mail.Attachments.Add(attachmentPath);
                }

                ((Outlook._MailItem)mail).Send();
                MessageBox.Show("Deine E-Mail wurde erfolgreich versandt.", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_attach_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                attachmentPath = openFileDialog.FileName;
                MessageBox.Show($"Anhang hinzugefügt: {attachmentPath}", "Attachment", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
