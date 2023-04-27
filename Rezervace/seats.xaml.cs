using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rezervace
{
    public partial class Seats : Window
    {
        private List<int[]> takenSeats = new List<int[]>();
        private int fontsize = 19;
        private string Uuid;
        public Seats(int rows, int columns, string uuid)
        {
            Uuid = uuid;
            InitializeComponent();
            CreateRowsAndColums(rows, columns);
            CreateButtons(rows, columns, uuid);

        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Int32.TryParse(btn.Tag.ToString(), out int tag);
            Int32.TryParse(btn.Content.ToString(), out int content);
            int[] seat = new int[2];
            seat[0] = tag;
            seat[1] = content;
            takenSeats.Add(seat);
            confirm cwindow = new confirm(takenSeats, Uuid);
            cwindow.ShowDialog();
            dbload db = new dbload();
            var database = db.DBLoad();
            var stav = database.Query<Seat>("select * from Seat");
            Int32.TryParse(btn.Tag.ToString(), out int row);
            Int32.TryParse(btn.Content.ToString(), out int column);
            foreach (Seat s in stav)
            {
                if (row == s.SeatRow && column == s.SeatColumn)
                {
                    if (s.Stav == "Prodáno na místě")
                    {
                        btn.Background = Brushes.Pink;
                    }
                    if (s.Stav == "Rezervovat")
                    {
                        btn.Background = Brushes.Red;
                    }
                    if (s.Stav == "Nedostupný")
                    {
                        btn.Background = Brushes.Gray;
                    }
                }
            }
            takenSeats.Clear();
        }

        void CreateRowsAndColums(int rows, int columns)
        {
            for (int i = 0; i < rows+1; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < columns +2; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        void CreateButtons(int rows, int columns, string uuid)
        {
            Label label = new Label()
            {
                Content = "Platno",
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                FontWeight = FontWeights.Bold,
                FontSize = fontsize,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,

            };
            Grid.SetColumnSpan(label, 24);
            Grid.SetRow(label, 0);
            grid.Children.Add(label);
            for (int i = 1; i < rows + 1; i++)
            {
                for (int j = 0; j < columns+2; j++)
                {
                    if (j == 0 || j == columns + 1)
                    {
                        Label lbl = new Label();
                        lbl.Content = i;
                        Grid.SetColumn(lbl, j);
                        Grid.SetRow(lbl, i);
                        lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                        grid.Children.Add(lbl);
                    }
                    else
                    {
                        dbload db = new dbload();
                        var database = db.DBLoad();
                        var stav = database.Query<Seat>("Select * from Seat");
                        Button button = new Button()
                        {
                            Content = string.Format(j.ToString()),
                            FontSize = 10,
                            Tag = i,
                            Margin = new Thickness(5),
                            BorderBrush = Brushes.Blue,
                        };
                        Int32.TryParse(button.Tag.ToString(), out int row);
                        Int32.TryParse(button.Content.ToString(), out int column);
                        foreach (Seat seat in stav)
                        {
                            if (row == seat.SeatRow && column == seat.SeatColumn && uuid == seat.Uuid)
                            {
                                if (seat.Stav == "Prodáno na místě")
                                {
                                    button.Background = Brushes.Pink;
                                }
                                if (seat.Stav == "Rezervovat")
                                {
                                    button.Background = Brushes.Red;
                                }
                                if (seat.Stav == "Nedostupný")
                                {
                                    button.Background = Brushes.Gray;
                                }
                            }     
                        }
                        Grid.SetRow(button, i);
                        Grid.SetColumn(button, j);
                        button.Click += new RoutedEventHandler(button_Click);
                        this.grid.Children.Add(button);
                    }

                }
            }
        }
    }
}
