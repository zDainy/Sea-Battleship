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
    /// Логика взаимодействия для ConfigOnlineNotHostWindow.xaml
    /// </summary>
    public partial class ConfigOnlineNotHostWindow : Window
    {
        private PlacementState placement = PlacementState.Manualy;
        public ConfigOnlineNotHostWindow()
        {
            InitializeComponent();
            
        }
        enum PlacementState
        {
            Manualy,
            Randomly,
            Strategily
        }


        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            PlayWindow window = new PlayWindow();
            window.Owner = this.Owner;
            window.Show();
            Close();
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

        private void Placement_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            switch (button.Content)
            {
                case "Ручной":
                    placement = PlacementState.Manualy;
                    break;
                case "Случайный":
                    placement = PlacementState.Randomly;
                    break;
                case "По стратегии":
                    placement = PlacementState.Strategily;
                    break;
            }
        }
    }
}
