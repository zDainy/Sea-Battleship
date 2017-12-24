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
    /// Логика взаимодействия для ConfigOnlineHostWindow.xaml
    /// </summary>
    public partial class ConfigOnlineHostWindow : Window
    {
        TimeLengthState timeLength = TimeLengthState.Fast;
        PlacementState placement = PlacementState.Manualy;

        public ConfigOnlineHostWindow()
        {
            InitializeComponent();
            

        }

        enum TimeLengthState
        {
            Fast,
            Medium,
            Slow,
            Turtle
        }

        enum PlacementState
        {
            Manualy,
            Randomly,
            Strategily
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
           
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
        private void TimeLength_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            switch (button.Content)
            {
                case "30 секунд":
                    timeLength = TimeLengthState.Fast;
                    break;
                case "1 минута":
                    timeLength = TimeLengthState.Medium;
                    break;
                case "2 минуты":
                    timeLength = TimeLengthState.Slow;
                    break;
                case "5 минут":
                    timeLength = TimeLengthState.Turtle;
                    break;
            }
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           // TextBox textBox = (TextBox)sender;
            //  textBox.Text;
        }
    }
}
