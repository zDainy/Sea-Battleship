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

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для PlacingWindow.xaml
    /// </summary>
    public partial class PlacingWindow : Window
    {
        public PlacingWindow()
        {
            InitializeComponent();

        }
        private void audioChanged(object sender, RoutedEventArgs e)
        {
            WindowConfig.AudioChanged((Image)sender);
        }

        private void Mouse_Down(object sender, MouseButtonEventArgs e) => WindowConfig.ShipState = WindowConfig.State.Ship4;
        private void Mouse_Down3(object sender, MouseButtonEventArgs e) => WindowConfig.ShipState = WindowConfig.State.Ship3;
    }
}
