using Core;
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
    /// Логика взаимодействия для ConfigOfflineWindow.xaml
    /// </summary>
    public partial class ConfigOfflineWindow : Window
    {
        //  private ga
        private BotLevels _bot = BotLevels.Easy;
        private GameSpeed _gameSpeed = GameSpeed.Fast;
        private PlacementState _placement = PlacementState.Manualy;

        private ShipArrangement arrangementClient;
        private GameConfig game;

        public ShipArrangement ArrangementClient { get => arrangementClient; set => arrangementClient = value; }
        public GameConfig Game { get => game; set => game = value; }

        public ConfigOfflineWindow()
        {
            InitializeComponent();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            Game = new GameConfig(_bot, _gameSpeed);
            DialogResult = true;
            ShipArrangement arrangement;
            switch (_placement)
            {
                case PlacementState.Randomly:
                    arrangement = ShipArrangement.Random();
                    if (_bot == BotLevels.Easy || _bot == BotLevels.Medium)
                        ArrangementClient = ShipArrangement.Random();
                    else
                        ArrangementClient = ShipArrangement.Strategy();
                    WindowConfig.game = new Game(arrangement, ArrangementClient, Game);
                    Close();
                    break;
                case PlacementState.Strategily:
                    arrangement = ShipArrangement.Strategy();
                    if (_bot == BotLevels.Easy || _bot == BotLevels.Medium)
                        ArrangementClient = ShipArrangement.Random();
                    else
                        ArrangementClient = ShipArrangement.Strategy();
                    WindowConfig.game = new Game(arrangement, ArrangementClient, Game);
                    Close();
                    break;
                case PlacementState.Manualy:
                    if (_bot == BotLevels.Easy || _bot == BotLevels.Medium)
                        ArrangementClient = ShipArrangement.Random();
                    else
                        ArrangementClient = ShipArrangement.Strategy();
                    Close();
                    break;
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

        private void Complexity_Click(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            switch (button.Content)
            {
                case "Лёгкая":
                    _bot = BotLevels.Easy;
                    break;
                case "Средняя":
                    _bot = BotLevels.Medium;
                    break;
                case "Сложная":
                    _bot = BotLevels.Hard;
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
                    break;
                case "По стратегии":
                    _placement = PlacementState.Strategily;
                    break;
            }
        }
    }
}
