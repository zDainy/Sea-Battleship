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
        public ConfigOnlineHostWindow()
        {
            InitializeComponent();
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
    }
}
