using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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

namespace Rezervace
{
    /// <summary>
    /// Interakční logika pro confirm.xaml
    /// </summary>
    public partial class confirm : Window
    {
        private bool readytoclos = false;
        private bool emailvalid = false;
        public confirm(List<int[]> takenseats)
        {
            InitializeComponent();
            foreach(var tk in takenseats)
            {
                seats.Content += $"Ř:{tk[0]} Č:{tk[1]}, ";
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            if (nameBox.Text != "" && IsValid(emailBox.Text))
            {
                readytoclos = true;
            }
            else
            {
                error.Visibility = Visibility.Visible;
            }
            if (readytoclos)
            {
                window.Close();
            }
        }

        private static bool IsValid(string email)
        {
            bool valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
    }
}
