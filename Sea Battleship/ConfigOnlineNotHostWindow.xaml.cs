using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Common;
using Core;
using Sea_Battleship.Engine;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для ConfigOnlineNotHostWindow.xaml
    /// </summary>
    public partial class ConfigOnlineNotHostWindow : Window
    {
        private PlacementState _placement = PlacementState.Manualy;
        public OnlineGame OnlineGame { get; set; }
        private ShipArrangement _shipArrangement;

        public ConfigOnlineNotHostWindow()
        {
            InitializeComponent();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OnlineGame = new OnlineGame(PlayerRole.Client, _placement, IPAddress.Parse(KeyTextBox.Text));
                OnlineGame.GoToGameWindow(_placement, _shipArrangement, Owner);
                Close();
            }
            catch (Exception exception)
            {
                LogService.Trace($"Невозможно подключиться: {exception.Message}");
                MessageBox.Show(exception.Message, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            Owner.Show();
            Close();
        }

        private void Placement_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            switch(button.Content)
            {
                case "Ручной":
                _placement = PlacementState.Manualy;
                break;
                case "Случайный":
                _placement = PlacementState.Randomly;
                _shipArrangement = ShipArrangement.Random();
                break;
                case "По стратегии":
                _shipArrangement = ShipArrangement.Strategy();
                _placement = PlacementState.Strategily;
                break;
            }
        }
    }
}
