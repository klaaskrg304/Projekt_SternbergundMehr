﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Projekt_SternbergundMehr
{
    /// <summary>
    /// Interaktionslogik für Window_mail02_.xaml
    /// </summary>
    public partial class Window_mail02_ : Window
    {
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
    }
}
