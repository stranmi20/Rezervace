using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Newtonsoft.Json;
using SQLite;

namespace Rezervace
{
    /// <summary>
    /// Interakční logika pro Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private List<Film> films = new List<Film>();
        public Window1()
        {
            InitializeComponent();
            films = GetFilms();
            lvFilms.ItemsSource = films;
        }

        public List<Film> GetFilms()
        {
            using (StreamReader r = new StreamReader("films.json"))
            {
                string json = r.ReadToEnd();
                films = JsonConvert.DeserializeObject<List<Film>>(json);
            }
            return films;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;

            if (item != null && item.IsSelected)
            {
                var s = item.Content;
                var cinema = s.GetType().GetProperty("cinema").GetValue(s, null);
                var rows = cinema.GetType().GetProperty("rows").GetValue(cinema, null);
                var columns = cinema.GetType().GetProperty("columns").GetValue(cinema, null);
                Int32.TryParse(rows.ToString(), out int row);
                Int32.TryParse(columns.ToString(), out int column);
                

                Seats seats = new Seats(row, column);
                seats.Show();
            }
        }
    }


    public class Cinema
    {
        public string name { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }
    }
    public class Film
    {
        public string Uuid { get; set; }
        public string Name { get; set; }

        public string Date { get; set; }

        public Cinema cinema { get; set; }



    }
}
