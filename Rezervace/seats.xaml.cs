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
        // PROMĚNNÝ
        private List<int[]> takenSeats = new List<int[]>();
        private int fontsize = 19;
        private string Uuid;
        public Seats(int rows, int columns, string uuid)
        {
            Uuid = uuid;
            InitializeComponent();
            // VYTVOŘENÍ GRIDU
            CreateRowsAndColums(rows, columns);

            // VYTVOŘENÍ SEDAČEK
            CreateButtons(rows, columns, uuid);

        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            // BRANÍ SI SLOUPEC A ŘADU SEDAČKY (TAG - ŘADA, CONTENT - SLOUPEC) A PŘIDANÍ DO LISTU
            Button btn = sender as Button;
            Int32.TryParse(btn.Tag.ToString(), out int tag);
            Int32.TryParse(btn.Content.ToString(), out int content);
            int[] seat = new int[2];
            seat[0] = tag;
            seat[1] = content;
            takenSeats.Add(seat);

            // OTEVŘENÍ CONFIRM OKNA
            confirm cwindow = new confirm(takenSeats, Uuid);
            cwindow.ShowDialog();

            // AŽ SE OKNO ZAVŘE

            // NAČTENÍ DB
            dbload db = new dbload();
            var database = db.DBLoad();

            // SELECT VŠECH SEDAČEK Z DB
            var stav = database.Query<Seat>("select * from Seat");

            // SLOUPEC, ŘADA
            Int32.TryParse(btn.Tag.ToString(), out int row);
            Int32.TryParse(btn.Content.ToString(), out int column);

            // NASTAVENÍ BARVY SEDAČKY
            btn.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD"));
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
            // VYČISTĚNÍ LISTU
            takenSeats.Clear();
        }


        // TVORBA GRIDU
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


        // TVORBA SEDAČEK
        void CreateButtons(int rows, int columns, string uuid)
        {
            // PLATNO
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
                    // ČÍSLA PRO ULIČKY
                    if (j == 0 || j == columns + 1)
                    {
                        Label lbl = new Label();
                        lbl.Content = i;
                        Grid.SetColumn(lbl, j);
                        Grid.SetRow(lbl, i);
                        lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                        grid.Children.Add(lbl);
                    }
                    // SEDAČKY
                    else
                    {
                        // LOAD DB
                        dbload db = new dbload();
                        var database = db.DBLoad();
                        
                        // SELECT VŠECH SEDAČEK Z DB
                        var stav = database.Query<Seat>("Select * from Seat");

                        // TVORBA SEDAČKY
                        Button button = new Button()
                        {
                            Content = string.Format(j.ToString()),
                            FontSize = 10,
                            Tag = i,
                            Margin = new Thickness(5),
                            BorderBrush = Brushes.Blue,
                        };

                        // SLOUPEC, ŘADA
                        Int32.TryParse(button.Tag.ToString(), out int row);
                        Int32.TryParse(button.Content.ToString(), out int column);

                        foreach (Seat seat in stav)
                        {
                            // POKUD JE SEDADLO UŽ V DB
                            if (row == seat.SeatRow && column == seat.SeatColumn && uuid == seat.Uuid)
                            {
                                // PŘÍDANÍ BARVY
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
