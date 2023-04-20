﻿using System;
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
        public Seats(int rows, int columns)
        {
            InitializeComponent();
            CreateRowsAndColums(rows, columns);
            CreateButtons(rows, columns);

        }
        void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.Red ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Red;
            if (btn.Background == Brushes.Red)
            {
                Int32.TryParse(btn.Tag.ToString(), out int tag);
                Int32.TryParse(btn.Content.ToString(), out int content);
                int[] seat = new int[2];
                seat[0] = tag;
                seat[1] = content;
                takenSeats.Add(seat);
            }
            else
            {
                Int32.TryParse(btn.Tag.ToString(), out int tag);
                Int32.TryParse(btn.Content.ToString(), out int content);
                int[] seat = new int[2];
                seat[0] = tag;
                seat[1] = content;
                int index = getIndex(takenSeats, seat);
                takenSeats.RemoveAt(index);
            }
        }

        private int getIndex(List<int[]> takenseats, int[] seat)
        {
            int index = 0;
            foreach (int[] tk in takenseats)
            {
                if (tk[0] == seat[0] && tk[1] == seat[1]) { return index; }
                index++;
            }
            return index;
        }

        void CreateRowsAndColums(int rows, int columns)
        {
            for (int i = 0; i < rows+2; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < columns +2; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        void CreateButtons(int rows, int columns)
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
                        Button button = new Button()
                        {
                            Content = string.Format(j.ToString()),
                            FontSize = 10,
                            Tag = i,
                            Margin = new Thickness(5),
                            BorderBrush = Brushes.Blue,
                        };

                        Grid.SetRow(button, i);
                        Grid.SetColumn(button, j);
                        button.Click += new RoutedEventHandler(button_Click);
                        this.grid.Children.Add(button);
                    }

                }
            }

            Button subButton = new Button()
            {
                Content = "Submit",
                FontSize = 10
            };
            Grid.SetRow(subButton, rows + 2);
            Grid.SetColumn(subButton, columns/2);
            Grid.SetColumnSpan(subButton, 2);
            subButton.Click += new RoutedEventHandler(subButton_Click);
            grid.Children.Add(subButton);
        }

        void subButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmwindowready = true;
            if (takenSeats.Count == 0)
            {
                confirmwindowready = false;
            }
            confirm cwindow = new confirm(takenSeats);
            if (confirmwindowready)
            {
                cwindow.Show();
            }
        }
    }
}
