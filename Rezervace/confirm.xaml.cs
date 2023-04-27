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
        private bool readytoclose = false;
        private int seatrow;
        private int seatcolumn;
        private string uuid;
        private bool newseat = true;
        private Seat existseat;
        private Seat deleteseat;

        public confirm(List<int[]> takenseats, string uuid)
        {
            InitializeComponent();
            foreach (var tk in takenseats)
            {
                seatrow = tk[0];
                seatcolumn = tk[1];
                seats.Content += $"Ř:{tk[0]} Č:{tk[1]}, ";
            }
            this.uuid = uuid;
            dbload db = new dbload();
            var database = db.DBLoad();
            var stav = database.Query<Seat>("select * from Seat");
            foreach (var seat in stav)
            {
                if (seat.SeatColumn == seatcolumn && seat.SeatRow == seatrow && seat.Uuid == uuid)
                {
                    existseat = database.Query<Seat>("select * from Seat where SeatRow = ? AND SeatColumn = ?", seat.SeatColumn, seat.SeatRow).FirstOrDefault();
                    newseat = false;
                }

            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            dbload db = new dbload();
            var database = db.DBLoad();
            var sa = ComboBox.SelectedItem;
            if (ComboBox.SelectionBoxItem.ToString() == "Smazat")
            {
                database.Query<Seat>("DELETE from Seat where SeatRow = ? AND SeatColumn = ? AND Uuid = ?", seatrow, seatcolumn, uuid).FirstOrDefault();
                database.Query<Seat>("DELETE from Reservation where SeatRow = ? AND SeatColumn = ? AND Uuid = ?", seatrow, seatcolumn, uuid).FirstOrDefault();
                window.Close();
            }
            else
            {
                if (ComboBox.SelectionBoxItem.ToString() == "Rezervovat")
                {
                    if (nameBox.Text != "" && IsValid(emailBox.Text))
                    {
                        readytoclose = true;
                    }
                    else
                    {
                        error.Visibility = Visibility.Visible;
                    }
                    if (readytoclose)
                    {
                        if (newseat)
                        {
                            Seat seat = new Seat();
                            seat.SeatColumn = seatcolumn;
                            seat.SeatRow = seatrow;
                            seat.Stav = ComboBox.SelectionBoxItem.ToString();
                            seat.Uuid = uuid;

                            Reservation reservation = new Reservation();
                            reservation.Uuid = uuid;
                            reservation.SeatRow = seatrow;
                            reservation.SeatColumn = seatcolumn;
                            reservation.Email = emailBox.Text;
                            reservation.Name = nameBox.Text;
                            database.Insert(seat);
                            database.Insert(reservation);
                        }
                        else
                        {
                            existseat.SeatColumn = seatcolumn;
                            existseat.SeatRow = seatrow;
                            existseat.Stav = ComboBox.SelectionBoxItem.ToString();
                            existseat.Uuid = uuid;

                            Reservation reservation = new Reservation();
                            reservation.Uuid = uuid;
                            reservation.SeatRow = seatrow;
                            reservation.SeatColumn = seatcolumn;
                            reservation.Email = emailBox.Text;
                            reservation.Name = nameBox.Text;

                            database.Update(existseat);
                            database.Update(reservation);
                        }
                        window.Close();
                    }
                }
                else
                {
                    if (newseat)
                    {
                        Seat seat = new Seat();
                        seat.SeatColumn = seatcolumn;
                        seat.SeatRow = seatrow;
                        seat.Stav = ComboBox.SelectionBoxItem.ToString();
                        seat.Uuid = uuid;

                        database.Insert(seat);
                    }
                    else
                    {
                        existseat.SeatColumn = seatcolumn;
                        existseat.SeatRow = seatrow;
                        existseat.Stav = ComboBox.SelectionBoxItem.ToString();
                        existseat.Uuid = uuid;
                        database.Update(existseat);
                    }
                    window.Close();
                }
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

        private void window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
