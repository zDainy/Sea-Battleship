using System;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Common;
using Core;
using Network;
using Sea_Battleship.Engine;
using ShipArrangement = Core.ShipArrangement;

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
                OnlineGame = new OnlineGame(PlayerRole.Client, _placement, ServerUtils.StringToIP(KeyTextBox.Text));
                Thread.Sleep(2000);
                if (_placement != PlacementState.Manualy)
                {
                    OnlineGame.CreateGame(_shipArrangement);
                    PlayPage window = new PlayPage(OnlineGame);
                    WindowConfig.MainPage.NavigationService.Navigate(window, UriKind.Relative);
                }
                else
                {
                    PlacingPage window = new PlacingPage(OnlineGame);
                    WindowConfig.MainPage.NavigationService.Navigate(window, UriKind.Relative);
                }
                Close();
            }
            catch (IndexOutOfRangeException exception)
            {
                LogService.Trace($"Невозможно подключиться: {exception.Message}");
                MessageBox.Show("Вы ввели неправильный ключ. Повторите попытку", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception exception)
            {
                LogService.Trace($"Невозможно подключиться: {exception.Message}");
                MessageBox.Show($"Ошибка подключения к серверу", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
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

        private void Type_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            Grid grid = (Grid)button.Parent;
            switch (button.Content)
            {
                case "Новая":
                    foreach (var el in grid.Children)
                    {
                        if ((el is RadioButton) && (((RadioButton)el).GroupName == "Placement"))
                        {
                            ((RadioButton)el).IsEnabled = true;
                        }
                    }
                    break;
                case "Сохранённая":
                    foreach (var el in grid.Children)
                    {
                        if ((el is RadioButton)&& (((RadioButton)el).GroupName   == "Placement"))
                        {
                            ((RadioButton)el).IsEnabled = false;
                            _placement = PlacementState.Randomly;
                        }
                    }
                    break;
            }
        }
    }
}
