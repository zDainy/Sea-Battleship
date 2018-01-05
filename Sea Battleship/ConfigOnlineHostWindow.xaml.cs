using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Common;
using Core;
using Network;
using Sea_Battleship.Engine;
using ShipArrangement = Core.ShipArrangement;
using System;

namespace Sea_Battleship
{
    /// <summary>
    /// Логика взаимодействия для ConfigOnlineHostWindow.xaml
    /// </summary>
    public partial class ConfigOnlineHostWindow : Window
    {
        private GameSpeed _gameSpeed = GameSpeed.Fast;
        private PlacementState _placement = PlacementState.Manualy;
        public OnlineGame OnlineGame { get; set; }
        private ShipArrangement _shipArrangement;

        public ConfigOnlineHostWindow()
        {
            InitializeComponent();
            OnlineGame = new OnlineGame(PlayerRole.Server, _placement);
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            OnlineGame.SetGameSettings(new GameConfig(PlayerRole.Server, "", _gameSpeed));

            if (OnlineGame.Connect.Server.IsClientConnected)
            {
               // OnlineGame.GoToGameWindow(_placement, _shipArrangement, Owner);
                if (_placement != PlacementState.Manualy)
                {
                    OnlineGame.CreateGame(_shipArrangement);
                    PlayPage window = new PlayPage(OnlineGame) ;
                    WindowConfig.MainPage.NavigationService.Navigate(window, UriKind.Relative);
                }
                else
                {
                    PlacingPage window = new PlacingPage(OnlineGame);
                    WindowConfig.MainPage.NavigationService.Navigate(window, UriKind.Relative);
                }
                Close();
            }
            else
            {
                WaitingWindow window = new WaitingWindow(OnlineGame, _shipArrangement, _placement);
                window.Show();
                window.Wait();
                Close();
            }
        }

        private void ButtonPrev_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void TimeLength_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            switch (button.Content)
            {
                case "30 секунд":
                    _gameSpeed = GameSpeed.Fast;
                    break;
                case "1 минута":
                    _gameSpeed = GameSpeed.Medium;
                    break;
                case "2 минуты":
                    _gameSpeed = GameSpeed.Slow;
                    break;
                case "5 минут":
                    _gameSpeed = GameSpeed.Turtle;
                    break;
            }
        }

        private void Placement_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            switch (button.Content)
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = ServerUtils.IPToString(ServerUtils.GetExternalIp());
        }
    }
}
