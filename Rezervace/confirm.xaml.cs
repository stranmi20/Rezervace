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
    public partial class confirm : Window
    {
        // PROMĚNNÝ
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

            // VYPSANÍ SEDADLA
            foreach (var tk in takenseats)
            {
                seatrow = tk[0];
                seatcolumn = tk[1];
                seats.Content += $"Ř:{tk[0]} Č:{tk[1]}, ";
            }
            this.uuid = uuid;
            // LOAD DATABASE
            dbload db = new dbload();
            var database = db.DBLoad();
            // VYBRANÍ VŠECH SEDACĚK Z DB
            var stav = database.Query<Seat>("select * from Seat");

            foreach (var seat in stav)
            {
                // POKUD JE SEDADLO UŽ V DB
                if (seat.SeatColumn == seatcolumn && seat.SeatRow == seatrow && seat.Uuid == uuid)
                {
                    existseat = database.Query<Seat>("select * from Seat where SeatRow = ? AND SeatColumn = ?", seat.SeatColumn, seat.SeatRow).FirstOrDefault();
                    newseat = false;
                }

            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            // LOAD DB
            dbload db = new dbload();
            var database = db.DBLoad();
            var sa = ComboBox.SelectedItem;

            // SMAZANÍ SEDADLA
            if (ComboBox.SelectionBoxItem.ToString() == "Smazat")
            {
                database.Query<Seat>("DELETE from Seat where SeatRow = ? AND SeatColumn = ? AND Uuid = ?", seatrow, seatcolumn, uuid).FirstOrDefault();
                database.Query<Seat>("DELETE from Reservation where SeatRow = ? AND SeatColumn = ? AND Uuid = ?", seatrow, seatcolumn, uuid).FirstOrDefault();
                window.Close();
            }
            else
            {
                // REZERVACE SEDADLA
                if (ComboBox.SelectionBoxItem.ToString() == "Rezervovat")
                {
                    // POKUD VYPLNIL VŠE
                    if (nameBox.Text != "" && IsValid(emailBox.Text))
                    {
                        readytoclose = true;
                    }
                    // POKUD NE
                    else
                    {
                        error.Visibility = Visibility.Visible;
                    }
                    if (readytoclose)
                    {
                        // NOVÉ SEDADLO
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
                        // UPDATE SEDADLA
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
                        // ZAVŘENÍ OKNA
                        window.Close();
                    }
                }
                // POKUD SE NEJEDNÁ O REZERVACI
                else
                {
                    // NOVÉ SEDADLO
                    if (newseat)
                    {
                        Seat seat = new Seat();
                        seat.SeatColumn = seatcolumn;
                        seat.SeatRow = seatrow;
                        seat.Stav = ComboBox.SelectionBoxItem.ToString();
                        seat.Uuid = uuid;

                        database.Insert(seat);
                    }
                    // UPDATE SEDADLA
                    else
                    {
                        existseat.SeatColumn = seatcolumn;
                        existseat.SeatRow = seatrow;
                        existseat.Stav = ComboBox.SelectionBoxItem.ToString();
                        existseat.Uuid = uuid;
                        database.Update(existseat);
                    }
                    // ZAVŘENÍ OKNA
                    window.Close();
                }
            }
       
        }

        // FUNKCE NA VALIDACI EMAILU
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
