using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rezervace
{
    public partial class MainWindow : Window
    {
        private int fontsize = 19;
        public MainWindow()
        {
            InitializeComponent();
            CreateRowsAndColums();
            CreateButtons();
            
        }
        void button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.Background = btn.Background == Brushes.Red ? (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFDDDDDD")) : Brushes.Red;
            Console.WriteLine(btn.Tag + " " + btn.Content);
        }

        void CreateRowsAndColums()
        {
            for (int i = 0; i < 12; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < 24; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        void CreateButtons()
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
            for (int i = 1; i < 12; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    if (j == 0 || j == 23)
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
        }
    }
}
