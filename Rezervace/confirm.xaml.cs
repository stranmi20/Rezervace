using System;
using System.Collections.Generic;
using System.Linq;
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
        public confirm()
        {
            InitializeComponent();
        }
        public confirm(List<int[]> takenseats)
        {
            for (int i = 0; i < 12; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < 24; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < takenseats.Count; i++)
            {
                Label lbl = new Label();
                lbl.Content = takenseats[i];
                lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                grid.Children.Add(lbl);
            }
        }
    }
}
